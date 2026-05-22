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
            ConexionBD objEst = new ConexionBD();
            string estado = string.IsNullOrEmpty(oTransporte.estado) ? "Activo" : oTransporte.estado;
            string sentencia = "INSERT INTO TRANSPORTE (cedula_domi, placa, tipo, descripcion, estado) VALUES ('" +
                               oTransporte.cedula_domi + "','" +
                               oTransporte.placa + "','" +
                               (oTransporte.tipo ?? "") + "','" +
                               (oTransporte.descripcion ?? "") + "','" +
                               estado + "')";
            if (objEst.EjecutarSentencia(sentencia, false))
            { ultimoError = ""; objEst = null; return true; }
            else
            { ultimoError = objEst.Error; objEst = null; return false; }
        }

        public static bool ActualizarTransporte(Transporte oTransporte)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "UPDATE TRANSPORTE SET " +
                               "cedula_domi = '" + oTransporte.cedula_domi + "', " +
                               "tipo = '" + (oTransporte.tipo ?? "") + "', " +
                               "descripcion = '" + (oTransporte.descripcion ?? "") + "', " +
                               "estado = '" + (oTransporte.estado ?? "Activo") + "' " +
                               "WHERE placa = '" + oTransporte.placa + "'";
            if (objEst.EjecutarSentencia(sentencia, false))
            { ultimoError = ""; objEst = null; return true; }
            else
            { ultimoError = objEst.Error; objEst = null; return false; }
        }

        public static bool EliminarTransporte(string placa)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "DELETE FROM TRANSPORTE WHERE placa = '" + placa + "'";
            if (objEst.EjecutarSentencia(sentencia, false))
            { ultimoError = ""; objEst = null; return true; }
            else
            { ultimoError = objEst.Error; objEst = null; return false; }
        }

        public static List<Transporte> ListarTransportes()
        {
            List<Transporte> lista = new List<Transporte>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "SELECT cedula_domi, placa, tipo, descripcion, estado FROM TRANSPORTE ORDER BY placa";
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
            return lista;
        }

        public static Transporte ConsultarTransporte(string placa)
        {
            Transporte oTransporte = null;
            ConexionBD objEst = new ConexionBD();
            string sentencia = "SELECT cedula_domi, placa, tipo, descripcion, estado FROM TRANSPORTE WHERE placa = '" + placa + "'";
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
            return oTransporte;
        }
    }
}
