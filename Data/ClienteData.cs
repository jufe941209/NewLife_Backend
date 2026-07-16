using ApiEjemplo.Data;
using NewLife.Helpers;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace NewLife.Data
{
    public class ClienteData
    {
        public static string ultimoError = "";

        public static bool InsertarCliente(Cliente oCliente)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@numero_identificacion", SqlDbType.VarChar, 20, oCliente.numero_identificacion);
                obj.AgregarParametro(ParameterDirection.Input, "@nombres", SqlDbType.VarChar, 100, oCliente.nombres);
                obj.AgregarParametro(ParameterDirection.Input, "@tipo_documento", SqlDbType.VarChar, 10, oCliente.tipo_documento ?? "CC");
                obj.AgregarParametro(ParameterDirection.Input, "@direccion", SqlDbType.VarChar, 200, oCliente.direccion ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@telefono", SqlDbType.VarChar, 20, oCliente.telefono ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@correo", SqlDbType.VarChar, 100, oCliente.correo);
                var pwd = oCliente.contrasena ?? "111111";
                obj.AgregarParametro(ParameterDirection.Input, "@contrasena", SqlDbType.VarChar, 255, HashHelper.EsHash(pwd) ? pwd : HashHelper.Sha256(pwd));
                obj.AgregarParametro(ParameterDirection.Input, "@tipo_cliente", SqlDbType.VarChar, 20, oCliente.tipo_cliente ?? "Regular");
                obj.AgregarParametro(ParameterDirection.Input, "@estado", SqlDbType.VarChar, 20, oCliente.estado ?? "Activo");
                obj.AgregarParametro(ParameterDirection.Input, "@cedula_adm", SqlDbType.VarChar, 20, oCliente.cedula_adm ?? "");
                if (obj.EjecutarSentencia("sp_Insertar_Cliente", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool ActualizarCliente(Cliente oCliente)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@numero_identificacion", SqlDbType.VarChar, 20, oCliente.numero_identificacion);
                obj.AgregarParametro(ParameterDirection.Input, "@nombres", SqlDbType.VarChar, 100, oCliente.nombres);
                obj.AgregarParametro(ParameterDirection.Input, "@tipo_documento", SqlDbType.VarChar, 10, oCliente.tipo_documento ?? "CC");
                obj.AgregarParametro(ParameterDirection.Input, "@direccion", SqlDbType.VarChar, 200, oCliente.direccion ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@telefono", SqlDbType.VarChar, 20, oCliente.telefono ?? "");
                obj.AgregarParametro(ParameterDirection.Input, "@correo", SqlDbType.VarChar, 100, oCliente.correo);
                obj.AgregarParametro(ParameterDirection.Input, "@tipo_cliente", SqlDbType.VarChar, 20, oCliente.tipo_cliente ?? "Regular");
                obj.AgregarParametro(ParameterDirection.Input, "@estado", SqlDbType.VarChar, 20, oCliente.estado ?? "Activo");
                obj.AgregarParametro(ParameterDirection.Input, "@cedula_adm", SqlDbType.VarChar, 20, oCliente.cedula_adm ?? "");
                if (obj.EjecutarSentencia("sp_Actualizar_Cliente", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool EliminarCliente(string numero_identificacion)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@numero_identificacion", SqlDbType.VarChar, 20, numero_identificacion);
                if (obj.EjecutarSentencia("sp_Eliminar_Cliente", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static List<Cliente> ListarClientes()
        {
            List<Cliente> lista = new List<Cliente>();
            using (ConexionBD obj = new ConexionBD())
            {
                if (obj.Consultar("sp_Listar_Clientes", true))
                {
                    NpgsqlDataReader dr = obj.Reader;
                    while (dr.Read())
                        lista.Add(MapCliente(dr));
                }
            }
            return lista;
        }

        public static Cliente LoginCliente(string correo, string contrasena)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@correo", SqlDbType.VarChar, 100, correo);
                obj.AgregarParametro(ParameterDirection.Input, "@contrasena", SqlDbType.VarChar, 255, HashHelper.Sha256(contrasena));
                if (obj.Consultar("sp_Login_Cliente", true))
                {
                    NpgsqlDataReader dr = obj.Reader;
                    if (dr.Read())
                        return MapCliente(dr);
                }
            }
            return null;
        }

        public static bool CambiarContrasena(string correo, string nuevaContrasena)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@correo", SqlDbType.VarChar, 100, correo);
                obj.AgregarParametro(ParameterDirection.Input, "@nuevaContrasena", SqlDbType.VarChar, 255, HashHelper.Sha256(nuevaContrasena));
                return obj.EjecutarSentencia("sp_CambiarContrasena_Cliente", true);
            }
        }

        public static bool ExisteCorreo(string correo)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@correo", SqlDbType.VarChar, 100, correo);
                return obj.ConsultarValorUnico("sp_ExisteCorreo_Cliente", true) && Convert.ToInt32(obj.ValorUnico) > 0;
            }
        }

        public static Cliente ConsultarCliente(string numero_identificacion)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@numero_identificacion", SqlDbType.VarChar, 20, numero_identificacion);
                if (obj.Consultar("sp_Consultar_Cliente", true))
                {
                    NpgsqlDataReader dr = obj.Reader;
                    if (dr.Read())
                        return MapCliente(dr);
                }
            }
            return null;
        }

        private static Cliente MapCliente(NpgsqlDataReader dr)
        {
            return new Cliente()
            {
                numero_identificacion = dr["numero_identificacion"].ToString(),
                nombres = dr["nombres"].ToString(),
                tipo_documento = dr["tipo_documento"] == DBNull.Value ? "" : dr["tipo_documento"].ToString(),
                direccion = dr["direccion"] == DBNull.Value ? "" : dr["direccion"].ToString(),
                telefono = dr["telefono"] == DBNull.Value ? "" : dr["telefono"].ToString(),
                correo = dr["correo"].ToString(),
                fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                tipo_cliente = dr["tipo_cliente"] == DBNull.Value ? "" : dr["tipo_cliente"].ToString(),
                estado = dr["estado"].ToString(),
                cedula_adm = dr["cedula_adm"] == DBNull.Value ? "" : dr["cedula_adm"].ToString()
            };
        }
    }
}
