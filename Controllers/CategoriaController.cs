using NewLife.Data;
using NewLife.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace NewLife.Controllers
{
    public class CategoriaController : ApiController
    {
        // GET api/categoria
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Categoria> lista = CategoriaData.ListarCategorias();
            if (lista.Count > 0)
                return Ok(lista);
            else
                return NotFound();
        }

        // GET api/categoria/1
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Categoria oCategoria = CategoriaData.ConsultarCategoria(id);
            if (oCategoria != null)
                return Ok(oCategoria);
            else
                return NotFound();
        }

        // POST api/categoria
        [HttpPost]
        public IHttpActionResult Post([FromBody] Categoria oCategoria)
        {
            if (oCategoria == null)
                return BadRequest("Datos inválidos.");

            bool resultado = CategoriaData.InsertarCategoria(oCategoria);
            if (resultado)
                return Ok("Categoría registrada exitosamente.");
            else
                return BadRequest("No se pudo registrar la categoría.");
        }

        // PUT api/categoria
        [HttpPut]
        public IHttpActionResult Put([FromBody] Categoria oCategoria)
        {
            if (oCategoria == null)
                return BadRequest("Datos inválidos.");

            bool resultado = CategoriaData.ActualizarCategoria(oCategoria);
            if (resultado)
                return Ok("Categoría actualizada exitosamente.");
            else
                return BadRequest("No se pudo actualizar la categoría.");
        }

        // DELETE api/categoria/1
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            bool resultado = CategoriaData.EliminarCategoria(id);
            if (resultado)
                return Ok("Categoría eliminada exitosamente.");
            else
                return BadRequest(CategoriaData.ultimoError); // error real
        }
    }
}