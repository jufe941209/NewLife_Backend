using Microsoft.AspNetCore.Mvc;
using NewLife.Data;
using NewLife.Models;

namespace NewLife.Controllers
{
    public class LoginClienteRequest
    {
        public string correo { get; set; }
        public string contrasena { get; set; }
    }

    public class CambiarContrasenaClienteRequest
    {
        public string correo { get; set; }
        public string contrasenaActual { get; set; }
        public string contrasenaNueva { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        // POST api/cliente/login
        [HttpPost]
        [Route("/api/cliente/login")]
        public IActionResult Login([FromBody] LoginClienteRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.correo) || string.IsNullOrEmpty(req.contrasena))
                return BadRequest("Correo y contraseña requeridos.");
            var cliente = ClienteData.LoginCliente(req.correo, req.contrasena);
            if (cliente != null)
                return Ok(new { success = true, cliente });
            // Verificar si existe pero inactivo o contraseña incorrecta
            var todos = ClienteData.ListarClientes();
            var porCorreo = todos.Find(c => c.correo == req.correo);
            if (porCorreo == null)
                return Ok(new { success = false, message = "No existe una cuenta con ese correo." });
            if (porCorreo.estado == "Inactivo")
                return Ok(new { success = false, message = "Esta cuenta fue desactivada. Contacta al administrador." });
            return Ok(new { success = false, message = "Contraseña incorrecta." });
        }
        // POST api/cliente/cambiar-contrasena
        [HttpPost]
        [Route("/api/cliente/cambiar-contrasena")]
        public IActionResult CambiarContrasena([FromBody] CambiarContrasenaClienteRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.correo))
                return BadRequest("Datos inválidos.");
            var cliente = ClienteData.LoginCliente(req.correo, req.contrasenaActual);
            if (cliente == null)
                return BadRequest("La contraseña actual no es correcta.");
            bool ok = ClienteData.CambiarContrasena(req.correo, req.contrasenaNueva);
            if (ok) return Ok("Contraseña actualizada.");
            return BadRequest(ClienteData.ultimoError);
        }

        // GET api/cliente
        [HttpGet]
        public IActionResult Get()
        {
            List<Cliente> lista = ClienteData.ListarClientes();
            return Ok(lista);
        }

        // GET api/cliente/1020304050
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            Cliente oCliente = ClienteData.ConsultarCliente(id);
            if (oCliente != null)
                return Ok(oCliente);
            else
                return NotFound();
        }

        // POST api/cliente
        [HttpPost]
        public IActionResult Post([FromBody] Cliente oCliente)
        {
            if (oCliente == null)
                return BadRequest("Datos inválidos.");

            bool resultado = ClienteData.InsertarCliente(oCliente);
            if (resultado)
                return Ok("Cliente registrado exitosamente.");
            else
                return BadRequest(ClienteData.ultimoError);
        }
        // PUT api/cliente
        [HttpPut]
        public IActionResult Put([FromBody] Cliente oCliente)
        {
            if (oCliente == null)
                return BadRequest("Datos inválidos.");

            bool resultado = ClienteData.ActualizarCliente(oCliente);
            if (resultado)
                return Ok("Cliente actualizado exitosamente.");
            else
                return BadRequest("No se pudo actualizar el cliente.");
        }

        // DELETE api/cliente/1020304050
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            bool resultado = ClienteData.EliminarCliente(id);
            if (resultado)
                return Ok("Cliente eliminado exitosamente.");
            else
                return BadRequest(string.IsNullOrEmpty(ClienteData.ultimoError)
                    ? "No se pudo eliminar el cliente. Puede tener facturas u otros registros asociados."
                    : ClienteData.ultimoError);
        }
    }
}
