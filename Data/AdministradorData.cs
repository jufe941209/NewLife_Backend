using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;

namespace NewLife.Data
{
    public class AdministradorData
    {
        // INSERTAR
        public static bool InsertarAdministrador(Administrador oAdministrador)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Insertar_Administrador '" + oAdministrador.cedula_adm + "','" +
                               oAdministrador.correo + "','" +
                               oAdministrador.contrasena + "','" +
                               oAdministrador.nombres + "'";

            if (objEst.EjecutarSentencia(sentencia, false))
            {
                objEst = null;
                return true;
            }
            else
            {
                string error = objEst.Error; // captura el error
                objEst = null;
                return false; // pon un breakpoint aquí
            }
        }

        // ACTUALIZAR
        public static bool ActualizarAdministrador(Administrador oAdministrador)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Actualizar_Administrador '" + oAdministrador.cedula_adm + "','" +
                               oAdministrador.correo + "','" +
                               oAdministrador.nombres + "','" +
                               oAdministrador.estado + "'";

            if (objEst.EjecutarSentencia(sentencia, false))
            {
                objEst = null;
                return true;
            }
            else
            {
                objEst = null;
                return false;
            }
        }
        

        // ELIMINAR
        public static bool EliminarAdministrador(string cedula_adm)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Eliminar_Administrador '" + cedula_adm + "'";

            if (objEst.EjecutarSentencia(sentencia, false))
            {
                objEst = null;
                return true;
            }
            else
            {
                objEst = null;
                return false;
            }
        }

        // LISTAR
        public static List<Administrador> ListarAdministradores()
        {
            List<Administrador> lista = new List<Administrador>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Listar_Administradores";

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

        // CONSULTAR POR CÉDULA
        public static Administrador ConsultarAdministrador(string cedula_adm)
        {
            Administrador oAdministrador = null;
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Consultar_Administrador '" + cedula_adm + "'";

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