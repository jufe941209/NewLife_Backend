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
            if (lista.Count > 0)
                return Ok(lista);
            else
                return NotFound();
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

            bool resultado = DespachoData.ActualizarDespacho(oDespacho);
            if (resultado)
                return Ok("Despacho actualizado exitosamente.");
            else
                return BadRequest("No se pudo actualizar el despacho.");
        }

        // DELETE api/despacho/1
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            bool resultado = DespachoData.EliminarDespacho(id);
            if (resultado)
                return Ok("Despacho eliminado exitosamente.");
            else
                return BadRequest("No se pudo eliminar el despacho.");
        }
    }
}