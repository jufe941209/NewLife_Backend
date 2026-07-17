using Microsoft.AspNetCore.Mvc;
using NewLife.Data;
using NewLife.Models;

namespace NewLife.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetalleFacturaController : ControllerBase
    {
        // GET api/detallefactura
        [HttpGet]
        public IActionResult Get()
        {
            List<DetalleFactura> lista = DetalleFacturaData.ListarDetalleFactura();
            return Ok(lista);
        }

        // GET api/detallefactura/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            DetalleFactura oDetalle = DetalleFacturaData.ConsultarDetalleFactura(id);
            if (oDetalle != null)
                return Ok(oDetalle);
            else
                return NotFound();
        }

        // GET api/detallefactura/porfactura/FV-2024-001
        [HttpGet]
        [Route("/api/detallefactura/porfactura/{numeroFactura}")]
        public IActionResult GetPorFactura(string numeroFactura)
        {
            List<DetalleFactura> lista = DetalleFacturaData.ListarDetalleFacturaPorFactura(numeroFactura);
            return Ok(lista);
        }

        // POST api/detallefactura
        [HttpPost]
        public IActionResult Post([FromBody] DetalleFactura oDetalle)
        {
            if (oDetalle == null)
                return BadRequest("Datos inválidos.");
            bool resultado = DetalleFacturaData.InsertarDetalleFactura(oDetalle);
            if (resultado)
                return Ok("Detalle de factura registrado exitosamente.");
            else
                return BadRequest(DetalleFacturaData.ultimoError);
        }

        // PUT api/detallefactura
        [HttpPut]
        public IActionResult Put([FromBody] DetalleFactura oDetalle)
        {
            if (oDetalle == null)
                return BadRequest("Datos inválidos.");
            bool resultado = DetalleFacturaData.ActualizarDetalleFactura(oDetalle);
            if (resultado)
                return Ok("Detalle de factura actualizado exitosamente.");
            else
                return BadRequest(DetalleFacturaData.ultimoError);
        }

        // DELETE api/detallefactura/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool resultado = DetalleFacturaData.EliminarDetalleFactura(id);
            if (resultado)
                return Ok("Detalle de factura eliminado exitosamente.");
            else
                return BadRequest(DetalleFacturaData.ultimoError);
        }

        // POST api/detallefactura/migrar-precio
        [HttpPost]
        [Route("/api/detallefactura/migrar-precio")]
        public IActionResult MigrarPrecio()
        {
            string resultado = DetalleFacturaData.MigrarPrecioUnitario();
            return Ok(resultado);
        }
    }
}
