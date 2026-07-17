using Microsoft.AspNetCore.Mvc;
using NewLife.Data;
using NewLife.Models;

namespace NewLife.Controllers
{
    [ApiController]
    [Route("api/domiciliario")]
    public class DomiciliarioController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var lista = DomiciliarioData.ListarDomiciliarios();
            lista.ForEach(d => d.contrasena = null);
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var oDomiciliario = DomiciliarioData.ConsultarDomiciliario(id);
            if (oDomiciliario == null) return NotFound();
            oDomiciliario.contrasena = null;
            return Ok(oDomiciliario);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Domiciliario oDomiciliario)
        {
            if (oDomiciliario == null) return BadRequest("Datos inválidos.");
            bool resultado = DomiciliarioData.InsertarDomiciliario(oDomiciliario);
            if (resultado) return Ok("Domiciliario registrado exitosamente.");
            return BadRequest(DomiciliarioData.ultimoError);
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Domiciliario oDomiciliario)
        {
            if (oDomiciliario == null) return BadRequest("Datos inválidos.");
            oDomiciliario.cedula_domi = id;
            bool resultado = DomiciliarioData.ActualizarDomiciliario(oDomiciliario);
            if (resultado) return Ok("Domiciliario actualizado exitosamente.");
            return BadRequest(DomiciliarioData.ultimoError);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            bool resultado = DomiciliarioData.EliminarDomiciliario(id);
            if (resultado) return Ok("Domiciliario eliminado exitosamente.");
            return BadRequest(DomiciliarioData.ultimoError);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginRequest req)
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
        public IActionResult Cleanup()
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
