using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;

namespace NewLife.Data
{
    public class ProductoData
    {
        public static string ultimoError = "";

        public static bool InsertarProducto(Producto oProducto)
        {
            string sentencia = "INSERT INTO PRODUCTO (codigo_prod, nombres, descripcion, stock_min, stock_real, img_url, precio, descuento, capacidad, temperatura_uso, numero_categoria, id_tipo_producto, fecha_registro, estado) VALUES ('" +
                               oProducto.codigo_prod + "','" +
                               oProducto.nombres + "','" +
                               (oProducto.descripcion ?? "") + "'," +
                               oProducto.stock_min + "," +
                               oProducto.stock_real + ",'" +
                               (oProducto.img_url ?? "") + "'," +
                               oProducto.precio.ToString(CultureInfo.InvariantCulture) + "," +
                               oProducto.descuento.ToString(CultureInfo.InvariantCulture) + "," +
                               (!string.IsNullOrEmpty(oProducto.capacidad) ? "'" + oProducto.capacidad + "'" : "NULL") + "," +
                               (!string.IsNullOrEmpty(oProducto.temperatura_uso) ? "'" + oProducto.temperatura_uso + "'" : "NULL") + "," +
                               oProducto.numero_categoria + "," +
                               oProducto.id_tipo_producto + ",GETDATE(),'Activo')";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static bool ActualizarProducto(Producto oProducto)
        {
            string sentencia = "UPDATE PRODUCTO SET " +
                               "nombres = '" + oProducto.nombres + "', " +
                               "descripcion = '" + (oProducto.descripcion ?? "") + "', " +
                               "stock_min = " + oProducto.stock_min + ", " +
                               "stock_real = " + oProducto.stock_real + ", " +
                               "img_url = '" + (oProducto.img_url ?? "") + "', " +
                               "precio = " + oProducto.precio.ToString(CultureInfo.InvariantCulture) + ", " +
                               "descuento = " + oProducto.descuento.ToString(CultureInfo.InvariantCulture) + ", " +
                               "estado = '" + (oProducto.estado ?? "Activo") + "', " +
                               "capacidad = " + (!string.IsNullOrEmpty(oProducto.capacidad) ? "'" + oProducto.capacidad.Replace("'", "''") + "'" : "NULL") + ", " +
                               "temperatura_uso = " + (!string.IsNullOrEmpty(oProducto.temperatura_uso) ? "'" + oProducto.temperatura_uso.Replace("'", "''") + "'" : "NULL") + ", " +
                               "numero_categoria = " + oProducto.numero_categoria + ", " +
                               "id_tipo_producto = " + oProducto.id_tipo_producto + " " +
                               "WHERE codigo_prod = '" + oProducto.codigo_prod.Replace("'", "''") + "'";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static bool EliminarProducto(string codigo_prod)
        {
            string sentencia = "DELETE FROM PRODUCTO WHERE codigo_prod = '" + codigo_prod + "'";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static List<Producto> ListarProductos()
        {
            List<Producto> lista = new List<Producto>();
            string sentencia = "SELECT codigo_prod, nombres, descripcion, stock_min, stock_real, img_url, " +
                               "fecha_registro, precio, ISNULL(descuento,0) AS descuento, estado, capacidad, " +
                               "temperatura_uso, numero_categoria, id_tipo_producto FROM PRODUCTO ORDER BY nombres";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    while (dr.Read())
                    {
                        lista.Add(new Producto()
                        {
                            codigo_prod = dr["codigo_prod"].ToString(),
                            nombres = dr["nombres"].ToString(),
                            descripcion = dr["descripcion"] == DBNull.Value ? "" : dr["descripcion"].ToString(),
                            stock_min = Convert.ToInt32(dr["stock_min"]),
                            stock_real = Convert.ToInt32(dr["stock_real"]),
                            img_url = dr["img_url"] == DBNull.Value ? "" : dr["img_url"].ToString(),
                            fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                            fecha_ultima_modificacion = null,
                            precio = Convert.ToDecimal(dr["precio"]),
                            descuento = Convert.ToDecimal(dr["descuento"]),
                            estado = dr["estado"].ToString(),
                            capacidad = dr["capacidad"] == DBNull.Value ? null : dr["capacidad"].ToString(),
                            temperatura_uso = dr["temperatura_uso"] == DBNull.Value ? null : dr["temperatura_uso"].ToString(),
                            numero_categoria = Convert.ToInt32(dr["numero_categoria"]),
                            id_tipo_producto = Convert.ToInt32(dr["id_tipo_producto"])
                        });
                    }
                }
            }
            return lista;
        }

        public static Producto ConsultarProducto(string codigo_prod)
        {
            Producto oProducto = null;
            string sentencia = "SELECT codigo_prod, nombres, descripcion, stock_min, stock_real, img_url, " +
                               "fecha_registro, precio, ISNULL(descuento,0) AS descuento, estado, capacidad, " +
                               "temperatura_uso, numero_categoria, id_tipo_producto FROM PRODUCTO " +
                               "WHERE codigo_prod = '" + codigo_prod.Replace("'", "''") + "'";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    if (dr.Read())
                    {
                        oProducto = new Producto()
                        {
                            codigo_prod = dr["codigo_prod"].ToString(),
                            nombres = dr["nombres"].ToString(),
                            descripcion = dr["descripcion"] == DBNull.Value ? "" : dr["descripcion"].ToString(),
                            stock_min = Convert.ToInt32(dr["stock_min"]),
                            stock_real = Convert.ToInt32(dr["stock_real"]),
                            img_url = dr["img_url"] == DBNull.Value ? "" : dr["img_url"].ToString(),
                            fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                            fecha_ultima_modificacion = null,
                            precio = Convert.ToDecimal(dr["precio"]),
                            descuento = Convert.ToDecimal(dr["descuento"]),
                            estado = dr["estado"].ToString(),
                            capacidad = dr["capacidad"] == DBNull.Value ? null : dr["capacidad"].ToString(),
                            temperatura_uso = dr["temperatura_uso"] == DBNull.Value ? null : dr["temperatura_uso"].ToString(),
                            numero_categoria = Convert.ToInt32(dr["numero_categoria"]),
                            id_tipo_producto = Convert.ToInt32(dr["id_tipo_producto"])
                        };
                    }
                }
            }
            return oProducto;
        }

