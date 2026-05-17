using NewLife.Data;
using NewLife.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace NewLife.Controllers
{
    public class TransporteController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Transporte> lista = TransporteData.ListarTransportes();
            if (lista.Count > 0)
                return Ok(lista);
            else
                return NotFound();
        }

        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            Transporte oTransporte = TransporteData.ConsultarTransporte(id);
            if (oTransporte != null)
                return Ok(oTransporte);
            else
                return NotFound();
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] Transporte oTransporte)
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
        public IHttpActionResult Put([FromBody] Transporte oTransporte)
        {
            if (oTransporte == null)
                return BadRequest("Datos inválidos.");

            bool resultado = TransporteData.ActualizarTransporte(oTransporte);
            if (resultado)
                return Ok("Transporte actualizado exitosamente.");
            else
                return BadRequest(TransporteData.ultimoError);
        }

        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            bool resultado = TransporteData.EliminarTransporte(id);
            if (resultado)
                return Ok("Transporte eliminado exitosamente.");
            else
                return BadRequest(TransporteData.ultimoError);
        }
    }
}