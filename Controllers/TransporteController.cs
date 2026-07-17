using Microsoft.AspNetCore.Mvc;
using NewLife.Data;
using NewLife.Models;

namespace NewLife.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransporteController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            List<Transporte> lista = TransporteData.ListarTransportes();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            Transporte oTransporte = TransporteData.ConsultarTransporte(id);
            if (oTransporte != null)
                return Ok(oTransporte);
            else
                return NotFound();
        }

        [HttpPost]
        public IActionResult Post([FromBody] Transporte oTransporte)
        {
            if (oTransporte == null)
                return BadRequest("Datos inválidos.");

            bool resultado = TransporteData.InsertarTransporte(oTransporte);
            if (resultado)
                return Ok("Transporte registrado exitosamente.");
            else
                return BadRequest(TransporteData.ultimoError);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Transporte oTransporte)
        {
            if (oTransporte == null)
                return BadRequest("Datos inválidos.");

            bool resultado = TransporteData.ActualizarTransporte(oTransporte);
            if (resultado)
                return Ok("Transporte actualizado exitosamente.");
            else
                return BadRequest(TransporteData.ultimoError);
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Transporte oTransporte)
        {
            if (oTransporte == null)
                return BadRequest("Datos inválidos.");
            oTransporte.placa = id;
            return Put(oTransporte);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            bool resultado = TransporteData.EliminarTransporte(id);
            if (resultado)
                return Ok("Transporte eliminado exitosamente.");
            else
                return BadRequest(TransporteData.ultimoError);
        }
    }
}
