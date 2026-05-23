using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class DespachoData
    {
        public static string ultimoError = "";

        public static bool InsertarDespacho(Despacho oDespacho)
        {
            string ccResp = string.IsNullOrEmpty(oDespacho.cc_responsable) ? "NULL" : "'" + oDespacho.cc_responsable.Replace("'", "''") + "'";
            string ccDomi = string.IsNullOrEmpty(oDespacho.cc_domiciliario) ? "NULL" : "'" + oDespacho.cc_domiciliario.Replace("'", "''") + "'";
            string sentencia = "INSERT INTO DESPACHO (fecha_despacho, estado, numero_factura, cc_responsable, cc_domiciliario) VALUES ('" +
                               oDespacho.fecha_despacho.ToString("yyyy-MM-dd") + "','Pendiente','" +
                               oDespacho.numero_factura.Replace("'", "''") + "'," +
                               ccResp + "," + ccDomi + ")";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static bool ActualizarDespacho(Despacho oDespacho)
        {
            string ccResp = string.IsNullOrEmpty(oDespacho.cc_responsable) ? "NULL" : "'" + oDespacho.cc_responsable.Replace("'", "''") + "'";
            string ccDomi = string.IsNullOrEmpty(oDespacho.cc_domiciliario) ? "NULL" : "'" + oDespacho.cc_domiciliario.Replace("'", "''") + "'";
            string sentencia = "UPDATE DESPACHO SET " +
                               "fecha_despacho = '" + oDespacho.fecha_despacho.ToString("yyyy-MM-dd") + "', " +
                               "fecha_aprobacion = " + (oDespacho.fecha_aprobacion.HasValue ? "'" + oDespacho.fecha_aprobacion.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'" : "NULL") + ", " +
                               "estado = '" + oDespacho.estado + "', " +
                               "fecha_entrega = " + (oDespacho.fecha_entrega.HasValue ? "'" + oDespacho.fecha_entrega.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'" : "NULL") + ", " +
                               "cc_responsable = " + ccResp + ", " +
                               "cc_domiciliario = " + ccDomi + " " +
                               "WHERE numero_despacho = " + oDespacho.numero_despacho;

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static bool EliminarDespacho(int numero_despacho)
        {
            string sentencia = "DELETE FROM DESPACHO WHERE numero_despacho = " + numero_despacho;

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static List<Despacho> ListarDespachos()
        {
            List<Despacho> lista = new List<Despacho>();
            string sentencia = "SELECT numero_despacho, fecha_despacho, fecha_aprobacion, estado, fecha_entrega, numero_factura, cc_responsable, cc_domiciliario FROM DESPACHO ORDER BY numero_despacho DESC";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    while (dr.Read())
                    {
                        lista.Add(new Despacho()
                        {
                            numero_despacho = Convert.ToInt32(dr["numero_despacho"]),
                            fecha_despacho = Convert.ToDateTime(dr["fecha_despacho"]),
                            fecha_aprobacion = dr["fecha_aprobacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["fecha_aprobacion"]),
                            estado = dr["estado"] == DBNull.Value ? "Pendiente" : dr["estado"].ToString(),
                            fecha_entrega = dr["fecha_entrega"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["fecha_entrega"]),
                            numero_factura = dr["numero_factura"].ToString(),
                            cc_responsable = dr["cc_responsable"] == DBNull.Value ? "" : dr["cc_responsable"].ToString(),
                            cc_domiciliario = dr["cc_domiciliario"] == DBNull.Value ? "" : dr["cc_domiciliario"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public static Despacho ConsultarDespacho(int numero_despacho)
        {
            Despacho oDespacho = null;
            string sentencia = "SELECT numero_despacho, fecha_despacho, fecha_aprobacion, estado, fecha_entrega, numero_factura, cc_responsable, cc_domiciliario FROM DESPACHO WHERE numero_despacho = " + numero_despacho;

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    if (dr.Read())
                    {
                        oDespacho = new Despacho()
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
            return oDespacho;
        }
    }
}
