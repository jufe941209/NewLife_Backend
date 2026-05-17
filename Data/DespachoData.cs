using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class DespachoData
    {
        // INSERTAR
        public static string ultimoError = "";

        public static bool InsertarDespacho(Despacho oDespacho)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Insertar_Despacho '" + oDespacho.fecha_despacho.ToString("yyyy-MM-dd") + "','" +
                               oDespacho.numero_factura + "'," +
                               (oDespacho.cc_responsable != null ? "'" + oDespacho.cc_responsable + "'" : "NULL") + "," +
                               (oDespacho.cc_domiciliario != null ? "'" + oDespacho.cc_domiciliario + "'" : "NULL");

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
        public static bool ActualizarDespacho(Despacho oDespacho)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Actualizar_Despacho " + oDespacho.numero_despacho + ",'" +
                               oDespacho.fecha_despacho.ToString("yyyy-MM-dd") + "'," +
                               (oDespacho.fecha_aprobacion.HasValue ? "'" + oDespacho.fecha_aprobacion.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'" : "NULL") + ",'" +
                               oDespacho.estado + "'," +
                               (oDespacho.fecha_entrega.HasValue ? "'" + oDespacho.fecha_entrega.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'" : "NULL") + "," +
                               (oDespacho.cc_responsable != null ? "'" + oDespacho.cc_responsable + "'" : "NULL") + "," +
                               (oDespacho.cc_domiciliario != null ? "'" + oDespacho.cc_domiciliario + "'" : "NULL");

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

        // ELIMINAR
        public static bool EliminarDespacho(int numero_despacho)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Eliminar_Despacho " + numero_despacho;

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
        public static List<Despacho> ListarDespachos()
        {
            List<Despacho> lista = new List<Despacho>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Listar_Despachos";

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
                        estado = dr["estado"].ToString(),
                        fecha_entrega = dr["fecha_entrega"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["fecha_entrega"]),
                        numero_factura = dr["numero_factura"].ToString(),
                        cc_responsable = dr["cc_responsable"].ToString(),
                        cc_domiciliario = dr["cc_domiciliario"].ToString()
                    });
                }
            }
            return lista;
        }

        // CONSULTAR POR ID
        public static Despacho ConsultarDespacho(int numero_despacho)
        {
            Despacho oDespacho = null;
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Consultar_Despacho " + numero_despacho;

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
                        estado = dr["estado"].ToString(),
                        fecha_entrega = dr["fecha_entrega"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["fecha_entrega"]),
                        numero_factura = dr["numero_factura"].ToString(),
                        cc_responsable = dr["cc_responsable"].ToString(),
                        cc_domiciliario = dr["cc_domiciliario"].ToString()
                    };
                }
            }
            return oDespacho;
        }
    }
}