using Microsoft.AspNetCore.Mvc;
using NewLife.Data;
using NewLife.Helpers;

namespace NewLife.Controllers
{
    public class EnviarCodigoRequest
    {
        public string correo { get; set; }
        public string nombres { get; set; }
        public string tipo { get; set; } // "registro" | "recuperacion"
    }

    public class ConfirmarCodigoRequest
    {
        public string correo { get; set; }
        public string codigo { get; set; }
        public string tipo { get; set; }
    }

    public class RecuperarPasswordRequest
    {
        public string correo { get; set; }
        public string codigo { get; set; }
        public string nuevaContrasena { get; set; }
        public string rol { get; set; } // "cliente" | "responsable"
    }

    [ApiController]
    public class VerificacionController : ControllerBase
    {
        // POST api/Verificacion/Enviar
        [HttpPost]
        [Route("api/Verificacion/Enviar")]
        public IActionResult Enviar([FromBody] EnviarCodigoRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.correo) || string.IsNullOrEmpty(req.tipo))
                return BadRequest("Datos incompletos.");

            if (req.tipo == "registro" && ClienteData.ExisteCorreo(req.correo))
                return Ok(new { success = false, message = "Ya existe una cuenta activa con ese correo." });

            string codigo = new Random().Next(100000, 999999).ToString();
            bool guardado = VerificacionData.GuardarCodigo(req.correo, codigo, req.tipo);
            if (!guardado)
                return StatusCode(500, "No se pudo guardar el código.");

            try
            {
                string nombres = string.IsNullOrEmpty(req.nombres) ? "usuario" : req.nombres;
                if (req.tipo == "registro")
                    EmailHelper.EnviarCodigoVerificacion(req.correo, codigo, nombres);
                else
                    EmailHelper.EnviarCodigoRecuperacion(req.correo, codigo, nombres);

                return Ok(new { success = true, message = "Código enviado al correo." });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = "No se pudo enviar el correo: " + ex.Message });
            }
        }

        // POST api/Verificacion/Confirmar
        [HttpPost]
        [Route("api/Verificacion/Confirmar")]
        public IActionResult Confirmar([FromBody] ConfirmarCodigoRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.correo) || string.IsNullOrEmpty(req.codigo))
                return BadRequest("Datos incompletos.");

            bool valido = VerificacionData.VerificarCodigo(req.correo, req.codigo, req.tipo ?? "registro");
            if (valido)
                return Ok(new { success = true, message = "Código verificado correctamente." });
            else
                return Ok(new { success = false, message = "Código incorrecto o expirado." });
        }

        // POST api/Verificacion/RecuperarPassword
        [HttpPost]
        [Route("api/Verificacion/RecuperarPassword")]
        public IActionResult RecuperarPassword([FromBody] RecuperarPasswordRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.correo) || string.IsNullOrEmpty(req.codigo)
                || string.IsNullOrEmpty(req.nuevaContrasena))
                return BadRequest("Datos incompletos.");

            bool valido = VerificacionData.VerificarCodigo(req.correo, req.codigo, "recuperacion");
            if (!valido)
                return Ok(new { success = false, message = "Código incorrecto o expirado." });

            bool cambiado = false;
            string rol = req.rol ?? "cliente";
            if (rol == "responsable")
                cambiado = ResponsableData.CambiarContrasena(req.correo, req.nuevaContrasena);
            else
                cambiado = ClienteData.CambiarContrasena(req.correo, req.nuevaContrasena);

            if (cambiado)
                return Ok(new { success = true, message = "Contraseña actualizada correctamente." });
            else
                return Ok(new { success = false, message = "No se encontró la cuenta o no se pudo actualizar." });
        }
    }
}
