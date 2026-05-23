using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class DetalleFacturaData
    {
        public static string ultimoError = "";

        public static string MigrarPrecioUnitario()
        {
            string sql = "IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'precio_unitario' AND Object_ID = OBJECT_ID(N'DETALLE_FACTURA')) " +
                         "BEGIN ALTER TABLE DETALLE_FACTURA ADD precio_unitario DECIMAL(10,2) NOT NULL DEFAULT 0 END";
            using (ConexionBD obj = new ConexionBD())
            {
                if (obj.EjecutarSentencia(sql, false))
                    return "Migración OK: precio_unitario verificado en DETALLE_FACTURA";
                return "Error: " + obj.Error;
            }
        }

        public static bool InsertarDetalleFactura(DetalleFactura oDetalle)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@cantidad", SqlDbType.Int, 0, oDetalle.cantidad);
                obj.AgregarParametro(ParameterDirection.Input, "@descuento_porcentaje", SqlDbType.Decimal, 0,
                    oDetalle.descuento_porcentaje.HasValue ? (object)oDetalle.descuento_porcentaje.Value : DBNull.Value);
                obj.AgregarParametro(ParameterDirection.Input, "@numero_factura", SqlDbType.VarChar, 20, oDetalle.numero_factura);
                obj.AgregarParametro(ParameterDirection.Input, "@codigo_prod", SqlDbType.VarChar, 20, oDetalle.codigo_prod);
                obj.AgregarParametro(ParameterDirection.Input, "@precio_unitario", SqlDbType.Decimal, 0, oDetalle.precio_unitario);
                if (obj.EjecutarSentencia("sp_Insertar_DetalleFactura", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool ActualizarDetalleFactura(DetalleFactura oDetalle)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@num_f_codigo", SqlDbType.Int, 0, oDetalle.num_f_codigo);
                obj.AgregarParametro(ParameterDirection.Input, "@cantidad", SqlDbType.Int, 0, oDetalle.cantidad);
                obj.AgregarParametro(ParameterDirection.Input, "@descuento_porcentaje", SqlDbType.Decimal, 0,
                    oDetalle.descuento_porcentaje.HasValue ? (object)oDetalle.descuento_porcentaje.Value : DBNull.Value);
                obj.AgregarParametro(ParameterDirection.Input, "@precio_unitario", SqlDbType.Decimal, 0, oDetalle.precio_unitario);
                if (obj.EjecutarSentencia("sp_Actualizar_DetalleFactura", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool EliminarDetalleFactura(int num_f_codigo)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@num_f_codigo", SqlDbType.Int, 0, num_f_codigo);
                if (obj.EjecutarSentencia("sp_Eliminar_DetalleFactura", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static List<DetalleFactura> ListarDetalleFactura()
        {
            List<DetalleFactura> lista = new List<DetalleFactura>();
            using (ConexionBD obj = new ConexionBD())
            {
                if (obj.Consultar("sp_Listar_DetalleFactura", true))
                {
                    SqlDataReader dr = obj.Reader;
                    while (dr.Read()) lista.Add(MapDetalle(dr));
                }
            }
            return lista;
        }

        public static List<DetalleFactura> ListarDetalleFacturaPorFactura(string numero_factura)
        {
            List<DetalleFactura> lista = new List<DetalleFactura>();
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@numero_factura", SqlDbType.VarChar, 20, numero_factura);
                if (obj.Consultar("sp_Listar_DetalleFactura_PorFactura", true))
                {
                    SqlDataReader dr = obj.Reader;
                    while (dr.Read()) lista.Add(MapDetalle(dr));
                }
            }
            return lista;
        }

        public static DetalleFactura ConsultarDetalleFactura(int num_f_codigo)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@num_f_codigo", SqlDbType.Int, 0, num_f_codigo);
                if (obj.Consultar("sp_Consultar_DetalleFactura", true))
                {
                    SqlDataReader dr = obj.Reader;
                    if (dr.Read()) return MapDetalle(dr);
                }
            }
            return null;
        }

        private static DetalleFactura MapDetalle(SqlDataReader dr)
        {
            return new DetalleFactura()
            {
                num_f_codigo = Convert.ToInt32(dr["num_f_codigo"]),
                cantidad = Convert.ToInt32(dr["cantidad"]),
                descuento_porcentaje = dr["descuento_porcentaje"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["descuento_porcentaje"]),
                numero_factura = dr["numero_factura"].ToString(),
                codigo_prod = dr["codigo_prod"].ToString(),
                precio_unitario = dr["precio_unitario"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["precio_unitario"])
            };
        }
    }
}
