using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class AdministradorData
    {
        public static string ultimoError = "";

        public static bool InsertarAdministrador(Administrador oAdministrador)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@cedula_adm", SqlDbType.VarChar, 20, oAdministrador.cedula_adm);
                obj.AgregarParametro(ParameterDirection.Input, "@correo", SqlDbType.VarChar, 100, oAdministrador.correo);
                obj.AgregarParametro(ParameterDirection.Input, "@contrasena", SqlDbType.VarChar, 255, oAdministrador.contrasena);
                obj.AgregarParametro(ParameterDirection.Input, "@nombres", SqlDbType.VarChar, 100, oAdministrador.nombres);
                if (obj.EjecutarSentencia("sp_Insertar_Administrador", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool ActualizarAdministrador(Administrador oAdministrador)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@cedula_adm", SqlDbType.VarChar, 20, oAdministrador.cedula_adm);
                obj.AgregarParametro(ParameterDirection.Input, "@correo", SqlDbType.VarChar, 100, oAdministrador.correo);
                obj.AgregarParametro(ParameterDirection.Input, "@nombres", SqlDbType.VarChar, 100, oAdministrador.nombres);
                obj.AgregarParametro(ParameterDirection.Input, "@estado", SqlDbType.VarChar, 20, oAdministrador.estado ?? "Activo");
                if (obj.EjecutarSentencia("sp_Actualizar_Administrador", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool EliminarAdministrador(string cedula_adm)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@cedula_adm", SqlDbType.VarChar, 20, cedula_adm);
                if (obj.EjecutarSentencia("sp_Eliminar_Administrador", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static List<Administrador> ListarAdministradores()
        {
            List<Administrador> lista = new List<Administrador>();
            using (ConexionBD obj = new ConexionBD())
            {
                if (obj.Consultar("sp_Listar_Administradores", true))
                {
                    SqlDataReader dr = obj.Reader;
                    while (dr.Read())
                        lista.Add(MapAdministrador(dr));
                }
            }
            return lista;
        }

        public static Administrador ConsultarAdministrador(string cedula_adm)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@cedula_adm", SqlDbType.VarChar, 20, cedula_adm);
                if (obj.Consultar("sp_Consultar_Administrador", true))
                {
                    SqlDataReader dr = obj.Reader;
                    if (dr.Read())
                        return MapAdministrador(dr);
                }
            }
            return null;
        }

        private static Administrador MapAdministrador(SqlDataReader dr)
        {
            return new Administrador()
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
