using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;

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
            string descuento = oDetalle.descuento_porcentaje.HasValue
                ? oDetalle.descuento_porcentaje.Value.ToString(CultureInfo.InvariantCulture)
                : "0";
            string precioUnit = oDetalle.precio_unitario.ToString(CultureInfo.InvariantCulture);

            string sentencia = "INSERT INTO DETALLE_FACTURA (cantidad, descuento_porcentaje, numero_factura, codigo_prod, precio_unitario) VALUES (" +
                               oDetalle.cantidad + ", " +
                               descuento + ", '" +
                               oDetalle.numero_factura.Replace("'", "''") + "', '" +
                               oDetalle.codigo_prod.Replace("'", "''") + "', " +
                               precioUnit + ")";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static bool ActualizarDetalleFactura(DetalleFactura oDetalle)
        {
            string descuento = oDetalle.descuento_porcentaje.HasValue
                ? oDetalle.descuento_porcentaje.Value.ToString(CultureInfo.InvariantCulture)
                : "0";
            string precioUnit = oDetalle.precio_unitario.ToString(CultureInfo.InvariantCulture);

            string sentencia = "UPDATE DETALLE_FACTURA SET " +
                               "cantidad = " + oDetalle.cantidad + ", " +
                               "descuento_porcentaje = " + descuento + ", " +
                               "precio_unitario = " + precioUnit + " " +
                               "WHERE num_f_codigo = " + oDetalle.num_f_codigo;

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static bool EliminarDetalleFactura(int num_f_codigo)
        {
            string sentencia = "DELETE FROM DETALLE_FACTURA WHERE num_f_codigo = " + num_f_codigo;

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        private static DetalleFactura ReadDetalle(SqlDataReader dr)
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

        public static List<DetalleFactura> ListarDetalleFactura()
        {
            List<DetalleFactura> lista = new List<DetalleFactura>();
            string sentencia = "SELECT num_f_codigo, cantidad, descuento_porcentaje, numero_factura, codigo_prod, precio_unitario FROM DETALLE_FACTURA ORDER BY num_f_codigo DESC";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    while (dr.Read()) lista.Add(ReadDetalle(dr));
                }
            }
            return lista;
        }

        public static List<DetalleFactura> ListarDetalleFacturaPorFactura(string numero_factura)
        {
            List<DetalleFactura> lista = new List<DetalleFactura>();
            string sentencia = "SELECT num_f_codigo, cantidad, descuento_porcentaje, numero_factura, codigo_prod, precio_unitario " +
                               "FROM DETALLE_FACTURA WHERE numero_factura = '" + numero_factura.Replace("'", "''") + "'";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    while (dr.Read()) lista.Add(ReadDetalle(dr));
                }
            }
            return lista;
        }

        public static DetalleFactura ConsultarDetalleFactura(int num_f_codigo)
        {
            DetalleFactura oDetalle = null;
            string sentencia = "SELECT num_f_codigo, cantidad, descuento_porcentaje, numero_factura, codigo_prod, precio_unitario " +
                               "FROM DETALLE_FACTURA WHERE num_f_codigo = " + num_f_codigo;

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    if (dr.Read()) oDetalle = ReadDetalle(dr);
                }
            }
            return oDetalle;
        }
    }
}
