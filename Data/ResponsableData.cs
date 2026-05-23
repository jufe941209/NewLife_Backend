using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class ResponsableData
    {
        public static string ultimoError = "";

        public static bool CambiarContrasena(string correo, string nuevaContrasena)
        {
            string sentencia = "UPDATE RESPONSABLE SET contrasena = '" + nuevaContrasena.Replace("'", "''") +
                               "' WHERE correo = '" + correo.Replace("'", "''") + "'";
            using (ConexionBD obj = new ConexionBD())
            {
                return obj.EjecutarSentencia(sentencia, false);
            }
        }

        public static bool ExisteCorreo(string correo)
        {
            string sentencia = "SELECT COUNT(*) FROM RESPONSABLE WHERE correo = '" + correo.Replace("'", "''") + "'";
            using (ConexionBD obj = new ConexionBD())
            {
                return obj.ConsultarValorUnico(sentencia, false) && Convert.ToInt32(obj.ValorUnico) > 0;
            }
        }

        public static void MigrarContrasena()
        {
            string sentencia =
                "IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'contrasena' AND Object_ID = OBJECT_ID(N'RESPONSABLE')) " +
                "BEGIN ALTER TABLE RESPONSABLE ADD contrasena VARCHAR(255) NULL END; " +
                "UPDATE RESPONSABLE SET contrasena = '11111' WHERE contrasena IS NULL OR contrasena = ''";
            using (ConexionBD obj = new ConexionBD())
            {
                obj.EjecutarSentencia(sentencia, false);
            }
        }

        public static bool InsertarResponsable(Responsable oResponsable)
        {
            string contrasena = string.IsNullOrEmpty(oResponsable.contrasena) ? "111111" : oResponsable.contrasena;

            using (ConexionBD objEst = new ConexionBD())
            {
                string sentencia = "INSERT INTO RESPONSABLE (cedula_resp, nombres, telefono, correo, contrasena, fecha_registro, estado) VALUES ('" +
                                   oResponsable.cedula_resp + "','" +
                                   oResponsable.nombres + "','" +
                                   (oResponsable.telefono ?? "") + "','" +
                                   (oResponsable.correo ?? "") + "','" +
                                   contrasena + "',GETDATE(),'Activo')";
                if (objEst.EjecutarSentencia(sentencia, false))
                { ultimoError = ""; return true; }
            }

            // Fallback: columna contrasena no existe aún
            using (ConexionBD objEst = new ConexionBD())
            {
                string sentencia = "INSERT INTO RESPONSABLE (cedula_resp, nombres, telefono, correo, fecha_registro, estado) VALUES ('" +
                                   oResponsable.cedula_resp + "','" +
                                   oResponsable.nombres + "','" +
                                   (oResponsable.telefono ?? "") + "','" +
                                   (oResponsable.correo ?? "") + "',GETDATE(),'Activo')";
                if (objEst.EjecutarSentencia(sentencia, false))
                { ultimoError = ""; return true; }
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static bool ActualizarResponsable(Responsable oResponsable)
        {
            bool tienePassword = !string.IsNullOrEmpty(oResponsable.contrasena);
            string baseSet = "nombres = '" + oResponsable.nombres + "', " +
                             "telefono = '" + (oResponsable.telefono ?? "") + "', " +
                             "correo = '" + (oResponsable.correo ?? "") + "', " +
                             "estado = '" + (oResponsable.estado ?? "Activo") + "'";
            string whereClause = " WHERE cedula_resp = '" + oResponsable.cedula_resp + "'";

            if (tienePassword)
            {
                using (ConexionBD objEst = new ConexionBD())
                {
                    string conPass = "UPDATE RESPONSABLE SET " + baseSet +
                                     ", contrasena = '" + oResponsable.contrasena + "'" + whereClause;
                    if (objEst.EjecutarSentencia(conPass, false))
                    { ultimoError = ""; return true; }
                }
            }

            using (ConexionBD objEst2 = new ConexionBD())
            {
                string sinPass = "UPDATE RESPONSABLE SET " + baseSet + whereClause;
                if (objEst2.EjecutarSentencia(sinPass, false))
                { ultimoError = ""; return true; }
                ultimoError = objEst2.Error;
                return false;
            }
        }

        public static bool EliminarResponsable(string cedula_resp)
        {
            string sentencia = "DELETE FROM RESPONSABLE WHERE cedula_resp = '" + cedula_resp + "'";
            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static List<Responsable> ListarResponsables()
        {
            List<Responsable> lista = new List<Responsable>();

            using (ConexionBD objEst = new ConexionBD())
            {
                string sentencia = "SELECT cedula_resp, nombres, telefono, correo, ISNULL(contrasena,'111111') AS contrasena, fecha_registro, estado FROM RESPONSABLE ORDER BY nombres";
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    while (dr.Read())
                    {
                        lista.Add(new Responsable()
                        {
                            cedula_resp = dr["cedula_resp"].ToString(),
                            nombres = dr["nombres"].ToString(),
                            telefono = dr["telefono"].ToString(),
                            correo = dr["correo"].ToString(),
                            contrasena = dr["contrasena"].ToString(),
                            fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                            estado = dr["estado"].ToString()
                        });
                    }
                    return lista;
                }
            }

            // Fallback: columna contrasena aún no existe en la BD
            using (ConexionBD objEst = new ConexionBD())
            {
                string sentencia = "SELECT cedula_resp, nombres, telefono, correo, fecha_registro, estado FROM RESPONSABLE ORDER BY nombres";
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    while (dr.Read())
                    {
                        lista.Add(new Responsable()
                        {
                            cedula_resp = dr["cedula_resp"].ToString(),
                            nombres = dr["nombres"].ToString(),
                            telefono = dr["telefono"].ToString(),
                            correo = dr["correo"].ToString(),
                            contrasena = "111111",
                            fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                            estado = dr["estado"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public static Responsable ConsultarResponsable(string cedula_resp)
        {
            Responsable oResponsable = null;

            using (ConexionBD objEst = new ConexionBD())
            {
                string sentencia = "SELECT cedula_resp, nombres, telefono, correo, ISNULL(contrasena,'111111') AS contrasena, fecha_registro, estado FROM RESPONSABLE WHERE cedula_resp = '" + cedula_resp + "'";
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    if (dr.Read())
                    {
                        return new Responsable()
                        {
                            cedula_resp = dr["cedula_resp"].ToString(),
                            nombres = dr["nombres"].ToString(),
                            telefono = dr["telefono"].ToString(),
                            correo = dr["correo"].ToString(),
                            contrasena = dr["contrasena"].ToString(),
                            fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                            estado = dr["estado"].ToString()
                        };
                    }
                }
            }

            // Fallback sin contrasena
            using (ConexionBD objEst = new ConexionBD())
            {
                string sentencia = "SELECT cedula_resp, nombres, telefono, correo, fecha_registro, estado FROM RESPONSABLE WHERE cedula_resp = '" + cedula_resp + "'";
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    if (dr.Read())
                    {
                        oResponsable = new Responsable()
                        {
                            cedula_resp = dr["cedula_resp"].ToString(),
                            nombres = dr["nombres"].ToString(),
                            telefono = dr["telefono"].ToString(),
                            correo = dr["correo"].ToString(),
                            contrasena = "111111",
                            fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                            estado = dr["estado"].ToString()
                        };
                    }
                }
            }
            return oResponsable;
        }
    }
}
