using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ApiEjemplo.Data
{
    public class ConexionBD : IDisposable
    {

        #region "Atributos"
        private string strError;
        private bool blnBDAbierta;
        private string strCadenaCnx;
        private SqlConnection objCnnBD;
        private SqlCommand objCmdBD;
        private SqlDataReader objReader;
        private SqlDataAdapter dapGenerico;
        private DataSet dts;
        private string strVrUnico;
        private SqlParameter objParametro;
        #endregion
        #region "Constructor"
        public ConexionBD()
        {
            objCnnBD = new SqlConnection();
            objCmdBD = new SqlCommand();
            dapGenerico = new SqlDataAdapter();
            strVrUnico = "";
            objParametro = new SqlParameter();
            strError = "";
        }
        #endregion
        #region "Propiedades"
        public SqlDataReader Reader
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
            strCadenaCnx = "Server=tcp:sql-newlife.database.windows.net,1433;Initial Catalog=db_newLife;Persist Security Info=False;" +
                           "User ID=usuario_backend;Password=Ju.1013654544;" +
                           "MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;" +
                           "Connection Timeout=30;ConnectRetryCount=3;ConnectRetryInterval=5;" +
                           "Max Pool Size=50;Min Pool Size=2;Pooling=True;";

            for (int intento = 1; intento <= 3; intento++)
            {
                try
                {
                    objCnnBD = new SqlConnection(strCadenaCnx);
                    objCmdBD = new SqlCommand();
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
                    objCmdBD.CommandType = blnCon_Parametros ? CommandType.StoredProcedure : CommandType.Text;
                    objCmdBD.CommandText = SentenciaSQL;
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
                    objCmdBD.CommandType = blnCon_Parametros ? CommandType.StoredProcedure : CommandType.Text;
                    objCmdBD.CommandText = SentenciaSQL;
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
                    objCmdBD.CommandType = blnCon_Parametros ? CommandType.StoredProcedure : CommandType.Text;
                    objCmdBD.CommandText = SentenciaSQL;
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
            if (blnCon_Parametros)
                objCmdBD.CommandType = CommandType.StoredProcedure;
            else
                objCmdBD.CommandType = CommandType.Text;
            objCmdBD.CommandText = SentenciaSQL;
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
                objParametro.Direction = Direccion;
                objParametro.ParameterName = Nombre_En_SP;
                objParametro.SqlDbType = TipoDato;
                objParametro.Size = Tamaño;
                objParametro.Value = Valor;
                objCmdBD.Parameters.Add(objParametro);
                objParametro = new SqlParameter();
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
