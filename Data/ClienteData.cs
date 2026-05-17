using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class ClienteData
    {
        // INSERTAR
        public static string ultimoError = "";

        public static bool InsertarCliente(Cliente oCliente)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Insertar_Cliente '" + oCliente.numero_identificacion + "','" +
                               oCliente.nombres + "','" +
                               oCliente.tipo_documento + "','" +
                               oCliente.direccion + "','" +
                               oCliente.telefono + "','" +
                               oCliente.correo + "','" +
                               oCliente.contrasena + "','" +
                               oCliente.tipo_cliente + "','" +
                               oCliente.estado + "'," +
                               oCliente.cedula_adm;

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
        public static bool ActualizarCliente(Cliente oCliente)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Actualizar_Cliente '" + oCliente.numero_identificacion + "','" +
                               oCliente.nombres + "','" +
                               oCliente.tipo_documento + "','" +
                               oCliente.direccion + "','" +
                               oCliente.telefono + "','" +
                               oCliente.correo + "','" +
                               oCliente.tipo_cliente + "','" +
                               oCliente.estado + "'," +
                               oCliente.cedula_adm;

            if (objEst.EjecutarSentencia(sentencia, false))
            {
                objEst = null;
                return true;
            }
            else
            {
                objEst = null;
                return false;
            }
        }

        // ELIMINAR
        public static bool EliminarCliente(string numero_identificacion)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Eliminar_Cliente '" + numero_identificacion + "'";

            if (objEst.EjecutarSentencia(sentencia, false))
            {
                objEst = null;
                return true;
            }
            else
            {
                objEst = null;
                return false;
            }
        }

        // LISTAR
        public static List<Cliente> ListarClientes()
        {
            List<Cliente> lista = new List<Cliente>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Listar_Clientes";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                while (dr.Read())
                {
                    lista.Add(new Cliente()
                    {
                        numero_identificacion = dr["numero_identificacion"].ToString(),
                        nombres = dr["nombres"].ToString(),
                        tipo_documento = dr["tipo_documento"].ToString(),
                        direccion = dr["direccion"].ToString(),
                        telefono = dr["telefono"].ToString(),
                        correo = dr["correo"].ToString(),
                        fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                        tipo_cliente = dr["tipo_cliente"].ToString(),
                        estado = dr["estado"].ToString(),
                        cedula_adm = dr["cedula_adm"].ToString()
                    });
                }
            }
            return lista;
        }

        // CONSULTAR POR IDENTIFICACION
        public static Cliente ConsultarCliente(string numero_identificacion)
        {
            Cliente oCliente = null;
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Consultar_Cliente '" + numero_identificacion + "'";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                if (dr.Read())
                {
                    oCliente = new Cliente()
                    {
                        numero_identificacion = dr["numero_identificacion"].ToString(),
                        nombres = dr["nombres"].ToString(),
                        tipo_documento = dr["tipo_documento"].ToString(),
                        direccion = dr["direccion"].ToString(),
                        telefono = dr["telefono"].ToString(),
                        correo = dr["correo"].ToString(),
                        fecha_registro = Convert.ToDateTime(dr["fecha_registro"]),
                        tipo_cliente = dr["tipo_cliente"].ToString(),
                        estado = dr["estado"].ToString(),
                        cedula_adm = dr["cedula_adm"].ToString()
                    };
                }
            }
            return oCliente;
        }
    }
    }