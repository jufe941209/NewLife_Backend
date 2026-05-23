using NewLife.Data;
using NewLife.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace NewLife.Controllers
{
    public class AprobarDespachoRequest
    {
        public int numero_despacho { get; set; }
        public string cc_responsable { get; set; }
    }

    public class DespachoController : ApiController
    {
        // GET api/despacho
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Despacho> lista = DespachoData.ListarDespachos();
            return Ok(lista);
        }

        // GET api/despacho/disponibles — despachos sin responsable asignado
        [HttpGet]
        [Route("api/despacho/disponibles")]
        public IHttpActionResult GetDisponibles()
        {
            List<Despacho> lista = DespachoData.ListarDespachosDisponibles();
            return Ok(lista);
        }

        // GET api/despacho/1
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Despacho oDespacho = DespachoData.ConsultarDespacho(id);
            if (oDespacho != null)
                return Ok(oDespacho);
            else
                return NotFound();
        }

        // POST api/despacho
        [HttpPost]
        public IHttpActionResult Post([FromBody] Despacho oDespacho)
        {
            if (oDespacho == null)
                return BadRequest("Datos inválidos.");

            bool resultado = DespachoData.InsertarDespacho(oDespacho);
            if (resultado)
                return Ok("Despacho registrado exitosamente.");
            else
                return BadRequest(DespachoData.ultimoError);
        }

        // PUT api/despacho/aprobar — responsable aprueba y se asigna el despacho
        [HttpPut]
        [Route("api/despacho/aprobar")]
        public IHttpActionResult Aprobar([FromBody] AprobarDespachoRequest req)
        {
            if (req == null || req.numero_despacho == 0 || string.IsNullOrEmpty(req.cc_responsable))
                return BadRequest("Numero de despacho y cedula del responsable son requeridos.");

            Despacho oDespacho = DespachoData.ConsultarDespacho(req.numero_despacho);
            if (oDespacho == null)
                return NotFound();

            if (!string.IsNullOrEmpty(oDespacho.cc_responsable))
                return Ok(new { success = false, message = "Este despacho ya fue tomado por otro responsable." });

            oDespacho.cc_responsable = req.cc_responsable;
            oDespacho.estado = "Aprobado";
            oDespacho.fecha_aprobacion = DateTime.Now;

            bool resultado = DespachoData.ActualizarDespacho(oDespacho);
            if (resultado)
                return Ok(new { success = true, message = "Despacho aprobado y asignado correctamente." });
            else
                return BadRequest(DespachoData.ultimoError);
        }

        // PUT api/despacho
        [HttpPut]
        public IHttpActionResult Put([FromBody] Despacho oDespacho)
        {
            if (oDespacho == null)
                return BadRequest("Datos inválidos.");

            if ((oDespacho.estado == "En camino" || oDespacho.estado == "Entregado") &&
                string.IsNullOrEmpty(oDespacho.cc_domiciliario))
                return BadRequest("Un despacho en estado '" + oDespacho.estado + "' debe tener un domiciliario asignado.");

            bool resultado = DespachoData.ActualizarDespacho(oDespacho);
            if (resultado)
                return Ok("Despacho actualizado exitosamente.");
            else
                return BadRequest(DespachoData.ultimoError);
        }

        // DELETE api/despacho/1
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            bool resultado = DespachoData.EliminarDespacho(id);
            if (resultado)
                return Ok("Despacho eliminado exitosamente.");
            else
                return BadRequest(DespachoData.ultimoError);
        }
    }
}
