using NewLife.Data;
using NewLife.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace NewLife.Controllers
{
    public class ResponsableController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Responsable> lista = ResponsableData.ListarResponsables();
            if (lista.Count > 0)
                return Ok(lista);
            else
                return NotFound();
        }

        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            Responsable oResponsable = ResponsableData.ConsultarResponsable(id);
            if (oResponsable != null)
                return Ok(oResponsable);
            else
                return NotFound();
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] Responsable oResponsable)
        {
            if (oResponsable == null)
                return BadRequest("Datos inválidos.");

            bool resultado = ResponsableData.InsertarResponsable(oResponsable);
            if (resultado)
                return Ok("Responsable registrado exitosamente.");
            else
                return BadRequest(ResponsableData.ultimoError);
        }

        [HttpPut]
        public IHttpActionResult Put([FromBody] Responsable oResponsable)
        {
            if (oResponsable == null)
                return BadRequest("Datos inválidos.");

            bool resultado = ResponsableData.ActualizarResponsable(oResponsable);
            if (resultado)
                return Ok("Responsable actualizado exitosamente.");
            else
                return BadRequest(ResponsableData.ultimoError);
        }

        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            bool resultado = ResponsableData.EliminarResponsable(id);
            if (resultado)
                return Ok("Responsable eliminado exitosamente.");
            else
                return BadRequest(ResponsableData.ultimoError);
        }
    }
}