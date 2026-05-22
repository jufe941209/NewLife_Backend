using NewLife.Data;
using NewLife.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace NewLife.Controllers
{
    public class DespachoController : ApiController
    {
        // GET api/despacho
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Despacho> lista = DespachoData.ListarDespachos();
            return Ok(lista);
        }

        // GET api/despacho/1
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Despacho oDespacho = DespachoData.ConsultarDespacho(id);
            if (oDespacho != null)
                return Ok(oDespacho);
            else
                return NotFound();
        }

        // POST api/despacho
        [HttpPost]
        public IHttpActionResult Post([FromBody] Despacho oDespacho)
        {
            if (oDespacho == null)
                return BadRequest("Datos inválidos.");

            bool resultado = DespachoData.InsertarDespacho(oDespacho);
            if (resultado)
                return Ok("Despacho registrado exitosamente.");
            else
                return BadRequest(DespachoData.ultimoError);
        }

        // PUT api/despacho
        [HttpPut]
        public IHttpActionResult Put([FromBody] Despacho oDespacho)
        {
            if (oDespacho == null)
                return BadRequest("Datos inválidos.");

            // Domiciliario obligatorio para estados activos
            if ((oDespacho.estado == "En camino" || oDespacho.estado == "Entregado") &&
                string.IsNullOrEmpty(oDespacho.cc_domiciliario))
            {
                return BadRequest("Un despacho en estado '" + oDespacho.estado + "' debe tener un domiciliario asignado.");
            }

            bool resultado = DespachoData.ActualizarDespacho(oDespacho);
            if (resultado)
                return Ok("Despacho actualizado exitosamente.");
            else
                return BadRequest(DespachoData.ultimoError);
        }

        // DELETE api/despacho/1
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            bool resultado = DespachoData.EliminarDespacho(id);
            if (resultado)
                return Ok("Despacho eliminado exitosamente.");
            else
                return BadRequest(DespachoData.ultimoError);
        }
    }
}
