using System;
using System.IO;
using System.Net;
using System.Text;

namespace NewLife.Helpers
{
    public static class EmailHelper
    {
        private static string ApiKey   => Environment.GetEnvironmentVariable("BREVO_API_KEY")   ?? "";
        private static string FromAddr => Environment.GetEnvironmentVariable("EMAIL_FROM")       ?? "";
        private static string FromName => Environment.GetEnvironmentVariable("EMAIL_FROM_NAME")  ?? "NEW LIFE";

        public static void Enviar(string destinatario, string asunto, string cuerpoHtml)
        {
            string html = cuerpoHtml
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\r\n", " ")
                .Replace("\r", " ")
                .Replace("\n", " ");

            string payload = "{" +
                "\"sender\":{\"email\":\"" + FromAddr + "\",\"name\":\"" + FromName + "\"}," +
                "\"to\":[{\"email\":\"" + destinatario + "\"}]," +
                "\"subject\":\"" + asunto + "\"," +
                "\"htmlContent\":\"" + html + "\"" +
            "}";

            var request = (HttpWebRequest)WebRequest.Create("https://api.brevo.com/v3/smtp/email");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers["api-key"] = ApiKey;
            request.Timeout = 20000;

            byte[] data = Encoding.UTF8.GetBytes(payload);
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
                stream.Write(data, 0, data.Length);

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string resp = reader.ReadToEnd();
                if ((int)response.StatusCode >= 400)
                    throw new Exception("Brevo error: " + resp);
            }
        }

        private static string PlantillaBase(string titulo, string subtitulo, string contenido)
        {
            return "<!DOCTYPE html><html lang='es'><head><meta charset='UTF-8'></head>" +
                "<body style='margin:0;padding:0;background:#f0fdf4;font-family:Arial,sans-serif;'>" +
                "<table width='100%' cellpadding='0' cellspacing='0' style='background:#f0fdf4;padding:40px 20px;'>" +
                "<tr><td align='center'>" +
                "<table width='560' cellpadding='0' cellspacing='0' style='background:#fff;border-radius:16px;overflow:hidden;box-shadow:0 4px 24px rgba(0,0,0,0.08);'>" +
                "<tr><td style='background:linear-gradient(135deg,#28a745,#20c997);padding:32px;text-align:center;'>" +
                "<div style='font-size:2rem;font-weight:900;color:#fff;letter-spacing:2px;'>NEW LIFE</div>" +
                "<div style='color:rgba(255,255,255,0.85);font-size:0.9rem;margin-top:4px;'>Productos Biodegradables</div>" +
                "</td></tr>" +
                "<tr><td style='padding:36px 40px;'>" +
                "<h2 style='margin:0 0 8px;color:#0f172a;font-size:1.4rem;'>" + titulo + "</h2>" +
                "<p style='margin:0 0 24px;color:#64748b;font-size:0.95rem;'>" + subtitulo + "</p>" +
                contenido +
                "</td></tr>" +
                "<tr><td style='background:#f8fafc;padding:20px 40px;text-align:center;border-top:1px solid #e2e8f0;'>" +
                "<p style='margin:0;color:#94a3b8;font-size:0.8rem;'>Correo automatico de NEW LIFE. Si no realizaste esta accion, ignoralo.</p>" +
                "</td></tr></table></td></tr></table></body></html>";
        }

        public static void EnviarCodigoVerificacion(string correo, string codigo, string nombres)
        {
            string contenido =
                "<p style='color:#374151;font-size:1rem;margin:0 0 20px;'>Hola <strong>" + nombres + "</strong>," +
                " gracias por registrarte en NEW LIFE. Para completar tu registro ingresa este codigo:</p>" +
                "<div style='text-align:center;margin:28px 0;'>" +
                "<div style='display:inline-block;background:#f0fdf4;border:2px dashed #28a745;border-radius:12px;padding:20px 40px;'>" +
                "<div style='font-size:2.5rem;font-weight:900;color:#16a34a;letter-spacing:10px;'>" + codigo + "</div>" +
                "<div style='font-size:0.8rem;color:#64748b;margin-top:6px;'>Valido por <strong>15 minutos</strong></div>" +
                "</div></div>" +
                "<p style='color:#64748b;font-size:0.88rem;margin:0;'>Si no solicitaste esto, ignora este correo.</p>";

            Enviar(correo, "Codigo de verificacion - NEW LIFE",
                PlantillaBase("Verifica tu cuenta", "Completa tu registro con el codigo a continuacion", contenido));
        }

        public static void EnviarCodigoRecuperacion(string correo, string codigo, string nombres)
        {
            string contenido =
                "<p style='color:#374151;font-size:1rem;margin:0 0 20px;'>Hola <strong>" + nombres + "</strong>," +
                " recibimos una solicitud para restablecer tu contrasena. Usa este codigo:</p>" +
                "<div style='text-align:center;margin:28px 0;'>" +
                "<div style='display:inline-block;background:#fff7ed;border:2px dashed #f59e0b;border-radius:12px;padding:20px 40px;'>" +
                "<div style='font-size:2.5rem;font-weight:900;color:#d97706;letter-spacing:10px;'>" + codigo + "</div>" +
                "<div style='font-size:0.8rem;color:#64748b;margin-top:6px;'>Valido por <strong>15 minutos</strong></div>" +
                "</div></div>" +
                "<p style='color:#64748b;font-size:0.88rem;margin:0;'>Si no solicitaste esto, tu cuenta esta segura.</p>";

            Enviar(correo, "Restablecer contrasena - NEW LIFE",
                PlantillaBase("Restablecer contrasena", "Usa el codigo para crear una nueva contrasena", contenido));
        }
    }
}
