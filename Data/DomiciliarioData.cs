using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class DomiciliarioData
    {
        public static string ultimoError = "";

        public static bool InsertarDomiciliario(Domiciliario oDomiciliario)
        {
            string sentencia = "INSERT INTO DOMICILIARIO (cedula_domi, nombres, telefono, fecha_registro, disponibilidad, estado) VALUES ('" +
                               oDomiciliario.cedula_domi.Replace("'", "''") + "','" +
                               oDomiciliario.nombres.Replace("'", "''") + "','" +
                               (oDomiciliario.telefono ?? "").Replace("'", "''") + "',GETDATE(),'Disponible','Activo')";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static bool ActualizarDomiciliario(Domiciliario oDomiciliario)
        {
            string sentencia = "UPDATE DOMICILIARIO SET " +
                               "nombres = '" + oDomiciliario.nombres.Replace("'", "''") + "', " +
                               "telefono = '" + (oDomiciliario.telefono ?? "").Replace("'", "''") + "', " +
                               "disponibilidad = '" + (oDomiciliario.disponibilidad ?? "Disponible") + "', " +
                               "estado = '" + (oDomiciliario.estado ?? "Activo") + "' " +
                               "WHERE cedula_domi = '" + oDomiciliario.cedula_domi.Replace("'", "''") + "'";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static bool EliminarDomiciliario(string cedula_domi)
        {
            using (ConexionBD obj1 = new ConexionBD())
            {
                obj1.EjecutarSentencia("UPDATE DESPACHO SET cc_domiciliario = NULL WHERE cc_domiciliario = '" + cedula_domi.Replace("'", "''") + "'", false);
            }
            using (ConexionBD obj2 = new ConexionBD())
            {
                obj2.EjecutarSentencia("DELETE FROM TRANSPORTE WHERE cedula_domi = '" + cedula_domi.Replace("'", "''") + "'", false);
            }
            using (ConexionBD obj3 = new ConexionBD())
            {
                string sentencia = "DELETE FROM DOMICILIARIO WHERE cedula_domi = '" + cedula_domi.Replace("'", "''") + "'";
                if (obj3.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = obj3.Error;
                return false;
            }
        }

        public static string LimpiarDomiciliariosPrueba()
        {
            string[] cedulas = { "50000001", "20000002", "1091011304", "20000001", "1070809001" };
            int eliminados = 0;
            foreach (string cedula in cedulas)
            {
                using (ConexionBD o1 = new ConexionBD())
                {
                    o1.EjecutarSentencia("UPDATE DESPACHO SET cc_domiciliario = NULL WHERE cc_domiciliario = '" + cedula + "'", false);
                }
                using (ConexionBD o2 = new ConexionBD())
                {
                    o2.EjecutarSentencia("DELETE FROM TRANSPORTE WHERE cedula_domi = '" + cedula + "'", false);
                }
                using (ConexionBD o3 = new ConexionBD())
                {
                    if (o3.EjecutarSentencia("DELETE FROM DOMICILIARIO WHERE cedula_domi = '" + cedula + "'", false))
                        eliminados++;
                }
            }
            return "Eliminados: " + eliminados + " de " + cedulas.Length;
        }

        public static List<Domiciliario> ListarDomiciliarios()
        {
            List<Domiciliario> lista = new List<Domiciliario>();
            string sentencia = "SELECT cedula_domi, nombres, telefono, fecha_registro, disponibilidad, estado FROM DOMICILIARIO ORDER BY nombres";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    while (dr.Read())
                    {
                        lista.Add(new Domiciliario()
                        {
                            cedula_domi = dr["cedula_domi"].ToString(),
                            nombres = dr["nombres"].ToString(),
                            telefono = dr["telefono"] == DBNull.Value ? "" : dr["telefono"].ToString(),
                            fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                            disponibilidad = dr["disponibilidad"] == DBNull.Value ? "Disponible" : dr["disponibilidad"].ToString(),
                            estado = dr["estado"] == DBNull.Value ? "Activo" : dr["estado"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public static Domiciliario ConsultarDomiciliario(string cedula_domi)
        {
            Domiciliario oDomiciliario = null;
            string sentencia = "SELECT cedula_domi, nombres, telefono, fecha_registro, disponibilidad, estado FROM DOMICILIARIO WHERE cedula_domi = '" +
                               cedula_domi.Replace("'", "''") + "'";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    if (dr.Read())
                    {
                        oDomiciliario = new Domiciliario()
                        {
                            cedula_domi = dr["cedula_domi"].ToString(),
                            nombres = dr["nombres"].ToString(),
                            telefono = dr["telefono"] == DBNull.Value ? "" : dr["telefono"].ToString(),
                            fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                            disponibilidad = dr["disponibilidad"] == DBNull.Value ? "Disponible" : dr["disponibilidad"].ToString(),
                            estado = dr["estado"] == DBNull.Value ? "Activo" : dr["estado"].ToString()
                        };
                    }
                }
            }
            return oDomiciliario;
        }
    }
}
