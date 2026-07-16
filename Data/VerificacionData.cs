using ApiEjemplo.Data;
using System;
using System.Data;

namespace NewLife.Data
{
    public class VerificacionData
    {
        public static void MigrarTablaVerificacion()
        {
            string sentencia =
                "CREATE TABLE IF NOT EXISTS codigos_verificacion (" +
                "id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY, " +
                "correo VARCHAR(255) NOT NULL, " +
                "codigo VARCHAR(10) NOT NULL, " +
                "tipo VARCHAR(30) NOT NULL, " +
                "fecha_creacion TIMESTAMP NOT NULL DEFAULT now(), " +
                "fecha_expiracion TIMESTAMP NOT NULL, " +
                "usado BOOLEAN NOT NULL DEFAULT false)";

            using (ConexionBD obj = new ConexionBD())
                obj.EjecutarSentencia(sentencia, false);
        }

        public static bool GuardarCodigo(string correo, string codigo, string tipo)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@correo", SqlDbType.VarChar, 255, correo);
                obj.AgregarParametro(ParameterDirection.Input, "@codigo", SqlDbType.VarChar, 10, codigo);
                obj.AgregarParametro(ParameterDirection.Input, "@tipo", SqlDbType.VarChar, 30, tipo);
                return obj.EjecutarSentencia("sp_GuardarCodigo_Verificacion", true);
            }
        }

        public static bool VerificarCodigo(string correo, string codigo, string tipo)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@correo", SqlDbType.VarChar, 255, correo);
                obj.AgregarParametro(ParameterDirection.Input, "@codigo", SqlDbType.VarChar, 10, codigo);
                obj.AgregarParametro(ParameterDirection.Input, "@tipo", SqlDbType.VarChar, 30, tipo);
                if (!obj.ConsultarValorUnico("sp_VerificarCodigo", true))
                    return false;
                return Convert.ToInt32(obj.ValorUnico) > 0;
            }
        }
    }
}
