using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace NewLife.Data
{
    public class TransporteData
    {
        public static string ultimoError = "";

        public static bool InsertarTransporte(Transporte oTransporte)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@cedula_domi", SqlDbType.VarChar, 20, oTransporte.cedula_domi);
                obj.AgregarParametro(ParameterDirection.Input, "@placa", SqlDbType.VarChar, 20, oTransporte.placa);
                obj.AgregarParametro(ParameterDirection.Input, "@tipo", SqlDbType.VarChar, 50, oTransporte.tipo ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@descripcion", SqlDbType.VarChar, 200, oTransporte.descripcion ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@estado", SqlDbType.VarChar, 20, string.IsNullOrEmpty(oTransporte.estado) ? "Activo" : oTransporte.estado);
                if (obj.EjecutarSentencia("sp_Insertar_Transporte", true))
                { ultimoError = ""; return true; }
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool ActualizarTransporte(Transporte oTransporte)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@placa", SqlDbType.VarChar, 20, oTransporte.placa);
                obj.AgregarParametro(ParameterDirection.Input, "@cedula_domi", SqlDbType.VarChar, 20, oTransporte.cedula_domi);
                obj.AgregarParametro(ParameterDirection.Input, "@tipo", SqlDbType.VarChar, 50, oTransporte.tipo ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@descripcion", SqlDbType.VarChar, 200, oTransporte.descripcion ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@estado", SqlDbType.VarChar, 20, oTransporte.estado ?? "Activo");
                if (obj.EjecutarSentencia("sp_Actualizar_Transporte", true))
                { ultimoError = ""; return true; }
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool EliminarTransporte(string placa)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@placa", SqlDbType.VarChar, 20, placa);
                if (obj.EjecutarSentencia("sp_Eliminar_Transporte", true))
                { ultimoError = ""; return true; }
                ultimoError = obj.Error;
                return false;
            }
        }

        public static List<Transporte> ListarTransportes()
        {
            List<Transporte> lista = new List<Transporte>();
            using (ConexionBD obj = new ConexionBD())
            {
                if (obj.Consultar("sp_Listar_Transportes", true))
                {
                    NpgsqlDataReader dr = obj.Reader;
                    while (dr.Read())
                        lista.Add(MapTransporte(dr));
                }
            }
            return lista;
        }

        public static Transporte ConsultarTransporte(string placa)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@placa", SqlDbType.VarChar, 20, placa);
                if (obj.Consultar("sp_Consultar_Transporte", true))
                {
                    NpgsqlDataReader dr = obj.Reader;
                    if (dr.Read())
                        return MapTransporte(dr);
                }
            }
            return null;
        }

        private static Transporte MapTransporte(NpgsqlDataReader dr)
        {
            return new Transporte()
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
