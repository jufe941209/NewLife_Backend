using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class TipoProductoData
    {
        public static string ultimoError = "";

        public static bool InsertarTipoProducto(TipoProducto oTipoProducto)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@nombre", SqlDbType.VarChar, 100, oTipoProducto.nombre);
                obj.AgregarParametro(ParameterDirection.Input, "@descripcion", SqlDbType.VarChar, 300, oTipoProducto.descripcion ?? "");
                if (obj.EjecutarSentencia("sp_Insertar_TipoProducto", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool ActualizarTipoProducto(TipoProducto oTipoProducto)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@id_tipo_producto", SqlDbType.Int, 0, oTipoProducto.id_tipo_producto);
                obj.AgregarParametro(ParameterDirection.Input, "@nombre", SqlDbType.VarChar, 100, oTipoProducto.nombre);
                obj.AgregarParametro(ParameterDirection.Input, "@descripcion", SqlDbType.VarChar, 300, oTipoProducto.descripcion ?? "");
                if (obj.EjecutarSentencia("sp_Actualizar_TipoProducto", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool EliminarTipoProducto(int id_tipo_producto)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@id_tipo_producto", SqlDbType.Int, 0, id_tipo_producto);
                if (obj.EjecutarSentencia("sp_Eliminar_TipoProducto", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static List<TipoProducto> ListarTiposProducto()
        {
            List<TipoProducto> lista = new List<TipoProducto>();
            using (ConexionBD obj = new ConexionBD())
            {
                if (obj.Consultar("sp_Listar_TiposProducto", true))
                {
                    SqlDataReader dr = obj.Reader;
                    while (dr.Read())
                        lista.Add(MapTipoProducto(dr));
                }
            }
            return lista;
        }

        public static TipoProducto ConsultarTipoProducto(int id_tipo_producto)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@id_tipo_producto", SqlDbType.Int, 0, id_tipo_producto);
                if (obj.Consultar("sp_Consultar_TipoProducto", true))
                {
                    SqlDataReader dr = obj.Reader;
                    if (dr.Read())
                        return MapTipoProducto(dr);
                }
            }
            return null;
        }

        private static TipoProducto MapTipoProducto(SqlDataReader dr)
        {
            return new TipoProducto()
            {
                id_tipo_producto = Convert.ToInt32(dr["id_tipo_producto"]),
                nombre = dr["nombre"].ToString(),
                descripcion = dr["descripcion"] == DBNull.Value ? "" : dr["descripcion"].ToString()
            };
        }
    }
}
