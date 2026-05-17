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

        // INSERTAR
        public static bool InsertarTransporte(Transporte oTransporte)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Insertar_Transporte '" + oTransporte.placa + "','" +
                               oTransporte.tipo + "','" +
                               oTransporte.descripcion + "'," +
                               oTransporte.cedula_domi;

            if (objEst.EjecutarSentencia(sentencia, false))
            {
                ultimoError = "";
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
        public static bool ActualizarTransporte(Transporte oTransporte)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Actualizar_Transporte '" + oTransporte.placa + "','" +
                               oTransporte.tipo + "','" +
                               oTransporte.descripcion + "','" +
                               oTransporte.estado + "'," +
                               oTransporte.cedula_domi;

            if (objEst.EjecutarSentencia(sentencia, false))
            {
                ultimoError = "";
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
        public static bool EliminarTransporte(string placa)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Eliminar_Transporte '" + placa + "'";

            if (objEst.EjecutarSentencia(sentencia, false))
            {
                ultimoError = "";
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
        public static List<Transporte> ListarTransportes()
        {
            List<Transporte> lista = new List<Transporte>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Listar_Transportes";

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

        // CONSULTAR
        public static Transporte ConsultarTransporte(string placa)
        {
            Transporte oTransporte = null;
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Consultar_Transporte '" + placa + "'";

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