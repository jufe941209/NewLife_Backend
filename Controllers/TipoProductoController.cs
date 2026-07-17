using Microsoft.AspNetCore.Mvc;
using NewLife.Data;
using NewLife.Models;

namespace NewLife.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoProductoController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            List<TipoProducto> lista = TipoProductoData.ListarTiposProducto();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            TipoProducto oTipoProducto = TipoProductoData.ConsultarTipoProducto(id);
            if (oTipoProducto != null)
                return Ok(oTipoProducto);
            else
                return NotFound();
        }

        [HttpPost]
        public IActionResult Post([FromBody] TipoProducto oTipoProducto)
        {
            if (oTipoProducto == null)
                return BadRequest("Datos inválidos.");

            bool resultado = TipoProductoData.InsertarTipoProducto(oTipoProducto);
            if (resultado)
                return Ok("Tipo de producto registrado exitosamente.");
            else
                return BadRequest(TipoProductoData.ultimoError);
        }

        [HttpPut]
        public IActionResult Put([FromBody] TipoProducto oTipoProducto)
        {
            if (oTipoProducto == null)
                return BadRequest("Datos inválidos.");

            bool resultado = TipoProductoData.ActualizarTipoProducto(oTipoProducto);
            if (resultado)
                return Ok("Tipo de producto actualizado exitosamente.");
            else
                return BadRequest(TipoProductoData.ultimoError);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool resultado = TipoProductoData.EliminarTipoProducto(id);
            if (resultado)
                return Ok("Tipo de producto eliminado exitosamente.");
            else
                return BadRequest(TipoProductoData.ultimoError);
        }
    }
}
