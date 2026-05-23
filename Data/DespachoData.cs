using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class DespachoData
    {
        public static string ultimoError = "";

        public static bool InsertarDespacho(Despacho oDespacho)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@fecha_despacho", SqlDbType.Date, 0, oDespacho.fecha_despacho);
                obj.AgregarParametro(ParameterDirection.Input, "@numero_factura", SqlDbType.VarChar, 20, oDespacho.numero_factura);
                obj.AgregarParametro(ParameterDirection.Input, "@cc_responsable", SqlDbType.VarChar, 20, oDespacho.cc_responsable ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@cc_domiciliario", SqlDbType.VarChar, 20, oDespacho.cc_domiciliario ?? "");
                if (obj.EjecutarSentencia("sp_Insertar_Despacho", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool ActualizarDespacho(Despacho oDespacho)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@numero_despacho", SqlDbType.Int, 0, oDespacho.numero_despacho);
                obj.AgregarParametro(ParameterDirection.Input, "@fecha_despacho", SqlDbType.Date, 0, oDespacho.fecha_despacho);
                obj.AgregarParametro(ParameterDirection.Input, "@fecha_aprobacion", SqlDbType.DateTime, 0,
                    oDespacho.fecha_aprobacion.HasValue ? (object)oDespacho.fecha_aprobacion.Value : DBNull.Value);
                obj.AgregarParametro(ParameterDirection.Input, "@estado", SqlDbType.VarChar, 30, oDespacho.estado ?? "Pendiente");
                obj.AgregarParametro(ParameterDirection.Input, "@fecha_entrega", SqlDbType.DateTime, 0,
                    oDespacho.fecha_entrega.HasValue ? (object)oDespacho.fecha_entrega.Value : DBNull.Value);
                obj.AgregarParametro(ParameterDirection.Input, "@cc_responsable", SqlDbType.VarChar, 20, oDespacho.cc_responsable ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@cc_domiciliario", SqlDbType.VarChar, 20, oDespacho.cc_domiciliario ?? "");
                if (obj.EjecutarSentencia("sp_Actualizar_Despacho", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool EliminarDespacho(int numero_despacho)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@numero_despacho", SqlDbType.Int, 0, numero_despacho);
                if (obj.EjecutarSentencia("sp_Eliminar_Despacho", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static List<Despacho> ListarDespachos()
        {
            List<Despacho> lista = new List<Despacho>();
            using (ConexionBD obj = new ConexionBD())
            {
                if (obj.Consultar("sp_Listar_Despachos", true))
                {
                    SqlDataReader dr = obj.Reader;
                    while (dr.Read())
                        lista.Add(MapDespacho(dr));
                }
            }
            return lista;
        }

        public static Despacho ConsultarDespacho(int numero_despacho)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@numero_despacho", SqlDbType.Int, 0, numero_despacho);
                if (obj.Consultar("sp_Consultar_Despacho", true))
                {
                    SqlDataReader dr = obj.Reader;
                    if (dr.Read())
                        return MapDespacho(dr);
                }
            }
            return null;
        }

        public static List<Despacho> ListarDespachosDisponibles()
        {
            List<Despacho> lista = new List<Despacho>();
            using (ConexionBD obj = new ConexionBD())
            {
                if (obj.Consultar("sp_Listar_Despachos_Disponibles", true))
                {
                    SqlDataReader dr = obj.Reader;
                    while (dr.Read())
                        lista.Add(MapDespacho(dr));
                }
            }
            return lista;
        }

        public static bool ExisteDespachoParaFactura(string numero_factura)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@numero_factura", SqlDbType.VarChar, 20, numero_factura);
                if (!obj.ConsultarValorUnico("sp_ExisteDespacho_PorFactura", true))
                    return false;
                return Convert.ToInt32(obj.ValorUnico) > 0;
            }
        }

        private static Despacho MapDespacho(SqlDataReader dr)
        {
            return new Despacho()
            {
                numero_despacho = Convert.ToInt32(dr["numero_despacho"]),
                fecha_despacho = Convert.ToDateTime(dr["fecha_despacho"]),
                fecha_aprobacion = dr["fecha_aprobacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["fecha_aprobacion"]),
                estado = dr["estado"] == DBNull.Value ? "Pendiente" : dr["estado"].ToString(),
                fecha_entrega = dr["fecha_entrega"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["fecha_entrega"]),
                numero_factura = dr["numero_factura"].ToString(),
                cc_responsable = dr["cc_responsable"] == DBNull.Value ? "" : dr["cc_responsable"].ToString(),
                cc_domiciliario = dr["cc_domiciliario"] == DBNull.Value ? "" : dr["cc_domiciliario"].ToString()
            };
        }
    }
}
