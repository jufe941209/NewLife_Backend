using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class CategoriaData
    {
        // INSERTAR (no recibe numero_categoria, es autoincremental)
        public static bool InsertarCategoria(Categoria oCategoria)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Insertar_Categoria '" + oCategoria.nombre + "','" +
                               oCategoria.descripcion + "'";

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

        // ACTUALIZAR
        public static bool ActualizarCategoria(Categoria oCategoria)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Actualizar_Categoria " + oCategoria.numero_categoria + ",'" +
                               oCategoria.nombre + "','" +
                               oCategoria.descripcion + "'";

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
        public static string ultimoError = "";

        public static bool EliminarCategoria(int numero_categoria)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Eliminar_Categoria " + numero_categoria;

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
        public static List<Categoria> ListarCategorias()
        {
            List<Categoria> lista = new List<Categoria>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Listar_Categorias";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                while (dr.Read())
                {
                    lista.Add(new Categoria()
                    {
                        numero_categoria = Convert.ToInt32(dr["numero_categoria"]),
                        nombre = dr["nombre"].ToString(),
                        descripcion = dr["descripcion"].ToString()
                    });
                }
            }
            return lista;
        }

        // CONSULTAR POR ID
        public static Categoria ConsultarCategoria(int numero_categoria)
        {
            Categoria oCategoria = null;
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXEC sp_Consultar_Categoria " + numero_categoria;

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                if (dr.Read())
                {
                    oCategoria = new Categoria()
                    {
                        numero_categoria = Convert.ToInt32(dr["numero_categoria"]),
                        nombre = dr["nombre"].ToString(),
                        descripcion = dr["descripcion"].ToString()
                    };
                }
            }
            return oCategoria;
        }
    }
}