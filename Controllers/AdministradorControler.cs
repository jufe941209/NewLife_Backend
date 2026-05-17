using NewLife.Data;
using NewLife.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace NewLife.Controllers
{
    public class AdministradorController : ApiController
    {
        // GET api/administrador
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Administrador> lista = AdministradorData.ListarAdministradores();
            if (lista.Count > 0)
                return Ok(lista);
            else
                return NotFound();
        }

        // GET api/administrador/1001234567
        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            Administrador oAdministrador = AdministradorData.ConsultarAdministrador(id);
            if (oAdministrador != null)
                return Ok(oAdministrador);
            else
                return NotFound();
        }

        // POST api/administrador
        [HttpPost]
        public IHttpActionResult Post([FromBody] Administrador oAdministrador)
        {
            if (oAdministrador == null)
                return BadRequest("Datos inválidos.");

            bool resultado = AdministradorData.InsertarAdministrador(oAdministrador);
            if (resultado)
                return Ok("Administrador registrado exitosamente.");
            else
                return BadRequest("No se pudo registrar el administrador.");
        }

        // PUT api/administrador
        [HttpPut]
        public IHttpActionResult Put([FromBody] Administrador oAdministrador)
        {
            if (oAdministrador == null)
                return BadRequest("Datos inválidos.");

            bool resultado = AdministradorData.ActualizarAdministrador(oAdministrador);
            if (resultado)
                return Ok("Administrador actualizado exitosamente.");
            else
                return BadRequest("No se pudo actualizar el administrador.");
        }

        // DELETE api/administrador/1001234567
        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            bool resultado = AdministradorData.EliminarAdministrador(id);
            if (resultado)
                return Ok("Administrador eliminado exitosamente.");
            else
                return BadRequest("No se pudo eliminar el administrador.");
        }
    }
}