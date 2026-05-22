using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class FacturaVentaData
    {
        public static string ultimoError = "";

        // INSERTAR
        public static bool InsertarFacturaVenta(FacturaVenta oFactura)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "INSERT INTO FACTURA_VENTA (numero_factura, fecha, metodo_pago, estado_pago, direccion_envio, cedula_cli) VALUES ('" +
                               oFactura.numero_factura.Replace("'", "''") + "', GETDATE(), '" +
                               oFactura.metodo_pago.Replace("'", "''") + "', 'Pendiente', '" +
                               (oFactura.direccion_envio ?? "").Replace("'", "''") + "', '" +
                               oFactura.cedula_cli.Replace("'", "''") + "')";

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
        public static bool ActualizarFacturaVenta(FacturaVenta oFactura)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "UPDATE FACTURA_VENTA SET " +
                               "metodo_pago = '" + oFactura.metodo_pago.Replace("'", "''") + "', " +
                               "estado_pago = '" + oFactura.estado_pago.Replace("'", "''") + "', " +
                               "direccion_envio = '" + (oFactura.direccion_envio ?? "").Replace("'", "''") + "' " +
                               "WHERE numero_factura = '" + oFactura.numero_factura.Replace("'", "''") + "'";

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
        public static bool EliminarFacturaVenta(string numero_factura)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "DELETE FROM DETALLE_FACTURA WHERE numero_factura = '" + numero_factura.Replace("'", "''") + "'; " +
                               "DELETE FROM FACTURA_VENTA WHERE numero_factura = '" + numero_factura.Replace("'", "''") + "'";

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
        public static List<FacturaVenta> ListarFacturasVenta()
        {
            List<FacturaVenta> lista = new List<FacturaVenta>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "SELECT numero_factura, fecha, metodo_pago, estado_pago, direccion_envio, cedula_cli FROM FACTURA_VENTA ORDER BY fecha DESC";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                while (dr.Read())
                {
                    lista.Add(new FacturaVenta()
                    {
                        numero_factura = dr["numero_factura"].ToString(),
                        fecha = Convert.ToDateTime(dr["fecha"]),
                        metodo_pago = dr["metodo_pago"].ToString(),
                        estado_pago = dr["estado_pago"].ToString(),
                        direccion_envio = dr["direccion_envio"].ToString(),
                        cedula_cli = dr["cedula_cli"].ToString()
                    });
                }
            }
            return lista;
        }

        // CONSULTAR
        public static FacturaVenta ConsultarFacturaVenta(string numero_factura)
        {
            FacturaVenta oFactura = null;
            ConexionBD objEst = new ConexionBD();
            string sentencia = "SELECT numero_factura, fecha, metodo_pago, estado_pago, direccion_envio, cedula_cli FROM FACTURA_VENTA WHERE numero_factura = '" + numero_factura.Replace("'", "''") + "'";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                if (dr.Read())
                {
                    oFactura = new FacturaVenta()
                    {
                        numero_factura = dr["numero_factura"].ToString(),
                        fecha = Convert.ToDateTime(dr["fecha"]),
                        metodo_pago = dr["metodo_pago"].ToString(),
                        estado_pago = dr["estado_pago"].ToString(),
                        direccion_envio = dr["direccion_envio"].ToString(),
                        cedula_cli = dr["cedula_cli"].ToString()
                    };
                }
            }
            return oFactura;
        }
    }
}
