using ApiEjemplo.Data;
using System;

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
            {
                obj.EjecutarSentencia(sentencia, false);
            }
        }

        public static bool GuardarCodigo(string correo, string codigo, string tipo)
        {
            string borrar = "UPDATE CODIGOS_VERIFICACION SET usado = 1 " +
                            "WHERE correo = '" + correo.Replace("'", "''") + "' " +
                            "AND tipo = '" + tipo.Replace("'", "''") + "' AND usado = 0";

            using (ConexionBD obj = new ConexionBD())
            {
                obj.EjecutarSentencia(borrar, false);
            }

            string insertar = "INSERT INTO CODIGOS_VERIFICACION (correo, codigo, tipo, fecha_creacion, fecha_expiracion, usado) " +
                              "VALUES ('" + correo.Replace("'", "''") + "', '" +
                              codigo.Replace("'", "''") + "', '" +
                              tipo.Replace("'", "''") + "', GETDATE(), DATEADD(MINUTE, 15, GETDATE()), 0)";

            using (ConexionBD obj2 = new ConexionBD())
            {
                return obj2.EjecutarSentencia(insertar, false);
            }
        }

        public static bool VerificarCodigo(string correo, string codigo, string tipo)
        {
            string sentencia = "SELECT COUNT(*) FROM CODIGOS_VERIFICACION " +
                               "WHERE correo = '" + correo.Replace("'", "''") + "' " +
                               "AND codigo = '" + codigo.Replace("'", "''") + "' " +
                               "AND tipo = '" + tipo.Replace("'", "''") + "' " +
                               "AND usado = 0 AND fecha_expiracion > GETDATE()";

            bool valido;
            using (ConexionBD obj = new ConexionBD())
            {
                if (!obj.ConsultarValorUnico(sentencia, false))
                    return false;
                valido = Convert.ToInt32(obj.ValorUnico) > 0;
            }

            if (valido)
            {
                string marcar = "UPDATE CODIGOS_VERIFICACION SET usado = 1 " +
                                "WHERE correo = '" + correo.Replace("'", "''") + "' " +
                                "AND codigo = '" + codigo.Replace("'", "''") + "' " +
                                "AND tipo = '" + tipo.Replace("'", "''") + "' AND usado = 0";
                using (ConexionBD obj2 = new ConexionBD())
                {
                    obj2.EjecutarSentencia(marcar, false);
                }
            }
            return valido;
        }
    }
}
