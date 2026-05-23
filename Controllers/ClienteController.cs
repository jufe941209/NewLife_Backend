using NewLife.Data;
using NewLife.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace NewLife.Controllers
{
    public class LoginClienteRequest
    {
        public string correo { get; set; }
        public string contrasena { get; set; }
    }

    public class ClienteController : ApiController
    {
        // POST api/cliente/login
        [HttpPost]
        [Route("api/cliente/login")]
        public IHttpActionResult Login([FromBody] LoginClienteRequest req)
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
        // GET api/cliente
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Cliente> lista = ClienteData.ListarClientes();
            return Ok(lista);
        }

        // GET api/cliente/1020304050
        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            Cliente oCliente = ClienteData.ConsultarCliente(id);
            if (oCliente != null)
                return Ok(oCliente);
            else
                return NotFound();
        }

        // POST api/cliente
        [HttpPost]
        public IHttpActionResult Post([FromBody] Cliente oCliente)
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
        public IHttpActionResult Put([FromBody] Cliente oCliente)
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
        [HttpDelete]
        public IHttpActionResult Delete(string id)
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