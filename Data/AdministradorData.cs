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
            ConexionBD objEst = new ConexionBD();
            string sentencia = "INSERT INTO ADMINISTRADOR (cedula_adm, correo, contrasena, nombres, fecha_registro, estado) VALUES ('" +
                               oAdministrador.cedula_adm.Replace("'", "''") + "','" +
                               oAdministrador.correo.Replace("'", "''") + "','" +
                               oAdministrador.contrasena.Replace("'", "''") + "','" +
                               oAdministrador.nombres.Replace("'", "''") + "',GETDATE(),'Activo')";

            if (objEst.EjecutarSentencia(sentencia, false))
            { objEst = null; return true; }
            else
            { ultimoError = objEst.Error; objEst = null; return false; }
        }

        public static bool ActualizarAdministrador(Administrador oAdministrador)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "UPDATE ADMINISTRADOR SET " +
                               "correo = '" + oAdministrador.correo.Replace("'", "''") + "', " +
                               "nombres = '" + oAdministrador.nombres.Replace("'", "''") + "', " +
                               "estado = '" + (oAdministrador.estado ?? "Activo") + "' " +
                               "WHERE cedula_adm = '" + oAdministrador.cedula_adm.Replace("'", "''") + "'";

            if (objEst.EjecutarSentencia(sentencia, false))
            { objEst = null; return true; }
            else
            { ultimoError = objEst.Error; objEst = null; return false; }
        }

        public static bool EliminarAdministrador(string cedula_adm)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "DELETE FROM ADMINISTRADOR WHERE cedula_adm = '" + cedula_adm.Replace("'", "''") + "'";

            if (objEst.EjecutarSentencia(sentencia, false))
            { objEst = null; return true; }
            else
            { ultimoError = objEst.Error; objEst = null; return false; }
        }

        public static List<Administrador> ListarAdministradores()
        {
            List<Administrador> lista = new List<Administrador>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "SELECT cedula_adm, correo, contrasena, nombres, fecha_registro, estado FROM ADMINISTRADOR ORDER BY nombres";

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
            return lista;
        }

        public static Administrador ConsultarAdministrador(string cedula_adm)
        {
            Administrador oAdministrador = null;
            ConexionBD objEst = new ConexionBD();
            string sentencia = "SELECT cedula_adm, correo, contrasena, nombres, fecha_registro, estado FROM ADMINISTRADOR WHERE cedula_adm = '" +
                               cedula_adm.Replace("'", "''") + "'";

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
            return oAdministrador;
        }
    }
}
