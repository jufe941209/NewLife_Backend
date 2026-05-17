using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class DetalleFacturaData
    {
        // INSERTAR

        public static string ultimoError = "";

        public static bool InsertarDetalleFactura(DetalleFactura oDetalle)
        {
            ConexionBD objEst = new ConexionBD();
            string descuento = oDetalle.descuento_porcentaje.HasValue
                ? oDetalle.descuento_porcentaje.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)
                : "0";

            string sentencia = "EXEC sp_Insertar_DetalleFactura " + oDetalle.cantidad + "," +
                               descuento + ",'" +
                               oDetalle.numero_factura + "','" +
                               oDetalle.codigo_prod + "'";

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


        // ACTUALIZAR
        public static bool ActualizarDetalleFactura(DetalleFactura oDetalle)
        {
            ConexionBD objEst = new ConexionBD();
            string descuento = oDetalle.descuento_porcentaje.HasValue
                ? oDetalle.descuento_porcentaje.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)
                : "0";

            string sentencia = "EXEC sp_Actualizar_DetalleFactura " + oDetalle.num_f_codigo + "," +
                               oDetalle.cantidad + "," +
                               descuento;

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
        public static bool EliminarDetalleFactura(int num_f_codigo)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Eliminar_DetalleFactura " + num_f_codigo;

            if (objEst.EjecutarSentencia(sentencia, false))
            {
                objEst = null;
                return true;
            }
            else
            {
                objEst = null;
                return false;
            }
        }

        // LISTAR
        public static List<DetalleFactura> ListarDetalleFactura()
        {
            List<DetalleFactura> lista = new List<DetalleFactura>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Listar_DetalleFactura";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                while (dr.Read())
                {
                    lista.Add(new DetalleFactura()
                    {
                        num_f_codigo = Convert.ToInt32(dr["num_f_codigo"]),
                        cantidad = Convert.ToInt32(dr["cantidad"]),
                        descuento_porcentaje = dr["descuento_porcentaje"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["descuento_porcentaje"]),
                        numero_factura = dr["numero_factura"].ToString(),
                        codigo_prod = dr["codigo_prod"].ToString()
                    });
                }
            }
            return lista;
        }

        // LISTAR POR FACTURA
        public static List<DetalleFactura> ListarDetalleFacturaPorFactura(string numero_factura)
        {
            List<DetalleFactura> lista = new List<DetalleFactura>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Listar_DetalleFactura_PorFactura '" + numero_factura + "'";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                while (dr.Read())
                {
                    lista.Add(new DetalleFactura()
                    {
                        num_f_codigo = Convert.ToInt32(dr["num_f_codigo"]),
                        cantidad = Convert.ToInt32(dr["cantidad"]),
                        descuento_porcentaje = dr["descuento_porcentaje"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["descuento_porcentaje"]),
                        numero_factura = dr["numero_factura"].ToString(),
                        codigo_prod = dr["codigo_prod"].ToString()
                    });
                }
            }
            return lista;
        }

        // CONSULTAR POR ID
        public static DetalleFactura ConsultarDetalleFactura(int num_f_codigo)
        {
            DetalleFactura oDetalle = null;
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Consultar_DetalleFactura " + num_f_codigo;

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                if (dr.Read())
                {
                    oDetalle = new DetalleFactura()
                    {
                        num_f_codigo = Convert.ToInt32(dr["num_f_codigo"]),
                        cantidad = Convert.ToInt32(dr["cantidad"]),
                        descuento_porcentaje = dr["descuento_porcentaje"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["descuento_porcentaje"]),
                        numero_factura = dr["numero_factura"].ToString(),
                        codigo_prod = dr["codigo_prod"].ToString()
                    };
                }
            }
            return oDetalle;
        }
    }
}