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

        // INSERTAR
        public static bool InsertarDomiciliario(Domiciliario oDomiciliario)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Insertar_Domiciliario '" + oDomiciliario.cedula_domi + "','" +
                               oDomiciliario.nombres + "','" +
                               oDomiciliario.telefono + "'";

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
        public static bool ActualizarDomiciliario(Domiciliario oDomiciliario)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Actualizar_Domiciliario '" + oDomiciliario.cedula_domi + "','" +
                               oDomiciliario.nombres + "','" +
                               oDomiciliario.telefono + "','" +
                               oDomiciliario.disponibilidad + "','" +
                               oDomiciliario.estado + "'";

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
        public static bool EliminarDomiciliario(string cedula_domi)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Eliminar_Domiciliario '" + cedula_domi + "'";

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
        public static List<Domiciliario> ListarDomiciliarios()
        {
            List<Domiciliario> lista = new List<Domiciliario>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Listar_Domiciliarios";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                while (dr.Read())
                {
                    lista.Add(new Domiciliario()
                    {
                        cedula_domi = dr["cedula_domi"].ToString(),
                        nombres = dr["nombres"].ToString(),
                        telefono = dr["telefono"].ToString(),
                        fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                        disponibilidad = dr["disponibilidad"].ToString(),
                        estado = dr["estado"].ToString()
                    });
                }
            }
            return lista;
        }

        // CONSULTAR POR CÉDULA
        public static Domiciliario ConsultarDomiciliario(string cedula_domi)
        {
            Domiciliario oDomiciliario = null;
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Consultar_Domiciliario '" + cedula_domi + "'";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                if (dr.Read())
                {
                    oDomiciliario = new Domiciliario()
                    {
                        cedula_domi = dr["cedula_domi"].ToString(),
                        nombres = dr["nombres"].ToString(),
                        telefono = dr["telefono"].ToString(),
                        fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                        disponibilidad = dr["disponibilidad"].ToString(),
                        estado = dr["estado"].ToString()
                    };
                }
            }
            return oDomiciliario;
        }
    }
}