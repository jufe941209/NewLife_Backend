using NewLife.Data;
using NewLife.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace NewLife.Controllers
{
    [RoutePrefix("api/responsable")]
    public class ResponsableController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var lista = ResponsableData.ListarResponsables();
            lista.ForEach(r => r.contrasena = null);
            return Ok(lista);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var oResponsable = ResponsableData.ConsultarResponsable(id);
            if (oResponsable == null) return NotFound();
            oResponsable.contrasena = null;
            return Ok(oResponsable);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] Responsable oResponsable)
        {
            if (oResponsable == null) return BadRequest("Datos inválidos.");
            bool resultado = ResponsableData.InsertarResponsable(oResponsable);
            if (resultado) return Ok("Responsable registrado exitosamente.");
            return BadRequest(ResponsableData.ultimoError);
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(string id, [FromBody] Responsable oResponsable)
        {
            if (oResponsable == null) return BadRequest("Datos inválidos.");
            oResponsable.cedula_resp = id;
            bool resultado = ResponsableData.ActualizarResponsable(oResponsable);
            if (resultado) return Ok("Responsable actualizado exitosamente.");
            return BadRequest(ResponsableData.ultimoError);
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            bool resultado = ResponsableData.EliminarResponsable(id);
            if (resultado) return Ok("Responsable eliminado exitosamente.");
            return BadRequest(ResponsableData.ultimoError);
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] LoginRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.correo) || string.IsNullOrEmpty(req.contrasena))
                return BadRequest("Correo y contraseña son requeridos.");
            var resp = ResponsableData.LoginResponsable(req.correo, req.contrasena);
            if (resp == null) return Unauthorized();
            resp.contrasena = null;
            return Ok(resp);
        }

        public class LoginRequest
        {
            public string correo { get; set; }
            public string contrasena { get; set; }
        }
    }
}
