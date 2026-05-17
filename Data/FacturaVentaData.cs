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

        // INSERTAR - quitar fecha y estado_pago, el SP los maneja solo
        public static bool InsertarFacturaVenta(FacturaVenta oFactura)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Insertar_FacturaVenta '" + oFactura.numero_factura + "','" +
                               oFactura.metodo_pago + "','" +
                               oFactura.direccion_envio + "','" +
                               oFactura.cedula_cli + "'";

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

        // ACTUALIZAR - solo los params que pide el SP
        public static bool ActualizarFacturaVenta(FacturaVenta oFactura)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Actualizar_FacturaVenta '" + oFactura.numero_factura + "','" +
                               oFactura.metodo_pago + "','" +
                               oFactura.estado_pago + "','" +
                               oFactura.direccion_envio + "'";

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
            string sentencia = "EXEC sp_Eliminar_FacturaVenta '" + numero_factura + "'";

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
            string sentencia = "EXEC sp_Listar_FacturasVenta";

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
            string sentencia = "EXEC sp_Consultar_FacturaVenta '" + numero_factura + "'";

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