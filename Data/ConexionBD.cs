using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ApiEjemplo.Data
{
    public class ConexionBD : IDisposable
    {

        #region "Atributos"
        private string strError;
        private bool blnBDAbierta;
        private NpgsqlConnection objCnnBD;
        private NpgsqlCommand objCmdBD;
        private NpgsqlDataReader objReader;
        private NpgsqlDataAdapter dapGenerico;
        private DataSet dts;
        private string strVrUnico;
        private readonly List<NpgsqlParameter> listaParametros;
        #endregion
        #region "Constructor"
        public ConexionBD()
        {
            objCnnBD = new NpgsqlConnection();
            objCmdBD = new NpgsqlCommand();
            dapGenerico = new NpgsqlDataAdapter();
            strVrUnico = "";
            listaParametros = new List<NpgsqlParameter>();
            strError = "";
        }
        #endregion
        #region "Propiedades"
        public NpgsqlDataReader Reader
        {
            get { return objReader; }
        }
        public DataSet DataSet_Retornado
        {
            get { return dts; }
        }
        public string Error
        {
            set { strError = value; }
            get { return strError; }
        }
        public string ValorUnico
        {
            get { return strVrUnico; }
        }
        #endregion
        #region "Metodos Privados"

        private bool AbrirConexion()
        {
            string strCadenaCnx = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
            if (string.IsNullOrEmpty(strCadenaCnx))
            {
                strError = "No se encontro la variable de entorno POSTGRES_CONNECTION_STRING";
                return false;
            }

            for (int intento = 1; intento <= 3; intento++)
            {
                try
                {
                    objCnnBD = new NpgsqlConnection(strCadenaCnx);
                    objCnnBD.Open();
                    blnBDAbierta = true;
                    return true;
                }
                catch (Exception ex)
                {
                    blnBDAbierta = false;
                    strError = "Error al abrir la conexion -" + ex.Message;
                    try { if (objCnnBD != null) { objCnnBD.Dispose(); objCnnBD = null; } } catch { }
                }
            }
            return false;
        }

        private bool ReconectarYReintentar()
        {
            blnBDAbierta = false;
            try
            {
                if (objReader != null && !objReader.IsClosed) { objReader.Close(); }
                objReader = null;
            }
            catch { }
            try { if (objCnnBD != null) { objCnnBD.Close(); objCnnBD.Dispose(); objCnnBD = null; } } catch { }
            return AbrirConexion();
        }

        // Las funciones PL/pgSQL declaran sus parametros con prefijo "p_" (ej. @correo -> p_correo)
        // para evitar el error "column reference is ambiguous" cuando el parametro coincide
        // con el nombre de una columna de la tabla.
        private string ConstruirLlamadaFuncion(string nombreFuncion)
        {
            var argumentos = listaParametros.Select(p => "p_" + p.ParameterName + " => @" + p.ParameterName);
            return nombreFuncion + "(" + string.Join(", ", argumentos) + ")";
        }

        private void AplicarParametros(NpgsqlCommand cmd)
        {
            cmd.Parameters.Clear();
            foreach (var p in listaParametros)
                cmd.Parameters.Add(p);
        }

        private static NpgsqlDbType MapearTipo(SqlDbType tipo)
        {
            switch (tipo)
            {
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar:
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.Text:
                    return NpgsqlDbType.Varchar;
                case SqlDbType.Int:
                case SqlDbType.SmallInt:
                case SqlDbType.TinyInt:
                    return NpgsqlDbType.Integer;
                case SqlDbType.BigInt:
                    return NpgsqlDbType.Bigint;
                case SqlDbType.Decimal:
                case SqlDbType.Money:
                    return NpgsqlDbType.Numeric;
                case SqlDbType.Date:
                    return NpgsqlDbType.Date;
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                case SqlDbType.SmallDateTime:
                    return NpgsqlDbType.Timestamp;
                case SqlDbType.Bit:
                    return NpgsqlDbType.Boolean;
                default:
                    return NpgsqlDbType.Varchar;
            }
        }
        #endregion
        #region "Metodos Publicos"
        public bool Consultar(string SentenciaSQL, bool blnCon_Parametros)
        {
            if (SentenciaSQL == "") { strError = "Error en instrucción SQL"; return false; }
            if (!blnBDAbierta && !AbrirConexion()) return false;

            for (int intento = 1; intento <= 2; intento++)
            {
                try
                {
                    objCmdBD.Connection = objCnnBD;
                    objCmdBD.CommandType = CommandType.Text;
                    objCmdBD.CommandText = blnCon_Parametros
                        ? "SELECT * FROM " + ConstruirLlamadaFuncion(SentenciaSQL)
                        : SentenciaSQL;
                    AplicarParametros(objCmdBD);
                    objReader = objCmdBD.ExecuteReader();
                    return true;
                }
                catch (Exception ex)
                {
                    strError = "Falla en ejecutar comando -" + ex.Message;
                    if (intento < 2 && ReconectarYReintentar())
                        continue;
                    return false;
                }
            }
            return false;
        }

        public bool EjecutarSentencia(string SentenciaSQL, bool blnCon_Parametros)
        {
            if (SentenciaSQL == "") { strError = "No se ha definido la sentencia a ejecutar "; return false; }
            if (!blnBDAbierta && !AbrirConexion()) return false;

            for (int intento = 1; intento <= 2; intento++)
            {
                try
                {
                    objCmdBD.Connection = objCnnBD;
                    objCmdBD.CommandType = CommandType.Text;
                    objCmdBD.CommandText = blnCon_Parametros
                        ? "SELECT " + ConstruirLlamadaFuncion(SentenciaSQL)
                        : SentenciaSQL;
                    AplicarParametros(objCmdBD);
                    objCmdBD.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    strError = "Error al ejecutar la instrucción -" + ex.Message;
                    if (intento < 2 && ReconectarYReintentar())
                        continue;
                    return false;
                }
            }
            return false;
        }

        public bool ConsultarValorUnico(string SentenciaSQL, bool blnCon_Parametros)
        {
            if (SentenciaSQL == "") { strError = "No se ha definido la sentencia a ejecutar "; return false; }
            if (!blnBDAbierta && !AbrirConexion()) return false;

            for (int intento = 1; intento <= 2; intento++)
            {
                try
                {
                    objCmdBD.Connection = objCnnBD;
                    objCmdBD.CommandType = CommandType.Text;
                    objCmdBD.CommandText = blnCon_Parametros
                        ? "SELECT " + ConstruirLlamadaFuncion(SentenciaSQL)
                        : SentenciaSQL;
                    AplicarParametros(objCmdBD);
                    strVrUnico = Convert.ToString(objCmdBD.ExecuteScalar());
                    return true;
                }
                catch (Exception ex)
                {
                    strError = "Error al ejecutar instrucción -" + ex.Message;
                    if (intento < 2 && ReconectarYReintentar())
                        continue;
                    return false;
                }
            }
            return false;
        }

        public void CerrarConexion()
        {
            try
            {
                if (objReader != null && !objReader.IsClosed)
                {
                    objReader.Close();
                    objReader = null;
                }
            }
            catch { }
            try
            {
                if (objCmdBD != null)
                {
                    objCmdBD.Parameters.Clear();
                    objCmdBD.Dispose();
                    objCmdBD = null;
                }
            }
            catch { }
            try
            {
                if (objCnnBD != null)
                {
                    objCnnBD.Close();
                    objCnnBD.Dispose();
                    objCnnBD = null;
                }
            }
            catch (Exception ex)
            {
                strError = "Falla en cerrar conexión -" + ex.Message;
            }
            blnBDAbierta = false;
        }

        public void Dispose()
        {
            CerrarConexion();
        }

        public bool LlenarDataSet(string NombreTabla, string SentenciaSQL, bool blnCon_Parametros)
        {
            if (blnBDAbierta == false)
            {
                if (AbrirConexion() == false)
                {
                    return false;
                }
            }
            objCmdBD.Connection = objCnnBD;
            objCmdBD.CommandType = CommandType.Text;
            objCmdBD.CommandText = blnCon_Parametros
                ? "SELECT * FROM " + ConstruirLlamadaFuncion(SentenciaSQL)
                : SentenciaSQL;
            AplicarParametros(objCmdBD);
            try
            {
                dts = new DataSet();
                dapGenerico.SelectCommand = objCmdBD;
                dapGenerico.Fill(dts, NombreTabla);
                return true;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

        public bool AgregarParametro(ParameterDirection Direccion, string Nombre_En_SP,
        SqlDbType TipoDato, Int16 Tamaño, object Valor)
        {
            try
            {
                var objParametro = new NpgsqlParameter
                {
                    Direction = Direccion,
                    ParameterName = Nombre_En_SP.TrimStart('@'),
                    NpgsqlDbType = MapearTipo(TipoDato),
                    Value = Valor ?? DBNull.Value
                };
                listaParametros.Add(objParametro);
                return (true);
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return (false);
            }
        }
        #endregion
    }
}
