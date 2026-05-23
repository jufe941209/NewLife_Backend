using NewLife.Data;
using NewLife.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace NewLife.Controllers
{
    public class FacturaVentaController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<FacturaVenta> lista = FacturaVentaData.ListarFacturasVenta();
            return Ok(lista);
        }

        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            FacturaVenta oFactura = FacturaVentaData.ConsultarFacturaVenta(id);
            if (oFactura != null)
                return Ok(oFactura);
            else
                return NotFound();
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] FacturaVenta oFactura)
        {
            if (oFactura == null)
                return BadRequest("Datos inválidos.");

            bool resultado = FacturaVentaData.InsertarFacturaVenta(oFactura);
            if (resultado)
                return Ok("Factura de venta registrada exitosamente.");
            else
                return BadRequest(FacturaVentaData.ultimoError);
        }

        [HttpPut]
        public IHttpActionResult Put([FromBody] FacturaVenta oFactura)
        {
            if (oFactura == null)
                return BadRequest("Datos inválidos.");

            bool resultado = FacturaVentaData.ActualizarFacturaVenta(oFactura);
            if (!resultado)
                return BadRequest(FacturaVentaData.ultimoError);

            // Auto-crear despacho cuando la factura se aprueba o paga por primera vez
            if ((oFactura.estado_pago == "Aprobado" || oFactura.estado_pago == "Pagado")
                && !DespachoData.ExisteDespachoParaFactura(oFactura.numero_factura))
            {
                var despacho = new Despacho
                {
                    fecha_despacho = System.DateTime.Now,
                    numero_factura = oFactura.numero_factura,
                    cc_responsable = "",
                    cc_domiciliario = "",
                    estado = "Pendiente"
                };
                DespachoData.InsertarDespacho(despacho);
            }

            return Ok("Factura de venta actualizada exitosamente.");
        }

        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            bool resultado = FacturaVentaData.EliminarFacturaVenta(id);
            if (resultado)
                return Ok("Factura de venta eliminada exitosamente.");
            else
                return BadRequest(FacturaVentaData.ultimoError);
        }
    }
}