using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class ClienteData
    {
        public static string ultimoError = "";

        public static bool InsertarCliente(Cliente oCliente)
        {
            string sentencia = "INSERT INTO CLIENTE (numero_identificacion, nombres, tipo_documento, direccion, telefono, correo, contrasena, tipo_cliente, estado, fecha_registro, cedula_adm) VALUES ('" +
                               oCliente.numero_identificacion.Replace("'", "''") + "','" +
                               oCliente.nombres.Replace("'", "''") + "','" +
                               (oCliente.tipo_documento ?? "CC").Replace("'", "''") + "','" +
                               (oCliente.direccion ?? "").Replace("'", "''") + "','" +
                               (oCliente.telefono ?? "").Replace("'", "''") + "','" +
                               oCliente.correo.Replace("'", "''") + "','" +
                               (oCliente.contrasena ?? "111111").Replace("'", "''") + "','" +
                               (oCliente.tipo_cliente ?? "Regular").Replace("'", "''") + "','" +
                               (oCliente.estado ?? "Activo") + "',GETDATE(),'" +
                               (oCliente.cedula_adm ?? "").Replace("'", "''") + "')";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static bool ActualizarCliente(Cliente oCliente)
        {
            string sentencia = "UPDATE CLIENTE SET " +
                               "nombres = '" + oCliente.nombres.Replace("'", "''") + "', " +
                               "tipo_documento = '" + (oCliente.tipo_documento ?? "CC").Replace("'", "''") + "', " +
                               "direccion = '" + (oCliente.direccion ?? "").Replace("'", "''") + "', " +
                               "telefono = '" + (oCliente.telefono ?? "").Replace("'", "''") + "', " +
                               "correo = '" + oCliente.correo.Replace("'", "''") + "', " +
                               "tipo_cliente = '" + (oCliente.tipo_cliente ?? "Regular").Replace("'", "''") + "', " +
                               "estado = '" + (oCliente.estado ?? "Activo") + "', " +
                               "cedula_adm = '" + (oCliente.cedula_adm ?? "").Replace("'", "''") + "' " +
                               "WHERE numero_identificacion = '" + oCliente.numero_identificacion.Replace("'", "''") + "'";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static bool EliminarCliente(string numero_identificacion)
        {
            string sentencia = "DELETE FROM CLIENTE WHERE numero_identificacion = '" + numero_identificacion.Replace("'", "''") + "'";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.EjecutarSentencia(sentencia, false))
                    return true;
                ultimoError = objEst.Error;
                return false;
            }
        }

        public static List<Cliente> ListarClientes()
        {
            List<Cliente> lista = new List<Cliente>();
            string sentencia = "SELECT numero_identificacion, nombres, tipo_documento, direccion, telefono, correo, fecha_registro, tipo_cliente, estado, cedula_adm FROM CLIENTE ORDER BY nombres";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    while (dr.Read())
                    {
                        lista.Add(new Cliente()
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
                        });
                    }
                }
            }
            return lista;
        }

        public static Cliente LoginCliente(string correo, string contrasena)
        {
            Cliente oCliente = null;
            string sentencia = "SELECT numero_identificacion, nombres, tipo_documento, direccion, telefono, correo, fecha_registro, tipo_cliente, estado, cedula_adm FROM CLIENTE " +
                               "WHERE correo = '" + correo.Replace("'", "''") + "' AND contrasena = '" + contrasena.Replace("'", "''") + "' AND estado = 'Activo'";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    if (dr.Read())
                    {
                        oCliente = new Cliente()
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
            return oCliente;
        }

        public static bool CambiarContrasena(string correo, string nuevaContrasena)
        {
            string sentencia = "UPDATE CLIENTE SET contrasena = '" + nuevaContrasena.Replace("'", "''") +
                               "' WHERE correo = '" + correo.Replace("'", "''") + "'";
            using (ConexionBD obj = new ConexionBD())
            {
                return obj.EjecutarSentencia(sentencia, false);
            }
        }

        public static bool ExisteCorreo(string correo)
        {
            string sentencia = "SELECT COUNT(*) FROM CLIENTE WHERE correo = '" + correo.Replace("'", "''") + "' AND estado = 'Activo'";
            using (ConexionBD obj = new ConexionBD())
            {
                return obj.ConsultarValorUnico(sentencia, false) && Convert.ToInt32(obj.ValorUnico) > 0;
            }
        }

        public static Cliente ConsultarCliente(string numero_identificacion)
        {
            Cliente oCliente = null;
            string sentencia = "SELECT numero_identificacion, nombres, tipo_documento, direccion, telefono, correo, fecha_registro, tipo_cliente, estado, cedula_adm FROM CLIENTE WHERE numero_identificacion = '" +
                               numero_identificacion.Replace("'", "''") + "'";

            using (ConexionBD objEst = new ConexionBD())
            {
                if (objEst.Consultar(sentencia, false))
                {
                    SqlDataReader dr = objEst.Reader;
                    if (dr.Read())
                    {
                        oCliente = new Cliente()
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
            return oCliente;
        }
    }
}
