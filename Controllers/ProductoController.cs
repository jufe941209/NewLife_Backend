using Microsoft.AspNetCore.Mvc;
using NewLife.Data;
using NewLife.Models;

namespace NewLife.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            List<Producto> lista = ProductoData.ListarProductos();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            Producto oProducto = ProductoData.ConsultarProducto(id);
            if (oProducto != null)
                return Ok(oProducto);
            else
                return NotFound();
        }

        [HttpPost]
        public IActionResult Post([FromBody] Producto oProducto)
        {
            if (oProducto == null)
                return BadRequest("Datos inválidos.");

            bool resultado = ProductoData.InsertarProducto(oProducto);
            if (resultado)
                return Ok("Producto registrado exitosamente.");
            else
                return BadRequest(ProductoData.ultimoError);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Producto oProducto)
        {
            if (oProducto == null)
                return BadRequest("Datos inválidos.");

            bool resultado = ProductoData.ActualizarProducto(oProducto);
            if (resultado)
                return Ok("Producto actualizado exitosamente.");
            else
                return BadRequest(ProductoData.ultimoError);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            bool resultado = ProductoData.EliminarProducto(id);
            if (resultado)
                return Ok("Producto eliminado exitosamente.");
            else
                return BadRequest(ProductoData.ultimoError);
        }

        [HttpPost]
        [Route("/api/producto/migrar-stock")]
        public IActionResult MigrarStock()
        {
            string resultado = ProductoData.MigrarStockReal();
            return Ok(resultado);
        }

        [HttpPost]
        [Route("/api/producto/reducir-stock/{id}/{cantidad}")]
        public IActionResult ReducirStock(string id, int cantidad)
        {
            bool resultado = ProductoData.ReducirStock(id, cantidad);
            if (resultado)
                return Ok("Stock reducido.");
            else
                return BadRequest(ProductoData.ultimoError);
        }
    }
}
