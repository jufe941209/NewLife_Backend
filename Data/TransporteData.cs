using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class TransporteData
    {
        public static string ultimoError = "";

        public static bool InsertarTransporte(Transporte oTransporte)
        {
            string estado = string.IsNullOrEmpty(oTransporte.estado) ? "Activo" : oTransporte.estado;
            string sentencia = "INSERT INTO TRANSPORTE (cedula_domi, placa, tipo, descripcion, estado) VALUES ('" +
                               oTransporte.cedula_domi + "','" +
                               oTransporte.placa + "','" +
                               (oTransporte.tipo ?? "") + "','" +
                               (oTransporte.descripcion ?? "") + "','" +
                               estado + "')";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                { ultimoError = ""; return true; }
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static bool ActualizarTransporte(Transporte oTransporte)
        {
            string sentencia = "UPDATE TRANSPORTE SET " +
                               "cedula_domi = '" + oTransporte.cedula_domi + "', " +
                               "tipo = '" + (oTransporte.tipo ?? "") + "', " +
                               "descripcion = '" + (oTransporte.descripcion ?? "") + "', " +
                               "estado = '" + (oTransporte.estado ?? "Activo") + "' " +
                               "WHERE placa = '" + oTransporte.placa + "'";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                { ultimoError = ""; return true; }
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static bool EliminarTransporte(string placa)
        {
            string sentencia = "DELETE FROM TRANSPORTE WHERE placa = '" + placa + "'";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                { ultimoError = ""; return true; }
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static List<Transporte> ListarTransportes()
        {
            List<Transporte> lista = new List<Transporte>();
            string sentencia = "SELECT cedula_domi, placa, tipo, descripcion, estado FROM TRANSPORTE ORDER BY placa";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    while (dr.Read())
                    {
                        lista.Add(new Transporte()
                        {
                            cedula_domi = dr["cedula_domi"].ToString(),
                            placa = dr["placa"].ToString(),
                            tipo = dr["tipo"].ToString(),
                            descripcion = dr["descripcion"].ToString(),
                            estado = dr["estado"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public static Transporte ConsultarTransporte(string placa)
        {
            Transporte oTransporte = null;
            string sentencia = "SELECT cedula_domi, placa, tipo, descripcion, estado FROM TRANSPORTE WHERE placa = '" + placa + "'";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    if (dr.Read())
                    {
                        oTransporte = new Transporte()
                        {
                            cedula_domi = dr["cedula_domi"].ToString(),
                            placa = dr["placa"].ToString(),
                            tipo = dr["tipo"].ToString(),
                            descripcion = dr["descripcion"].ToString(),
                            estado = dr["estado"].ToString()
                        };
                    }
                }
            }
            return oTransporte;
        }
    }
}
