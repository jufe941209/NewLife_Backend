using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class TipoProductoData
    {
        public static string ultimoError = "";

        public static bool InsertarTipoProducto(TipoProducto oTipoProducto)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Insertar_TipoProducto '" + oTipoProducto.nombre + "','" +
                               oTipoProducto.descripcion + "'";

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

        public static bool ActualizarTipoProducto(TipoProducto oTipoProducto)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Actualizar_TipoProducto " + oTipoProducto.id_tipo_producto + ",'" +
                               oTipoProducto.nombre + "','" +
                               oTipoProducto.descripcion + "'";

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

        public static bool EliminarTipoProducto(int id_tipo_producto)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Eliminar_TipoProducto " + id_tipo_producto;

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

        public static List<TipoProducto> ListarTiposProducto()
        {
            List<TipoProducto> lista = new List<TipoProducto>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Listar_TiposProducto";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                while (dr.Read())
                {
                    lista.Add(new TipoProducto()
                    {
                        id_tipo_producto = Convert.ToInt32(dr["id_tipo_producto"]),
                        nombre = dr["nombre"].ToString(),
                        descripcion = dr["descripcion"].ToString()
                    });
                }
            }
            return lista;
        }

        public static TipoProducto ConsultarTipoProducto(int id_tipo_producto)
        {
            TipoProducto oTipoProducto = null;
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Consultar_TipoProducto " + id_tipo_producto;

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                if (dr.Read())
                {
                    oTipoProducto = new TipoProducto()
                    {
                        id_tipo_producto = Convert.ToInt32(dr["id_tipo_producto"]),
                        nombre = dr["nombre"].ToString(),
                        descripcion = dr["descripcion"].ToString()
                    };
                }
            }
            return oTipoProducto;
        }
    }
}