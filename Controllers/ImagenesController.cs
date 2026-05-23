using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace NewLife.Controllers
{
    public class ImagenesController : ApiController
    {
        private static readonly string[] EXTENSIONES_PERMITIDAS = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        private const long TAMANO_MAXIMO = 5 * 1024 * 1024; // 5 MB

        [HttpPost]
        [Route("api/imagenes/producto")]
        public async Task<IHttpActionResult> SubirImagen()
        {
            if (!Request.Content.IsMimeMultipartContent())
                return BadRequest("El contenido debe ser multipart/form-data.");

            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (var content in provider.Contents)
                {
                    var nombreOriginal = content.Headers.ContentDisposition?.FileName?.Trim('"') ?? "imagen";
                    var extension = Path.GetExtension(nombreOriginal).ToLower();

                    if (!EXTENSIONES_PERMITIDAS.Contains(extension))
                        return BadRequest("Solo se permiten imágenes JPG, PNG, WebP o GIF.");

                    var bytes = await content.ReadAsByteArrayAsync();

                    if (bytes.Length > TAMANO_MAXIMO)
                        return BadRequest("La imagen no puede superar 5 MB.");

                    if (bytes.Length == 0)
                        return BadRequest("El archivo está vacío.");

                    // Carpeta de destino en wwwroot
                    var carpeta = HttpContext.Current.Server.MapPath("~/Uploads/productos");
                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    // Nombre único para evitar colisiones
                    var nombreArchivo = Guid.NewGuid().ToString("N") + extension;
                    var rutaCompleta = Path.Combine(carpeta, nombreArchivo);
                    File.WriteAllBytes(rutaCompleta, bytes);

                    // URL pública del archivo
                    var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                    var url = $"{baseUrl}/Uploads/productos/{nombreArchivo}";

                    return Ok(new { url, nombre = nombreArchivo });
                }

                return BadRequest("No se recibió ningún archivo.");
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al guardar la imagen: " + ex.Message));
            }
        }

        [HttpDelete]
        [Route("api/imagenes/producto/{nombre}")]
        public IHttpActionResult EliminarImagen(string nombre)
        {
            try
            {
                // Solo permitir nombres de archivo simples (sin rutas)
                if (nombre.Contains('/') || nombre.Contains('\\') || nombre.Contains(".."))
                    return BadRequest("Nombre de archivo inválido.");

                var carpeta = HttpContext.Current.Server.MapPath("~/Uploads/productos");
                var ruta = Path.Combine(carpeta, nombre);

                if (File.Exists(ruta))
                    File.Delete(ruta);

                return Ok("Imagen eliminada.");
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al eliminar: " + ex.Message));
            }
        }
    }
}
