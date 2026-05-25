using NewLife.Data;
using NewLife.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace NewLife.Controllers
{
    [RoutePrefix("api/domiciliario")]
    public class DomiciliarioController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var lista = DomiciliarioData.ListarDomiciliarios();
            lista.ForEach(d => d.contrasena = null);
            return Ok(lista);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var oDomiciliario = DomiciliarioData.ConsultarDomiciliario(id);
            if (oDomiciliario == null) return NotFound();
            oDomiciliario.contrasena = null;
            return Ok(oDomiciliario);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] Domiciliario oDomiciliario)
        {
            if (oDomiciliario == null) return BadRequest("Datos inválidos.");
            bool resultado = DomiciliarioData.InsertarDomiciliario(oDomiciliario);
            if (resultado) return Ok("Domiciliario registrado exitosamente.");
            return BadRequest(DomiciliarioData.ultimoError);
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(string id, [FromBody] Domiciliario oDomiciliario)
        {
            if (oDomiciliario == null) return BadRequest("Datos inválidos.");
            oDomiciliario.cedula_domi = id;
            bool resultado = DomiciliarioData.ActualizarDomiciliario(oDomiciliario);
            if (resultado) return Ok("Domiciliario actualizado exitosamente.");
            return BadRequest(DomiciliarioData.ultimoError);
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            bool resultado = DomiciliarioData.EliminarDomiciliario(id);
            if (resultado) return Ok("Domiciliario eliminado exitosamente.");
            return BadRequest(DomiciliarioData.ultimoError);
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] LoginRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.cedula) || string.IsNullOrEmpty(req.contrasena))
                return BadRequest("Cédula y contraseña son requeridas.");
            var domi = DomiciliarioData.LoginDomiciliario(req.cedula, req.contrasena);
            if (domi == null) return Unauthorized();
            domi.contrasena = null;
            return Ok(domi);
        }

        [HttpPost]
        [Route("cleanup")]
        public IHttpActionResult Cleanup()
        {
            string resultado = DomiciliarioData.LimpiarDomiciliariosPrueba();
            return Ok(resultado);
        }

        public class LoginRequest
        {
            public string cedula { get; set; }
            public string contrasena { get; set; }
        }
    }
}
