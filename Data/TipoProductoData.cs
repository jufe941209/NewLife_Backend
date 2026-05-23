using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class TipoProductoData
    {
        public static string ultimoError = "";

        public static List<TipoProducto> ListarTiposProducto()
        {
            List<TipoProducto> lista = new List<TipoProducto>();
            string sentencia = "SELECT id_tipo_producto, nombre, descripcion FROM TIPO_PRODUCTO ORDER BY nombre";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    while (dr.Read())
                    {
                        lista.Add(new TipoProducto()
                        {
                            id_tipo_producto = Convert.ToInt32(dr["id_tipo_producto"]),
                            nombre = dr["nombre"].ToString(),
                            descripcion = dr["descripcion"] == DBNull.Value ? "" : dr["descripcion"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public static TipoProducto ConsultarTipoProducto(int id_tipo_producto)
        {
            TipoProducto oTipoProducto = null;
            string sentencia = "SELECT id_tipo_producto, nombre, descripcion FROM TIPO_PRODUCTO WHERE id_tipo_producto = " + id_tipo_producto;

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    if (dr.Read())
                    {
                        oTipoProducto = new TipoProducto()
                        {
                            id_tipo_producto = Convert.ToInt32(dr["id_tipo_producto"]),
                            nombre = dr["nombre"].ToString(),
                            descripcion = dr["descripcion"] == DBNull.Value ? "" : dr["descripcion"].ToString()
                        };
                    }
                }
            }
            return oTipoProducto;
        }

        public static bool InsertarTipoProducto(TipoProducto oTipoProducto)
        {
            string sentencia = "INSERT INTO TIPO_PRODUCTO (nombre, descripcion) VALUES ('" +
                               oTipoProducto.nombre.Replace("'", "''") + "','" +
                               (oTipoProducto.descripcion ?? "").Replace("'", "''") + "')";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static bool ActualizarTipoProducto(TipoProducto oTipoProducto)
        {
            string sentencia = "UPDATE TIPO_PRODUCTO SET nombre = '" +
                               oTipoProducto.nombre.Replace("'", "''") + "', descripcion = '" +
                               (oTipoProducto.descripcion ?? "").Replace("'", "''") +
                               "' WHERE id_tipo_producto = " + oTipoProducto.id_tipo_producto;

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static bool EliminarTipoProducto(int id_tipo_producto)
        {
            string sentencia = "DELETE FROM TIPO_PRODUCTO WHERE id_tipo_producto = " + id_tipo_producto;

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }
    }
}
