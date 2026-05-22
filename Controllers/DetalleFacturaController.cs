using NewLife.Data;
using NewLife.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace NewLife.Controllers
{
    public class DetalleFacturaController : ApiController
    {
        // GET api/detallefactura
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<DetalleFactura> lista = DetalleFacturaData.ListarDetalleFactura();
            if (lista.Count > 0)
                return Ok(lista);
            else
                return NotFound();
        }

        // GET api/detallefactura/1
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            DetalleFactura oDetalle = DetalleFacturaData.ConsultarDetalleFactura(id);
            if (oDetalle != null)
                return Ok(oDetalle);
            else
                return NotFound();
        }

        // GET api/detallefactura?numerofactura=FV-2024-001
        [HttpGet]
        [Route("api/detallefactura/porfactura/{numeroFactura}")]
        public IHttpActionResult GetPorFactura(string numeroFactura)
        {
            List<DetalleFactura> lista = DetalleFacturaData.ListarDetalleFacturaPorFactura(numeroFactura);
            if (lista.Count > 0)
                return Ok(lista);
            else
                return NotFound();
        }

        // POST api/detallefactura
        [HttpPost]
        public IHttpActionResult Post([FromBody] DetalleFactura oDetalle)
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
        public IHttpActionResult Put([FromBody] DetalleFactura oDetalle)
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
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            bool resultado = DetalleFacturaData.EliminarDetalleFactura(id);
            if (resultado)
                return Ok("Detalle de factura eliminado exitosamente.");
            else
                return BadRequest(DetalleFacturaData.ultimoError);
        }

        // POST api/detallefactura/migrar-precio
        [HttpPost]
        [Route("api/detallefactura/migrar-precio")]
        public IHttpActionResult MigrarPrecio()
        {
            string resultado = DetalleFacturaData.MigrarPrecioUnitario();
            return Ok(resultado);
        }
    }
}