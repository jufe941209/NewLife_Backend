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

        public static bool InsertarResponsable(Responsable oResponsable)
        {
            string contrasena = string.IsNullOrEmpty(oResponsable.contrasena) ? "111111" : oResponsable.contrasena;
            // Intentar con columna contrasena
            ConexionBD objEst = new ConexionBD();
            string sentencia = "INSERT INTO RESPONSABLE (cedula_resp, nombres, telefono, correo, contrasena, fecha_registro, estado) VALUES ('" +
                               oResponsable.cedula_resp + "','" +
                               oResponsable.nombres + "','" +
                               (oResponsable.telefono ?? "") + "','" +
                               (oResponsable.correo ?? "") + "','" +
                               contrasena + "',GETDATE(),'Activo')";
            if (objEst.EjecutarSentencia(sentencia, false))
            { ultimoError = ""; objEst = null; return true; }

            // Fallback: columna contrasena no existe aún
            objEst = new ConexionBD();
            sentencia = "INSERT INTO RESPONSABLE (cedula_resp, nombres, telefono, correo, fecha_registro, estado) VALUES ('" +
                        oResponsable.cedula_resp + "','" +
                        oResponsable.nombres + "','" +
                        (oResponsable.telefono ?? "") + "','" +
                        (oResponsable.correo ?? "") + "',GETDATE(),'Activo')";
            if (objEst.EjecutarSentencia(sentencia, false))
            { ultimoError = ""; objEst = null; return true; }

            ultimoError = objEst.Error;
            objEst = null;
            return false;
        }

        public static bool ActualizarResponsable(Responsable oResponsable)
        {
            bool tienePassword = !string.IsNullOrEmpty(oResponsable.contrasena);
            string baseSet = "nombres = '" + oResponsable.nombres + "', " +
                             "telefono = '" + (oResponsable.telefono ?? "") + "', " +
                             "correo = '" + (oResponsable.correo ?? "") + "', " +
                             "estado = '" + (oResponsable.estado ?? "Activo") + "'";
            string whereClause = " WHERE cedula_resp = '" + oResponsable.cedula_resp + "'";

            // Si hay contraseña nueva, intentar actualizarla también
            if (tienePassword)
            {
                ConexionBD objEst = new ConexionBD();
                string conPass = "UPDATE RESPONSABLE SET " + baseSet +
                                 ", contrasena = '" + oResponsable.contrasena + "'" + whereClause;
                if (objEst.EjecutarSentencia(conPass, false))
                { ultimoError = ""; objEst = null; return true; }
                // Si falla (columna no existe), caer al update sin contraseña
            }

            // Update sin contraseña (columna no existe aún o no se cambió)
            ConexionBD objEst2 = new ConexionBD();
            string sinPass = "UPDATE RESPONSABLE SET " + baseSet + whereClause;
            if (objEst2.EjecutarSentencia(sinPass, false))
            { ultimoError = ""; objEst2 = null; return true; }

            ultimoError = objEst2.Error;
            objEst2 = null;
            return false;
        }

        public static bool EliminarResponsable(string cedula_resp)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "DELETE FROM RESPONSABLE WHERE cedula_resp = '" + cedula_resp + "'";
            if (objEst.EjecutarSentencia(sentencia, false))
            { objEst = null; return true; }
            else
            { ultimoError = objEst.Error; objEst = null; return false; }
        }

        public static List<Responsable> ListarResponsables()
        {
            List<Responsable> lista = new List<Responsable>();
            ConexionBD objEst = new ConexionBD();
            // Primero intentamos con la columna contrasena
            string sentencia = "SELECT cedula_resp, nombres, telefono, correo, ISNULL(contrasena,'111111') AS contrasena, fecha_registro, estado FROM RESPONSABLE ORDER BY nombres";
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
                        contrasena = dr["contrasena"].ToString(),
                        fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                        estado = dr["estado"].ToString()
                    });
                }
                return lista;
            }
            // Fallback: columna contrasena aún no existe en la BD
            objEst = new ConexionBD();
            sentencia = "SELECT cedula_resp, nombres, telefono, correo, fecha_registro, estado FROM RESPONSABLE ORDER BY nombres";
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
                        contrasena = "111111",
                        fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                        estado = dr["estado"].ToString()
                    });
                }
            }
            return lista;
        }

        public static Responsable ConsultarResponsable(string cedula_resp)
        {
            Responsable oResponsable = null;
            ConexionBD objEst = new ConexionBD();
            string sentencia = "SELECT cedula_resp, nombres, telefono, correo, ISNULL(contrasena,'111111') AS contrasena, fecha_registro, estado FROM RESPONSABLE WHERE cedula_resp = '" + cedula_resp + "'";
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
                        contrasena = dr["contrasena"].ToString(),
                        fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                        estado = dr["estado"].ToString()
                    };
                    return oResponsable;
                }
            }
            // Fallback sin contrasena
            objEst = new ConexionBD();
            sentencia = "SELECT cedula_resp, nombres, telefono, correo, fecha_registro, estado FROM RESPONSABLE WHERE cedula_resp = '" + cedula_resp + "'";
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
                        contrasena = "111111",
                        fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                        estado = dr["estado"].ToString()
                    };
                }
            }
            return oResponsable;
        }
    }
}
