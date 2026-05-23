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
                "IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CODIGOS_VERIFICACION') " +
                "BEGIN CREATE TABLE CODIGOS_VERIFICACION (" +
                "id INT IDENTITY(1,1) PRIMARY KEY, " +
                "correo VARCHAR(255) NOT NULL, " +
                "codigo VARCHAR(10) NOT NULL, " +
                "tipo VARCHAR(30) NOT NULL, " +
                "fecha_creacion DATETIME NOT NULL DEFAULT GETDATE(), " +
                "fecha_expiracion DATETIME NOT NULL, " +
                "usado BIT NOT NULL DEFAULT 0) END";

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
