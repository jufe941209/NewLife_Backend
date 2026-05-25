using NewLife.Data;
using NewLife.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace NewLife.Controllers
{
    public class FirmaRequest
    {
        public string referencia { get; set; }
        public long montoCentavos { get; set; }
    }

    [RoutePrefix("api/pagos")]
    public class PagosController : ApiController
    {
        // POST /api/pagos/firma
        // Devuelve la firma de integridad Wompi para un pago dado
        [HttpPost]
        [Route("firma")]
        public IHttpActionResult ObtenerFirma([FromBody] FirmaRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.referencia) || req.montoCentavos <= 0)
                return BadRequest("Referencia y monto requeridos.");

            string integritySecret = ConfigurationManager.AppSettings["Wompi:IntegritySecret"];
            string llavePublica = ConfigurationManager.AppSettings["Wompi:PublicKey"];

            // Fórmula Wompi: SHA256(referencia + montoCentavos + "COP" + integritySecret)
            string cadena = $"{req.referencia}{req.montoCentavos}COP{integritySecret}";
            string firma = ComputarSHA256(cadena);

            return Ok(new { firma, llavePublica });
        }

        // POST /api/pagos/webhook
        // Wompi llama a este endpoint cuando cambia el estado de una transacción
        [HttpPost]
        [Route("webhook")]
        public async Task<IHttpActionResult> Webhook()
        {
            string body = await Request.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(body)) return Ok();

            JObject data;
            try { data = JObject.Parse(body); }
            catch { return BadRequest("JSON inválido"); }

            string evento = data["event"]?.ToString();
            if (evento != "transaction.updated") return Ok();

            var tx = data["data"]?["transaction"];
            if (tx == null) return Ok();

            string txId = tx["id"]?.ToString() ?? "";
            string status = tx["status"]?.ToString() ?? "";
            string montoCentavos = tx["amount_in_cents"]?.ToString() ?? "";
            string referencia = tx["reference"]?.ToString() ?? "";
            string timestamp = data["timestamp"]?.ToString() ?? "";
            string checksum = data["signature"]?["checksum"]?.ToString() ?? "";

            // Verificar firma del webhook
            string eventsSecret = ConfigurationManager.AppSettings["Wompi:EventsSecret"];
            string firmaEsperada = ComputarSHA256($"{txId}{status}{montoCentavos}{timestamp}{eventsSecret}");

            if (checksum != firmaEsperada)
                return BadRequest("Firma de webhook inválida.");

            // Mapear estado Wompi → estado interno
            string estadoPago;
            switch (status)
            {
                case "APPROVED": estadoPago = "Aprobado"; break;
                case "DECLINED": estadoPago = "Rechazado"; break;
                case "VOIDED":   estadoPago = "Anulado";   break;
                default:         estadoPago = "Pendiente"; break;
            }

            // Actualizar factura
            FacturaVenta factura = FacturaVentaData.ConsultarFacturaVenta(referencia);
            if (factura != null)
            {
                factura.estado_pago = estadoPago;
                FacturaVentaData.ActualizarFacturaVenta(factura);
            }

            return Ok();
        }

        private static string ComputarSHA256(string entrada)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(entrada));
                return string.Concat(bytes.Select(b => b.ToString("x2")));
            }
        }
    }
}
