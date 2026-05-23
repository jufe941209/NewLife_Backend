using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class FacturaVentaData
    {
        public static string ultimoError = "";

        public static bool InsertarFacturaVenta(FacturaVenta oFactura)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@numero_factura", SqlDbType.VarChar, 20, oFactura.numero_factura);
                obj.AgregarParametro(ParameterDirection.Input, "@metodo_pago", SqlDbType.VarChar, 50, oFactura.metodo_pago);
                obj.AgregarParametro(ParameterDirection.Input, "@direccion_envio", SqlDbType.VarChar, 200, oFactura.direccion_envio ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@cedula_cli", SqlDbType.VarChar, 20, oFactura.cedula_cli);
                if (obj.EjecutarSentencia("sp_Insertar_FacturaVenta", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool ActualizarFacturaVenta(FacturaVenta oFactura)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@numero_factura", SqlDbType.VarChar, 20, oFactura.numero_factura);
                obj.AgregarParametro(ParameterDirection.Input, "@metodo_pago", SqlDbType.VarChar, 50, oFactura.metodo_pago);
                obj.AgregarParametro(ParameterDirection.Input, "@estado_pago", SqlDbType.VarChar, 30, oFactura.estado_pago);
                obj.AgregarParametro(ParameterDirection.Input, "@direccion_envio", SqlDbType.VarChar, 200, oFactura.direccion_envio ?? "");
                if (obj.EjecutarSentencia("sp_Actualizar_FacturaVenta", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool EliminarFacturaVenta(string numero_factura)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@numero_factura", SqlDbType.VarChar, 20, numero_factura);
                if (obj.EjecutarSentencia("sp_Eliminar_FacturaVenta", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static List<FacturaVenta> ListarFacturasVenta()
        {
            List<FacturaVenta> lista = new List<FacturaVenta>();
            using (ConexionBD obj = new ConexionBD())
            {
                if (obj.Consultar("sp_Listar_FacturasVenta", true))
                {
                    SqlDataReader dr = obj.Reader;
                    while (dr.Read())
                        lista.Add(MapFactura(dr));
                }
            }
            return lista;
        }

        public static FacturaVenta ConsultarFacturaVenta(string numero_factura)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@numero_factura", SqlDbType.VarChar, 20, numero_factura);
                if (obj.Consultar("sp_Consultar_FacturaVenta", true))
                {
                    SqlDataReader dr = obj.Reader;
                    if (dr.Read())
                        return MapFactura(dr);
                }
            }
            return null;
        }

        private static FacturaVenta MapFactura(SqlDataReader dr)
        {
            return new FacturaVenta()
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
}
