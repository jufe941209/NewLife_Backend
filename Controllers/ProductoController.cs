using NewLife.Data;
using NewLife.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace NewLife.Controllers
{
    public class ProductoController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Producto> lista = ProductoData.ListarProductos();
            return Ok(lista);
        }

        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            Producto oProducto = ProductoData.ConsultarProducto(id);
            if (oProducto != null)
                return Ok(oProducto);
            else
                return NotFound();
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] Producto oProducto)
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
        public IHttpActionResult Put([FromBody] Producto oProducto)
        {
            if (oProducto == null)
                return BadRequest("Datos inválidos.");

            bool resultado = ProductoData.ActualizarProducto(oProducto);
            if (resultado)
                return Ok("Producto actualizado exitosamente.");
            else
                return BadRequest(ProductoData.ultimoError);
        }

        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            bool resultado = ProductoData.EliminarProducto(id);
            if (resultado)
                return Ok("Producto eliminado exitosamente.");
            else
                return BadRequest(ProductoData.ultimoError);
        }

        [HttpPost]
        [Route("api/producto/migrar-stock")]
        public IHttpActionResult MigrarStock()
        {
            string resultado = ProductoData.MigrarStockReal();
            return Ok(resultado);
        }

        [HttpPost]
        [Route("api/producto/reducir-stock/{id}/{cantidad}")]
        public IHttpActionResult ReducirStock(string id, int cantidad)
        {
            bool resultado = ProductoData.ReducirStock(id, cantidad);
            if (resultado)
                return Ok("Stock reducido.");
            else
                return BadRequest(ProductoData.ultimoError);
        }
    }
}