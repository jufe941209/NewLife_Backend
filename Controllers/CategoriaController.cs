using Microsoft.AspNetCore.Mvc;
using NewLife.Data;
using NewLife.Models;

namespace NewLife.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        // GET api/categoria
        [HttpGet]
        public IActionResult Get()
        {
            List<Categoria> lista = CategoriaData.ListarCategorias();
            return Ok(lista);
        }

        // GET api/categoria/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Categoria oCategoria = CategoriaData.ConsultarCategoria(id);
            if (oCategoria != null)
                return Ok(oCategoria);
            else
                return NotFound();
        }

        // POST api/categoria
        [HttpPost]
        public IActionResult Post([FromBody] Categoria oCategoria)
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
        public IActionResult Put([FromBody] Categoria oCategoria)
        {
            if (oCategoria == null)
                return BadRequest("Datos inválidos.");

            bool resultado = CategoriaData.ActualizarCategoria(oCategoria);
            if (resultado)
                return Ok("Categoría actualizada exitosamente.");
            else
                return BadRequest("No se pudo actualizar la categoría.");
        }

        // PUT api/categoria/{id} — el frontend manda el id en la URL
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Categoria oCategoria)
        {
            if (oCategoria == null)
                return BadRequest("Datos inválidos.");
            oCategoria.numero_categoria = id;
            return Put(oCategoria);
        }

        // DELETE api/categoria/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool resultado = CategoriaData.EliminarCategoria(id);
            if (resultado)
                return Ok("Categoría eliminada exitosamente.");
            else
                return BadRequest(CategoriaData.ultimoError); // error real
        }
    }
}
