using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class ProductoData
    {
        public static string ultimoError = "";

        public static bool InsertarProducto(Producto oProducto)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@codigo_prod", SqlDbType.VarChar, 20, oProducto.codigo_prod);
                obj.AgregarParametro(ParameterDirection.Input, "@nombres", SqlDbType.VarChar, 100, oProducto.nombres);
                obj.AgregarParametro(ParameterDirection.Input, "@descripcion", SqlDbType.VarChar, 500, oProducto.descripcion ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@stock_min", SqlDbType.Int, 0, oProducto.stock_min);
                obj.AgregarParametro(ParameterDirection.Input, "@stock_real", SqlDbType.Int, 0, oProducto.stock_real);
                obj.AgregarParametro(ParameterDirection.Input, "@img_url", SqlDbType.VarChar, 500, oProducto.img_url ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@precio", SqlDbType.Decimal, 0, oProducto.precio);
                obj.AgregarParametro(ParameterDirection.Input, "@descuento", SqlDbType.Decimal, 0, oProducto.descuento);
                obj.AgregarParametro(ParameterDirection.Input, "@capacidad", SqlDbType.VarChar, 50, string.IsNullOrEmpty(oProducto.capacidad) ? (object)DBNull.Value : oProducto.capacidad);
                obj.AgregarParametro(ParameterDirection.Input, "@temperatura_uso", SqlDbType.VarChar, 50, string.IsNullOrEmpty(oProducto.temperatura_uso) ? (object)DBNull.Value : oProducto.temperatura_uso);
                obj.AgregarParametro(ParameterDirection.Input, "@numero_categoria", SqlDbType.Int, 0, oProducto.numero_categoria);
                obj.AgregarParametro(ParameterDirection.Input, "@id_tipo_producto", SqlDbType.Int, 0, oProducto.id_tipo_producto);
                if (obj.EjecutarSentencia("sp_Insertar_Producto", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool ActualizarProducto(Producto oProducto)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@codigo_prod", SqlDbType.VarChar, 20, oProducto.codigo_prod);
                obj.AgregarParametro(ParameterDirection.Input, "@nombres", SqlDbType.VarChar, 100, oProducto.nombres);
                obj.AgregarParametro(ParameterDirection.Input, "@descripcion", SqlDbType.VarChar, 500, oProducto.descripcion ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@stock_min", SqlDbType.Int, 0, oProducto.stock_min);
                obj.AgregarParametro(ParameterDirection.Input, "@stock_real", SqlDbType.Int, 0, oProducto.stock_real);
                obj.AgregarParametro(ParameterDirection.Input, "@img_url", SqlDbType.VarChar, 500, oProducto.img_url ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@precio", SqlDbType.Decimal, 0, oProducto.precio);
                obj.AgregarParametro(ParameterDirection.Input, "@descuento", SqlDbType.Decimal, 0, oProducto.descuento);
                obj.AgregarParametro(ParameterDirection.Input, "@estado", SqlDbType.VarChar, 20, oProducto.estado ?? "Activo");
                obj.AgregarParametro(ParameterDirection.Input, "@capacidad", SqlDbType.VarChar, 50, string.IsNullOrEmpty(oProducto.capacidad) ? (object)DBNull.Value : oProducto.capacidad);
                obj.AgregarParametro(ParameterDirection.Input, "@temperatura_uso", SqlDbType.VarChar, 50, string.IsNullOrEmpty(oProducto.temperatura_uso) ? (object)DBNull.Value : oProducto.temperatura_uso);
                obj.AgregarParametro(ParameterDirection.Input, "@numero_categoria", SqlDbType.Int, 0, oProducto.numero_categoria);
                obj.AgregarParametro(ParameterDirection.Input, "@id_tipo_producto", SqlDbType.Int, 0, oProducto.id_tipo_producto);
                if (obj.EjecutarSentencia("sp_Actualizar_Producto", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool EliminarProducto(string codigo_prod)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@codigo_prod", SqlDbType.VarChar, 20, codigo_prod);
                if (obj.EjecutarSentencia("sp_Eliminar_Producto", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static List<Producto> ListarProductos()
        {
            List<Producto> lista = new List<Producto>();
            using (ConexionBD obj = new ConexionBD())
            {
                if (obj.Consultar("sp_Listar_Productos", true))
                {
                    SqlDataReader dr = obj.Reader;
                    while (dr.Read())
                        lista.Add(MapProducto(dr));
                }
            }
            return lista;
        }

        public static Producto ConsultarProducto(string codigo_prod)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@codigo_prod", SqlDbType.VarChar, 20, codigo_prod);
                if (obj.Consultar("sp_Consultar_Producto", true))
                {
                    SqlDataReader dr = obj.Reader;
                    if (dr.Read())
                        return MapProducto(dr);
                }
            }
            return null;
        }

        public static bool ReducirStock(string codigo_prod, int cantidad)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@codigo_prod", SqlDbType.VarChar, 20, codigo_prod);
                obj.AgregarParametro(ParameterDirection.Input, "@cantidad", SqlDbType.Int, 0, cantidad);
                if (obj.EjecutarSentencia("sp_ReducirStock_Producto", true))
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

        private static Producto MapProducto(SqlDataReader dr)
        {
            return new Producto()
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
