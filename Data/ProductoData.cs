using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class ProductoData
    {
        public static string ultimoError = "";

        // INSERTAR - quitar fecha_registro, fecha_ultima_modificacion y estado, el SP los maneja solo
        public static bool InsertarProducto(Producto oProducto)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Insertar_Producto '" + oProducto.codigo_prod + "','" +
                               oProducto.nombres + "','" +
                               oProducto.descripcion + "'," +
                               oProducto.stock_min + ",'" +
                               oProducto.img_url + "'," +
                               oProducto.precio.ToString(System.Globalization.CultureInfo.InvariantCulture) + "," +
                               (oProducto.capacidad != null ? "'" + oProducto.capacidad + "'" : "NULL") + "," +
                               (oProducto.temperatura_uso != null ? "'" + oProducto.temperatura_uso + "'" : "NULL") + "," +
                               oProducto.numero_categoria + "," +
                               oProducto.id_tipo_producto;

            if (objEst.EjecutarSentencia(sentencia, false))
            {
                objEst = null;
                return true;
            }
            else
            {
                ultimoError = objEst.Error;
                objEst = null;
                return false;
            }
        }

        // ACTUALIZAR - quitar fecha_registro y fecha_ultima_modificacion, el SP los maneja solo
        public static bool ActualizarProducto(Producto oProducto)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Actualizar_Producto '" + oProducto.codigo_prod + "','" +
                               oProducto.nombres + "','" +
                               oProducto.descripcion + "'," +
                               oProducto.stock_min + ",'" +
                               oProducto.img_url + "'," +
                               oProducto.precio.ToString(System.Globalization.CultureInfo.InvariantCulture) + ",'" +
                               oProducto.estado + "'," +
                               (oProducto.capacidad != null ? "'" + oProducto.capacidad + "'" : "NULL") + "," +
                               (oProducto.temperatura_uso != null ? "'" + oProducto.temperatura_uso + "'" : "NULL") + "," +
                               oProducto.numero_categoria + "," +
                               oProducto.id_tipo_producto;

            if (objEst.EjecutarSentencia(sentencia, false))
            {
                objEst = null;
                return true;
            }
            else
            {
                ultimoError = objEst.Error;
                objEst = null;
                return false;
            }
        }

        // ELIMINAR
        public static bool EliminarProducto(string codigo_prod)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Eliminar_Producto '" + codigo_prod + "'";

            if (objEst.EjecutarSentencia(sentencia, false))
            {
                objEst = null;
                return true;
            }
            else
            {
                ultimoError = objEst.Error;
                objEst = null;
                return false;
            }
        }

        // LISTAR
        public static List<Producto> ListarProductos()
        {
            List<Producto> lista = new List<Producto>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Listar_Productos";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                while (dr.Read())
                {
                    lista.Add(new Producto()
                    {
                        codigo_prod = dr["codigo_prod"].ToString(),
                        nombres = dr["nombres"].ToString(),
                        descripcion = dr["descripcion"].ToString(),
                        stock_min = Convert.ToInt32(dr["stock_min"]),
                        img_url = dr["img_url"].ToString(),
                        fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                        fecha_ultima_modificacion = dr["fecha_ultima_modificacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["fecha_ultima_modificacion"]),
                        precio = Convert.ToDecimal(dr["precio"]),
                        estado = dr["estado"].ToString(),
                        capacidad = dr["capacidad"].ToString(),
                        temperatura_uso = dr["temperatura_uso"].ToString(),
                        numero_categoria = Convert.ToInt32(dr["numero_categoria"]),
                        id_tipo_producto = Convert.ToInt32(dr["id_tipo_producto"])
                    });
                }
            }
            return lista;
        }

        // CONSULTAR
        public static Producto ConsultarProducto(string codigo_prod)
        {
            Producto oProducto = null;
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Consultar_Producto '" + codigo_prod + "'";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                if (dr.Read())
                {
                    oProducto = new Producto()
                    {
                        codigo_prod = dr["codigo_prod"].ToString(),
                        nombres = dr["nombres"].ToString(),
                        descripcion = dr["descripcion"].ToString(),
                        stock_min = Convert.ToInt32(dr["stock_min"]),
                        img_url = dr["img_url"].ToString(),
                        fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                        fecha_ultima_modificacion = dr["fecha_ultima_modificacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["fecha_ultima_modificacion"]),
                        precio = Convert.ToDecimal(dr["precio"]),
                        estado = dr["estado"].ToString(),
                        capacidad = dr["capacidad"].ToString(),
                        temperatura_uso = dr["temperatura_uso"].ToString(),
                        numero_categoria = Convert.ToInt32(dr["numero_categoria"]),
                        id_tipo_producto = Convert.ToInt32(dr["id_tipo_producto"])
                    };
                }
            }
            return oProducto;
        }
    }
}