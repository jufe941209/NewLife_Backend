using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class AdministradorData
    {
        public static string ultimoError = "";

        public static bool InsertarAdministrador(Administrador oAdministrador)
        {
            string sentencia = "INSERT INTO ADMINISTRADOR (cedula_adm, correo, contrasena, nombres, fecha_registro, estado) VALUES ('" +
                               oAdministrador.cedula_adm.Replace("'", "''") + "','" +
                               oAdministrador.correo.Replace("'", "''") + "','" +
                               oAdministrador.contrasena.Replace("'", "''") + "','" +
                               oAdministrador.nombres.Replace("'", "''") + "',GETDATE(),'Activo')";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static bool ActualizarAdministrador(Administrador oAdministrador)
        {
            string sentencia = "UPDATE ADMINISTRADOR SET " +
                               "correo = '" + oAdministrador.correo.Replace("'", "''") + "', " +
                               "nombres = '" + oAdministrador.nombres.Replace("'", "''") + "', " +
                               "estado = '" + (oAdministrador.estado ?? "Activo") + "' " +
                               "WHERE cedula_adm = '" + oAdministrador.cedula_adm.Replace("'", "''") + "'";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static bool EliminarAdministrador(string cedula_adm)
        {
            string sentencia = "DELETE FROM ADMINISTRADOR WHERE cedula_adm = '" + cedula_adm.Replace("'", "''") + "'";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static List<Administrador> ListarAdministradores()
        {
            List<Administrador> lista = new List<Administrador>();
            string sentencia = "SELECT cedula_adm, correo, contrasena, nombres, fecha_registro, estado FROM ADMINISTRADOR ORDER BY nombres";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    while (dr.Read())
                    {
                        lista.Add(new Administrador()
                        {
                            cedula_adm = dr["cedula_adm"].ToString(),
                            correo = dr["correo"].ToString(),
                            contrasena = dr["contrasena"].ToString(),
                            nombres = dr["nombres"].ToString(),
                            fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                            estado = dr["estado"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public static Administrador ConsultarAdministrador(string cedula_adm)
        {
            Administrador oAdministrador = null;
            string sentencia = "SELECT cedula_adm, correo, contrasena, nombres, fecha_registro, estado FROM ADMINISTRADOR WHERE cedula_adm = '" +
                               cedula_adm.Replace("'", "''") + "'";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    if (dr.Read())
                    {
                        oAdministrador = new Administrador()
                        {
                            cedula_adm = dr["cedula_adm"].ToString(),
                            correo = dr["correo"].ToString(),
                            contrasena = dr["contrasena"].ToString(),
                            nombres = dr["nombres"].ToString(),
                            fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                            estado = dr["estado"].ToString()
                        };
                    }
                }
            }
            return oAdministrador;
        }
    }
}
