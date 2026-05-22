using NewLife.Data;
using NewLife.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace NewLife.Controllers
{
    public class DomiciliarioController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Domiciliario> lista = DomiciliarioData.ListarDomiciliarios();
            return Ok(lista);
        }

        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            Domiciliario oDomiciliario = DomiciliarioData.ConsultarDomiciliario(id);
            if (oDomiciliario != null)
                return Ok(oDomiciliario);
            else
                return NotFound();
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] Domiciliario oDomiciliario)
        {
            if (oDomiciliario == null)
                return BadRequest("Datos inválidos.");

            bool resultado = DomiciliarioData.InsertarDomiciliario(oDomiciliario);
            if (resultado)
                return Ok("Domiciliario registrado exitosamente.");
            else
                return BadRequest(DomiciliarioData.ultimoError);
        }

        [HttpPut]
        public IHttpActionResult Put([FromBody] Domiciliario oDomiciliario)
        {
            if (oDomiciliario == null)
                return BadRequest("Datos inválidos.");

            bool resultado = DomiciliarioData.ActualizarDomiciliario(oDomiciliario);
            if (resultado)
                return Ok("Domiciliario actualizado exitosamente.");
            else
                return BadRequest(DomiciliarioData.ultimoError);
        }

        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            bool resultado = DomiciliarioData.EliminarDomiciliario(id);
            if (resultado)
                return Ok("Domiciliario eliminado exitosamente.");
            else
                return BadRequest(DomiciliarioData.ultimoError);
        }

        [HttpPost]
        [Route("api/domiciliario/cleanup")]
        public IHttpActionResult Cleanup()
        {
            string resultado = DomiciliarioData.LimpiarDomiciliariosPrueba();
            return Ok(resultado);
        }
    }
}