using NewLife.Data;
using NewLife.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace NewLife.Controllers
{
    [RoutePrefix("api/administrador")]
    public class AdministradorController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var lista = AdministradorData.ListarAdministradores();
            lista.ForEach(a => a.contrasena = null);
            return Ok(lista);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var oAdministrador = AdministradorData.ConsultarAdministrador(id);
            if (oAdministrador == null) return NotFound();
            oAdministrador.contrasena = null;
            return Ok(oAdministrador);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] Administrador oAdministrador)
        {
            if (oAdministrador == null) return BadRequest("Datos inválidos.");
            bool resultado = AdministradorData.InsertarAdministrador(oAdministrador);
            if (resultado) return Ok("Administrador registrado exitosamente.");
            return BadRequest(AdministradorData.ultimoError);
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(string id, [FromBody] Administrador oAdministrador)
        {
            if (oAdministrador == null) return BadRequest("Datos inválidos.");
            oAdministrador.cedula_adm = id;
            bool resultado = AdministradorData.ActualizarAdministrador(oAdministrador);
            if (resultado) return Ok("Administrador actualizado exitosamente.");
            return BadRequest(AdministradorData.ultimoError);
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            bool resultado = AdministradorData.EliminarAdministrador(id);
            if (resultado) return Ok("Administrador eliminado exitosamente.");
            return BadRequest(AdministradorData.ultimoError);
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] LoginRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.correo) || string.IsNullOrEmpty(req.contrasena))
                return BadRequest("Correo y contraseña son requeridos.");
            var admin = AdministradorData.LoginAdministrador(req.correo, req.contrasena);
            if (admin == null) return Unauthorized();
            admin.contrasena = null;
            return Ok(admin);
        }

        [HttpPost]
        [Route("cambiar-contrasena")]
        public IHttpActionResult CambiarContrasena([FromBody] CambiarContrasenaRequest req)
        {
            if (req == null) return BadRequest("Datos inválidos.");
            var admin = AdministradorData.LoginAdministrador(req.correo, req.contrasenaActual);
            if (admin == null) return BadRequest("La contraseña actual no es correcta.");
            bool ok = AdministradorData.ActualizarContrasena(admin.cedula_adm, NewLife.Helpers.HashHelper.Sha256(req.contrasenaNueva));
            if (ok) return Ok("Contraseña actualizada.");
            return BadRequest(AdministradorData.ultimoError);
        }

        public class LoginRequest
        {
            public string correo { get; set; }
            public string contrasena { get; set; }
        }

        public class CambiarContrasenaRequest
        {
            public string correo { get; set; }
            public string contrasenaActual { get; set; }
            public string contrasenaNueva { get; set; }
        }
    }
}
