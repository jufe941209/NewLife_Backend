using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class ResponsableData
    {
        public static string ultimoError = "";

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
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@cedula_resp", SqlDbType.VarChar, 20, oResponsable.cedula_resp);
                obj.AgregarParametro(ParameterDirection.Input, "@nombres", SqlDbType.VarChar, 100, oResponsable.nombres);
                obj.AgregarParametro(ParameterDirection.Input, "@telefono", SqlDbType.VarChar, 20, oResponsable.telefono ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@correo", SqlDbType.VarChar, 100, oResponsable.correo ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@contrasena", SqlDbType.VarChar, 255, string.IsNullOrEmpty(oResponsable.contrasena) ? "111111" : oResponsable.contrasena);
                if (obj.EjecutarSentencia("sp_Insertar_Responsable", true))
                { ultimoError = ""; return true; }
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool ActualizarResponsable(Responsable oResponsable)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@cedula_resp", SqlDbType.VarChar, 20, oResponsable.cedula_resp);
                obj.AgregarParametro(ParameterDirection.Input, "@nombres", SqlDbType.VarChar, 100, oResponsable.nombres);
                obj.AgregarParametro(ParameterDirection.Input, "@telefono", SqlDbType.VarChar, 20, oResponsable.telefono ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@correo", SqlDbType.VarChar, 100, oResponsable.correo ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@contrasena", SqlDbType.VarChar, 255, oResponsable.contrasena ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@estado", SqlDbType.VarChar, 20, oResponsable.estado ?? "Activo");
                if (obj.EjecutarSentencia("sp_Actualizar_Responsable", true))
                { ultimoError = ""; return true; }
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool EliminarResponsable(string cedula_resp)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@cedula_resp", SqlDbType.VarChar, 20, cedula_resp);
                if (obj.EjecutarSentencia("sp_Eliminar_Responsable", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static List<Responsable> ListarResponsables()
        {
            List<Responsable> lista = new List<Responsable>();
            using (ConexionBD obj = new ConexionBD())
            {
                if (obj.Consultar("sp_Listar_Responsables", true))
                {
                    SqlDataReader dr = obj.Reader;
                    while (dr.Read())
                        lista.Add(MapResponsable(dr));
                }
            }
            return lista;
        }

        public static Responsable ConsultarResponsable(string cedula_resp)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@cedula_resp", SqlDbType.VarChar, 20, cedula_resp);
                if (obj.Consultar("sp_Consultar_Responsable", true))
                {
                    SqlDataReader dr = obj.Reader;
                    if (dr.Read())
                        return MapResponsable(dr);
                }
            }
            return null;
        }

        public static bool CambiarContrasena(string correo, string nuevaContrasena)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@correo", SqlDbType.VarChar, 100, correo);
                obj.AgregarParametro(ParameterDirection.Input, "@nuevaContrasena", SqlDbType.VarChar, 255, nuevaContrasena);
                return obj.EjecutarSentencia("sp_CambiarContrasena_Responsable", true);
            }
        }

        public static bool ExisteCorreo(string correo)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@correo", SqlDbType.VarChar, 100, correo);
                return obj.ConsultarValorUnico("sp_ExisteCorreo_Responsable", true) && Convert.ToInt32(obj.ValorUnico) > 0;
            }
        }

        private static Responsable MapResponsable(SqlDataReader dr)
        {
            return new Responsable()
            {
                cedula_resp = dr["cedula_resp"].ToString(),
                nombres = dr["nombres"].ToString(),
                telefono = dr["telefono"].ToString(),
                correo = dr["correo"].ToString(),
                contrasena = dr["contrasena"] == DBNull.Value ? "111111" : dr["contrasena"].ToString(),
                fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                estado = dr["estado"].ToString()
            };
        }
    }
}
