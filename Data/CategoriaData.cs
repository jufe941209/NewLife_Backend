using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewLife.Data
{
    public class CategoriaData
    {
        public static string ultimoError = "";

        public static void MigrarCategorias()
        {
            var categorias = new string[]
            {
                "Bolsas", "Cajas Cartón Kraft", "Cartones y papel germinable",
                "Cubiertos", "Germinables", "Otros productos",
                "Platos y vajillas", "Tapas", "vasos, contenedores", "Bowls y portacomidas"
            };
            foreach (var nombre in categorias)
            {
                ConexionBD obj = new ConexionBD();
                string sentencia = "IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE nombre = '" +
                                   nombre.Replace("'", "''") + "') " +
                                   "INSERT INTO CATEGORIA (nombre, descripcion) VALUES ('" +
                                   nombre.Replace("'", "''") + "', '')";
                obj.EjecutarSentencia(sentencia, false);
            }
            // Eliminar categorías antiguas que no tengan productos asociados y no estén en la lista nueva
            string nombresIn = string.Join(",", new string[]
            {
                "'Bolsas'","'Cajas Cartón Kraft'","'Cartones y papel germinable'",
                "'Cubiertos'","'Germinables'","'Otros productos'",
                "'Platos y vajillas'","'Tapas'","'vasos, contenedores'","'Bowls y portacomidas'"
            });
            ConexionBD objDel = new ConexionBD();
            objDel.EjecutarSentencia(
                "DELETE FROM CATEGORIA WHERE nombre NOT IN (" + nombresIn + ") " +
                "AND numero_categoria NOT IN (SELECT DISTINCT numero_categoria FROM PRODUCTO WHERE numero_categoria IS NOT NULL)",
                false);
        }

        public static bool InsertarCategoria(Categoria oCategoria)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "INSERT INTO CATEGORIA (nombre, descripcion) VALUES ('" +
                               oCategoria.nombre.Replace("'", "''") + "','" +
                               (oCategoria.descripcion ?? "").Replace("'", "''") + "')";

            if (objEst.EjecutarSentencia(sentencia, false))
            { objEst = null; return true; }
            else
            { ultimoError = objEst.Error; objEst = null; return false; }
        }

        public static bool ActualizarCategoria(Categoria oCategoria)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "UPDATE CATEGORIA SET nombre = '" +
                               oCategoria.nombre.Replace("'", "''") + "', descripcion = '" +
                               (oCategoria.descripcion ?? "").Replace("'", "''") +
                               "' WHERE numero_categoria = " + oCategoria.numero_categoria;

            if (objEst.EjecutarSentencia(sentencia, false))
            { objEst = null; return true; }
            else
            { ultimoError = objEst.Error; objEst = null; return false; }
        }

        public static bool EliminarCategoria(int numero_categoria)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "DELETE FROM CATEGORIA WHERE numero_categoria = " + numero_categoria;

            if (objEst.EjecutarSentencia(sentencia, false))
            { objEst = null; return true; }
            else
            { ultimoError = objEst.Error; objEst = null; return false; }
        }

        public static List<Categoria> ListarCategorias()
        {
            List<Categoria> lista = new List<Categoria>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "SELECT numero_categoria, nombre, descripcion FROM CATEGORIA ORDER BY nombre";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                while (dr.Read())
                {
                    lista.Add(new Categoria()
                    {
                        numero_categoria = Convert.ToInt32(dr["numero_categoria"]),
                        nombre = dr["nombre"].ToString(),
                        descripcion = dr["descripcion"] == DBNull.Value ? "" : dr["descripcion"].ToString()
                    });
                }
            }
            return lista;
        }

        public static Categoria ConsultarCategoria(int numero_categoria)
        {
            Categoria oCategoria = null;
            ConexionBD objEst = new ConexionBD();
            string sentencia = "SELECT numero_categoria, nombre, descripcion FROM CATEGORIA WHERE numero_categoria = " + numero_categoria;

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                if (dr.Read())
                {
                    oCategoria = new Categoria()
                    {
                        numero_categoria = Convert.ToInt32(dr["numero_categoria"]),
                        nombre = dr["nombre"].ToString(),
                        descripcion = dr["descripcion"] == DBNull.Value ? "" : dr["descripcion"].ToString()
                    };
                }
            }
            return oCategoria;
        }
    }
}
