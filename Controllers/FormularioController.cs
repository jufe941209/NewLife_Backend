using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;

namespace NewLife.Controllers
{
    [ApiController]
    public class FormularioController : ControllerBase
    {
        [HttpPost]
        [Route("api/formulario")]
        public IActionResult Post([FromBody] FormularioRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.nombre) ||
                string.IsNullOrWhiteSpace(request.correo) ||
                string.IsNullOrWhiteSpace(request.asunto) ||
                string.IsNullOrWhiteSpace(request.mensaje))
                return BadRequest("Faltan campos requeridos.");

            try
            {
                string host     = Environment.GetEnvironmentVariable("SMTP_HOST")      ?? "smtp.gmail.com";
                int    port     = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587");
                string user     = Environment.GetEnvironmentVariable("SMTP_USER")      ?? "";
                string pass     = Environment.GetEnvironmentVariable("SMTP_PASS")      ?? "";
                string fromAddr = Environment.GetEnvironmentVariable("SMTP_FROM")      ?? user;
                string fromName = Environment.GetEnvironmentVariable("SMTP_FROM_NAME") ?? "NEW LIFE";

                string cuerpo =
                    "<div style='font-family:Arial,sans-serif;max-width:600px;margin:auto;'>" +
                    "<div style='background:linear-gradient(135deg,#28a745,#20c997);padding:28px;text-align:center;border-radius:12px 12px 0 0;'>" +
                    "<h1 style='color:#fff;margin:0;font-size:1.6rem;'>NEW LIFE</h1>" +
                    "<p style='color:rgba(255,255,255,0.85);margin:4px 0 0;font-size:0.9rem;'>Nuevo mensaje desde el formulario de contacto</p>" +
                    "</div>" +
                    "<div style='background:#fff;padding:32px;border:1px solid #e9ecef;border-top:none;border-radius:0 0 12px 12px;'>" +
                    "<table style='width:100%;border-collapse:collapse;'>" +
                    "<tr><td style='padding:10px 0;border-bottom:1px solid #f1f1f1;color:#6c757d;width:120px;font-weight:600;'>Nombre</td>" +
                    "<td style='padding:10px 0;border-bottom:1px solid #f1f1f1;color:#212529;'>" + WebUtility.HtmlEncode(request.nombre) + "</td></tr>" +
                    "<tr><td style='padding:10px 0;border-bottom:1px solid #f1f1f1;color:#6c757d;font-weight:600;'>Correo</td>" +
                    "<td style='padding:10px 0;border-bottom:1px solid #f1f1f1;color:#212529;'><a href='mailto:" + request.correo + "' style='color:#28a745;'>" + WebUtility.HtmlEncode(request.correo) + "</a></td></tr>" +
                    "<tr><td style='padding:10px 0;border-bottom:1px solid #f1f1f1;color:#6c757d;font-weight:600;'>Teléfono</td>" +
                    "<td style='padding:10px 0;border-bottom:1px solid #f1f1f1;color:#212529;'>" + WebUtility.HtmlEncode(request.telefono ?? "-") + "</td></tr>" +
                    "<tr><td style='padding:10px 0;border-bottom:1px solid #f1f1f1;color:#6c757d;font-weight:600;'>Asunto</td>" +
                    "<td style='padding:10px 0;border-bottom:1px solid #f1f1f1;color:#212529;'><strong>" + WebUtility.HtmlEncode(request.asunto) + "</strong></td></tr>" +
                    "</table>" +
                    "<div style='margin-top:20px;'><p style='color:#6c757d;font-weight:600;margin-bottom:8px;'>Mensaje:</p>" +
                    "<div style='background:#f8f9fa;border-left:4px solid #28a745;padding:16px;border-radius:0 8px 8px 0;color:#212529;line-height:1.6;'>" +
                    WebUtility.HtmlEncode(request.mensaje).Replace("\n", "<br/>") +
                    "</div></div>" +
                    "</div></div>";

                using (var smtp = new SmtpClient(host, port))
                {
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(user, pass);
                    smtp.Timeout = 20000;

                    var mail = new MailMessage();
                    mail.From = new MailAddress(fromAddr, fromName);
                    mail.To.Add(fromAddr);
                    mail.ReplyToList.Add(new MailAddress(request.correo, request.nombre));
                    mail.Subject = "[Mensaje NEW LIFE] " + request.asunto;
                    mail.Body = cuerpo;
                    mail.IsBodyHtml = true;

                    smtp.Send(mail);
                }

                return Ok(new { mensaje = "Mensaje enviado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "No se pudo enviar el mensaje: " + ex.Message);
            }
        }
    }

    public class FormularioRequest
    {
        public string nombre   { get; set; }
        public string correo   { get; set; }
        public string telefono { get; set; }
        public string asunto   { get; set; }
        public string mensaje  { get; set; }
    }
}
