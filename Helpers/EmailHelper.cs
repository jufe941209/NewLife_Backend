using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace NewLife.Helpers
{
    public class EmailHelper
    {
        private static string Host     => ConfigurationManager.AppSettings["SmtpHost"]     ?? "smtp.gmail.com";
        private static int    Port     => int.Parse(ConfigurationManager.AppSettings["SmtpPort"] ?? "587");
        private static string User     => ConfigurationManager.AppSettings["SmtpUser"]     ?? "";
        private static string Pass     => ConfigurationManager.AppSettings["SmtpPass"]     ?? "";
        private static string From     => ConfigurationManager.AppSettings["SmtpFrom"]     ?? "";
        private static string FromName => ConfigurationManager.AppSettings["SmtpFromName"] ?? "NEW LIFE";

        public static void Enviar(string destinatario, string asunto, string cuerpoHtml)
        {
            using (var smtp = new SmtpClient(Host, Port))
            {
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(User, Pass);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                var msg = new MailMessage
                {
                    From = new MailAddress(From, FromName),
                    Subject = asunto,
                    Body = cuerpoHtml,
                    IsBodyHtml = true
                };
                msg.To.Add(destinatario);
                smtp.Send(msg);
            }
        }

        private static string PlantillaBase(string titulo, string subtitulo, string contenido)
        {
            return $@"
<!DOCTYPE html>
<html lang='es'>
<head><meta charset='UTF-8'><meta name='viewport' content='width=device-width,initial-scale=1'>
<title>{titulo}</title></head>
<body style='margin:0;padding:0;background:#f0fdf4;font-family:Arial,sans-serif;'>
  <table width='100%' cellpadding='0' cellspacing='0' style='background:#f0fdf4;padding:40px 20px;'>
    <tr><td align='center'>
      <table width='560' cellpadding='0' cellspacing='0' style='background:#fff;border-radius:16px;overflow:hidden;box-shadow:0 4px 24px rgba(0,0,0,0.08);'>
        <!-- Header -->
        <tr>
          <td style='background:linear-gradient(135deg,#28a745,#20c997);padding:32px;text-align:center;'>
            <div style='font-size:2rem;font-weight:900;color:#fff;letter-spacing:2px;'>🌿 NEW LIFE</div>
            <div style='color:rgba(255,255,255,0.85);font-size:0.9rem;margin-top:4px;'>Productos Biodegradables</div>
          </td>
        </tr>
        <!-- Body -->
        <tr>
          <td style='padding:36px 40px;'>
            <h2 style='margin:0 0 8px;color:#0f172a;font-size:1.4rem;'>{titulo}</h2>
            <p style='margin:0 0 24px;color:#64748b;font-size:0.95rem;'>{subtitulo}</p>
            {contenido}
          </td>
        </tr>
        <!-- Footer -->
        <tr>
          <td style='background:#f8fafc;padding:20px 40px;text-align:center;border-top:1px solid #e2e8f0;'>
            <p style='margin:0;color:#94a3b8;font-size:0.8rem;'>
              Este correo fue enviado automáticamente por NEW LIFE.<br>
              Si no realizaste esta acción, ignora este mensaje.
            </p>
          </td>
        </tr>
      </table>
    </td></tr>
  </table>
</body></html>";
        }

        public static void EnviarCodigoVerificacion(string correo, string codigo, string nombres)
        {
            string contenido = $@"
<p style='color:#374151;font-size:1rem;margin:0 0 20px;'>
  Hola <strong>{nombres}</strong>, gracias por registrarte en NEW LIFE.<br>
  Para completar tu registro, ingresa el siguiente código de verificación:
</p>
<div style='text-align:center;margin:28px 0;'>
  <div style='display:inline-block;background:#f0fdf4;border:2px dashed #28a745;border-radius:12px;padding:20px 40px;'>
    <div style='font-size:2.5rem;font-weight:900;color:#16a34a;letter-spacing:10px;'>{codigo}</div>
    <div style='font-size:0.8rem;color:#64748b;margin-top:6px;'>Código válido por <strong>15 minutos</strong></div>
  </div>
</div>
<p style='color:#64748b;font-size:0.88rem;margin:0;'>
  Si no solicitaste este código, puedes ignorar este correo con total seguridad.
</p>";
            Enviar(correo, "✅ Código de verificación - NEW LIFE", PlantillaBase(
                "Verifica tu cuenta",
                "Completa tu registro con el código a continuación",
                contenido));
        }

        public static void EnviarCodigoRecuperacion(string correo, string codigo, string nombres)
        {
            string contenido = $@"
<p style='color:#374151;font-size:1rem;margin:0 0 20px;'>
  Hola <strong>{nombres}</strong>, recibimos una solicitud para restablecer tu contraseña.<br>
  Usa el siguiente código para continuar:
</p>
<div style='text-align:center;margin:28px 0;'>
  <div style='display:inline-block;background:#fff7ed;border:2px dashed #f59e0b;border-radius:12px;padding:20px 40px;'>
    <div style='font-size:2.5rem;font-weight:900;color:#d97706;letter-spacing:10px;'>{codigo}</div>
    <div style='font-size:0.8rem;color:#64748b;margin-top:6px;'>Código válido por <strong>15 minutos</strong></div>
  </div>
</div>
<p style='color:#64748b;font-size:0.88rem;margin:0;'>
  Si no solicitaste restablecer tu contraseña, ignora este correo. Tu cuenta está segura.
</p>";
            Enviar(correo, "🔐 Restablecer contraseña - NEW LIFE", PlantillaBase(
                "Restablecer contraseña",
                "Usa el código a continuación para crear una nueva contraseña",
                contenido));
        }
    }
}
