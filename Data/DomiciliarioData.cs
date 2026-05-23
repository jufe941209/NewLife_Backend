using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class DomiciliarioData
    {
        public static string ultimoError = "";

        public static bool InsertarDomiciliario(Domiciliario oDomiciliario)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@cedula_domi", SqlDbType.VarChar, 20, oDomiciliario.cedula_domi);
                obj.AgregarParametro(ParameterDirection.Input, "@nombres", SqlDbType.VarChar, 100, oDomiciliario.nombres);
                obj.AgregarParametro(ParameterDirection.Input, "@telefono", SqlDbType.VarChar, 20, oDomiciliario.telefono ?? "");
                if (obj.EjecutarSentencia("sp_Insertar_Domiciliario", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool ActualizarDomiciliario(Domiciliario oDomiciliario)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@cedula_domi", SqlDbType.VarChar, 20, oDomiciliario.cedula_domi);
                obj.AgregarParametro(ParameterDirection.Input, "@nombres", SqlDbType.VarChar, 100, oDomiciliario.nombres);
                obj.AgregarParametro(ParameterDirection.Input, "@telefono", SqlDbType.VarChar, 20, oDomiciliario.telefono ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@disponibilidad", SqlDbType.VarChar, 20, oDomiciliario.disponibilidad ?? "Disponible");
                obj.AgregarParametro(ParameterDirection.Input, "@estado", SqlDbType.VarChar, 20, oDomiciliario.estado ?? "Activo");
                obj.AgregarParametro(ParameterDirection.Input, "@contrasena", SqlDbType.VarChar, 100, oDomiciliario.contrasena ?? "111111");
                if (obj.EjecutarSentencia("sp_Actualizar_Domiciliario", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool EliminarDomiciliario(string cedula_domi)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@cedula_domi", SqlDbType.VarChar, 20, cedula_domi);
                if (obj.EjecutarSentencia("sp_Eliminar_Domiciliario", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static List<Domiciliario> ListarDomiciliarios()
        {
            List<Domiciliario> lista = new List<Domiciliario>();
            using (ConexionBD obj = new ConexionBD())
            {
                if (obj.Consultar("sp_Listar_Domiciliarios", true))
                {
                    SqlDataReader dr = obj.Reader;
                    while (dr.Read())
                        lista.Add(MapDomiciliario(dr));
                }
            }
            return lista;
        }

        public static Domiciliario ConsultarDomiciliario(string cedula_domi)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@cedula_domi", SqlDbType.VarChar, 20, cedula_domi);
                if (obj.Consultar("sp_Consultar_Domiciliario", true))
                {
                    SqlDataReader dr = obj.Reader;
                    if (dr.Read())
                        return MapDomiciliario(dr);
                }
            }
            return null;
        }

        public static string LimpiarDomiciliariosPrueba()
        {
            string[] cedulas = { "50000001", "20000002", "1091011304", "20000001", "1070809001" };
            int eliminados = 0;
            foreach (string cedula in cedulas)
            {
                using (ConexionBD o1 = new ConexionBD())
                    o1.EjecutarSentencia("UPDATE DESPACHO SET cc_domiciliario = NULL WHERE cc_domiciliario = '" + cedula + "'", false);
                using (ConexionBD o2 = new ConexionBD())
                    o2.EjecutarSentencia("DELETE FROM TRANSPORTE WHERE cedula_domi = '" + cedula + "'", false);
                using (ConexionBD o3 = new ConexionBD())
                {
                    if (o3.EjecutarSentencia("DELETE FROM DOMICILIARIO WHERE cedula_domi = '" + cedula + "'", false))
                        eliminados++;
                }
            }
            return "Eliminados: " + eliminados + " de " + cedulas.Length;
        }

        private static Domiciliario MapDomiciliario(SqlDataReader dr)
        {
            return new Domiciliario()
            {
                cedula_domi = dr["cedula_domi"].ToString(),
                nombres = dr["nombres"].ToString(),
                telefono = dr["telefono"] == DBNull.Value ? "" : dr["telefono"].ToString(),
                fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                disponibilidad = dr["disponibilidad"] == DBNull.Value ? "Disponible" : dr["disponibilidad"].ToString(),
                estado = dr["estado"] == DBNull.Value ? "Activo" : dr["estado"].ToString(),
                contrasena = dr["contrasena"] == DBNull.Value ? "111111" : dr["contrasena"].ToString()
            };
        }
    }
}
