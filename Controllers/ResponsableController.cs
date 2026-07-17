using Microsoft.AspNetCore.Mvc;
using NewLife.Data;
using NewLife.Models;

namespace NewLife.Controllers
{
    [ApiController]
    [Route("api/responsable")]
    public class ResponsableController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var lista = ResponsableData.ListarResponsables();
            lista.ForEach(r => r.contrasena = null);
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var oResponsable = ResponsableData.ConsultarResponsable(id);
            if (oResponsable == null) return NotFound();
            oResponsable.contrasena = null;
            return Ok(oResponsable);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Responsable oResponsable)
        {
            if (oResponsable == null) return BadRequest("Datos inválidos.");
            bool resultado = ResponsableData.InsertarResponsable(oResponsable);
            if (resultado) return Ok("Responsable registrado exitosamente.");
            return BadRequest(ResponsableData.ultimoError);
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Responsable oResponsable)
        {
            if (oResponsable == null) return BadRequest("Datos inválidos.");
            oResponsable.cedula_resp = id;
            bool resultado = ResponsableData.ActualizarResponsable(oResponsable);
            if (resultado) return Ok("Responsable actualizado exitosamente.");
            return BadRequest(ResponsableData.ultimoError);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            bool resultado = ResponsableData.EliminarResponsable(id);
            if (resultado) return Ok("Responsable eliminado exitosamente.");
            return BadRequest(ResponsableData.ultimoError);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginRequest req)
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
