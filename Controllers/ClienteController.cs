using NewLife.Data;
using NewLife.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace NewLife.Controllers
{
    public class ClienteController : ApiController
    {
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