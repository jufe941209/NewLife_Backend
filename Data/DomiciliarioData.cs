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
            ConexionBD objEst = new ConexionBD();
            string sentencia = "INSERT INTO DOMICILIARIO (cedula_domi, nombres, telefono, fecha_registro, disponibilidad, estado) VALUES ('" +
                               oDomiciliario.cedula_domi.Replace("'", "''") + "','" +
                               oDomiciliario.nombres.Replace("'", "''") + "','" +
                               (oDomiciliario.telefono ?? "").Replace("'", "''") + "',GETDATE(),'Disponible','Activo')";

            if (objEst.EjecutarSentencia(sentencia, false))
            { objEst = null; return true; }
            else
            { ultimoError = objEst.Error; objEst = null; return false; }
        }

        public static bool ActualizarDomiciliario(Domiciliario oDomiciliario)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "UPDATE DOMICILIARIO SET " +
                               "nombres = '" + oDomiciliario.nombres.Replace("'", "''") + "', " +
                               "telefono = '" + (oDomiciliario.telefono ?? "").Replace("'", "''") + "', " +
                               "disponibilidad = '" + (oDomiciliario.disponibilidad ?? "Disponible") + "', " +
                               "estado = '" + (oDomiciliario.estado ?? "Activo") + "' " +
                               "WHERE cedula_domi = '" + oDomiciliario.cedula_domi.Replace("'", "''") + "'";

            if (objEst.EjecutarSentencia(sentencia, false))
            { objEst = null; return true; }
            else
            { ultimoError = objEst.Error; objEst = null; return false; }
        }

        public static bool EliminarDomiciliario(string cedula_domi)
        {
            ConexionBD obj1 = new ConexionBD();
            obj1.EjecutarSentencia("UPDATE DESPACHO SET cc_domiciliario = NULL WHERE cc_domiciliario = '" + cedula_domi.Replace("'", "''") + "'", false);

            ConexionBD obj2 = new ConexionBD();
            obj2.EjecutarSentencia("DELETE FROM TRANSPORTE WHERE cedula_domi = '" + cedula_domi.Replace("'", "''") + "'", false);

            ConexionBD obj3 = new ConexionBD();
            string sentencia = "DELETE FROM DOMICILIARIO WHERE cedula_domi = '" + cedula_domi.Replace("'", "''") + "'";
            if (obj3.EjecutarSentencia(sentencia, false))
            { return true; }
            else
            { ultimoError = obj3.Error; return false; }
        }

        public static string LimpiarDomiciliariosPrueba()
        {
            string[] cedulas = { "50000001", "20000002", "1091011304", "20000001", "1070809001" };
            int eliminados = 0;
            foreach (string cedula in cedulas)
            {
                ConexionBD o1 = new ConexionBD();
                o1.EjecutarSentencia("UPDATE DESPACHO SET cc_domiciliario = NULL WHERE cc_domiciliario = '" + cedula + "'", false);
                ConexionBD o2 = new ConexionBD();
                o2.EjecutarSentencia("DELETE FROM TRANSPORTE WHERE cedula_domi = '" + cedula + "'", false);
                ConexionBD o3 = new ConexionBD();
                if (o3.EjecutarSentencia("DELETE FROM DOMICILIARIO WHERE cedula_domi = '" + cedula + "'", false))
                    eliminados++;
            }
            return "Eliminados: " + eliminados + " de " + cedulas.Length;
        }

        public static List<Domiciliario> ListarDomiciliarios()
        {
            List<Domiciliario> lista = new List<Domiciliario>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "SELECT cedula_domi, nombres, telefono, fecha_registro, disponibilidad, estado FROM DOMICILIARIO ORDER BY nombres";

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
            return lista;
        }

        public static Domiciliario ConsultarDomiciliario(string cedula_domi)
        {
            Domiciliario oDomiciliario = null;
            ConexionBD objEst = new ConexionBD();
            string sentencia = "SELECT cedula_domi, nombres, telefono, fecha_registro, disponibilidad, estado FROM DOMICILIARIO WHERE cedula_domi = '" +
                               cedula_domi.Replace("'", "''") + "'";

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
            return oDomiciliario;
        }
    }
}
