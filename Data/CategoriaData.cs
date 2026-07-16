using ApiEjemplo.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

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
                string sentencia = "INSERT INTO categoria (nombre, descripcion) SELECT '" +
                                   nombre.Replace("'", "''") + "', '' " +
                                   "WHERE NOT EXISTS (SELECT 1 FROM categoria WHERE nombre = '" +
                                   nombre.Replace("'", "''") + "')";
                using (ConexionBD obj = new ConexionBD())
                    obj.EjecutarSentencia(sentencia, false);
            }

            string nombresIn = string.Join(",", new string[]
            {
                "'Bolsas'","'Cajas Cartón Kraft'","'Cartones y papel germinable'",
                "'Cubiertos'","'Germinables'","'Otros productos'",
                "'Platos y vajillas'","'Tapas'","'vasos, contenedores'","'Bowls y portacomidas'"
            });
            using (ConexionBD obj = new ConexionBD())
            {
                obj.EjecutarSentencia(
                    "DELETE FROM categoria WHERE nombre NOT IN (" + nombresIn + ") " +
                    "AND numero_categoria NOT IN (SELECT DISTINCT numero_categoria FROM producto WHERE numero_categoria IS NOT NULL)",
                    false);
            }
        }

        public static bool InsertarCategoria(Categoria oCategoria)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@nombre", SqlDbType.VarChar, 100, oCategoria.nombre);
                obj.AgregarParametro(ParameterDirection.Input, "@descripcion", SqlDbType.VarChar, 300, oCategoria.descripcion ?? "");
                if (obj.EjecutarSentencia("sp_Insertar_Categoria", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool ActualizarCategoria(Categoria oCategoria)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@numero_categoria", SqlDbType.Int, 0, oCategoria.numero_categoria);
                obj.AgregarParametro(ParameterDirection.Input, "@nombre", SqlDbType.VarChar, 100, oCategoria.nombre);
                obj.AgregarParametro(ParameterDirection.Input, "@descripcion", SqlDbType.VarChar, 300, oCategoria.descripcion ?? "");
                if (obj.EjecutarSentencia("sp_Actualizar_Categoria", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static bool EliminarCategoria(int numero_categoria)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@numero_categoria", SqlDbType.Int, 0, numero_categoria);
                if (obj.EjecutarSentencia("sp_Eliminar_Categoria", true))
                    return true;
                ultimoError = obj.Error;
                return false;
            }
        }

        public static List<Categoria> ListarCategorias()
        {
            List<Categoria> lista = new List<Categoria>();
            using (ConexionBD obj = new ConexionBD())
            {
                if (obj.Consultar("sp_Listar_Categorias", true))
                {
                    NpgsqlDataReader dr = obj.Reader;
                    while (dr.Read())
                        lista.Add(MapCategoria(dr));
                }
            }
            return lista;
        }

        public static Categoria ConsultarCategoria(int numero_categoria)
        {
            using (ConexionBD obj = new ConexionBD())
            {
                obj.AgregarParametro(ParameterDirection.Input, "@numero_categoria", SqlDbType.Int, 0, numero_categoria);
                if (obj.Consultar("sp_Consultar_Categoria", true))
                {
                    NpgsqlDataReader dr = obj.Reader;
                    if (dr.Read())
                        return MapCategoria(dr);
                }
            }
            return null;
        }

        private static Categoria MapCategoria(NpgsqlDataReader dr)
        {
            return new Categoria()
            {
                numero_categoria = Convert.ToInt32(dr["numero_categoria"]),
                nombre = dr["nombre"].ToString(),
                descripcion = dr["descripcion"] == DBNull.Value ? "" : dr["descripcion"].ToString()
            };
        }
    }
}
