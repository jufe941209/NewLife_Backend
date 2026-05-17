using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class ResponsableData
    {
        public static string ultimoError = "";

        // INSERTAR - quitar fecha_registro y estado, el SP los maneja solo
        public static bool InsertarResponsable(Responsable oResponsable)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Insertar_Responsable '" + oResponsable.cedula_resp + "','" +
                               oResponsable.nombres + "','" +
                               oResponsable.telefono + "','" +
                               oResponsable.correo + "'";

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

        // ACTUALIZAR - quitar fecha_registro
        public static bool ActualizarResponsable(Responsable oResponsable)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Actualizar_Responsable '" + oResponsable.cedula_resp + "','" +
                               oResponsable.nombres + "','" +
                               oResponsable.telefono + "','" +
                               oResponsable.correo + "','" +
                               oResponsable.estado + "'";

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
        public static bool EliminarResponsable(string cedula_resp)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Eliminar_Responsable '" + cedula_resp + "'";

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
        public static List<Responsable> ListarResponsables()
        {
            List<Responsable> lista = new List<Responsable>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Listar_Responsables";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                while (dr.Read())
                {
                    lista.Add(new Responsable()
                    {
                        cedula_resp = dr["cedula_resp"].ToString(),
                        nombres = dr["nombres"].ToString(),
                        telefono = dr["telefono"].ToString(),
                        correo = dr["correo"].ToString(),
                        fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                        estado = dr["estado"].ToString()
                    });
                }
            }
            return lista;
        }

        // CONSULTAR
        public static Responsable ConsultarResponsable(string cedula_resp)
        {
            Responsable oResponsable = null;
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Consultar_Responsable '" + cedula_resp + "'";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                if (dr.Read())
                {
                    oResponsable = new Responsable()
                    {
                        cedula_resp = dr["cedula_resp"].ToString(),
                        nombres = dr["nombres"].ToString(),
                        telefono = dr["telefono"].ToString(),
                        correo = dr["correo"].ToString(),
                        fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                        estado = dr["estado"].ToString()
                    };
                }
            }
            return oResponsable;
        }
    }
}