        public static bool ReducirStock(string codigo_prod, int cantidad)
        {
            string sentencia = "UPDATE PRODUCTO SET stock_real = CASE WHEN stock_real >= " + cantidad +
                               " THEN stock_real - " + cantidad +
                               " ELSE 0 END WHERE codigo_prod = '" + codigo_prod.Replace("'", "''") + "'";
            using (ConexionBD obj = new ConexionBD())
            {
                if (obj.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static void MigrarDescuento()
        {
            string sentencia =
                "IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'descuento' AND Object_ID = OBJECT_ID(N'PRODUCTO')) " +
                "BEGIN ALTER TABLE PRODUCTO ADD descuento DECIMAL(5,2) NOT NULL DEFAULT 0 END";
            using (ConexionBD obj = new ConexionBD())
            {
                obj.EjecutarSentencia(sentencia, false);
            }
        }

        public static string MigrarStockReal()
        {
            string sentencia =
                "IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'stock_real' AND Object_ID = OBJECT_ID(N'PRODUCTO')) " +
                "BEGIN ALTER TABLE PRODUCTO ADD stock_real INT NOT NULL DEFAULT 0 END";
            using (ConexionBD obj = new ConexionBD())
            {
                if (obj.EjecutarSentencia(sentencia, false))
                    return "Columna stock_real añadida o ya existente.";
                return "Error: " + obj.Error;
            }
        }
    }
}
