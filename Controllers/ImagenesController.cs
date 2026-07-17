using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace NewLife.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagenesController : ControllerBase
    {
        private static readonly string[] EXTENSIONES_PERMITIDAS = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        private const long TAMANO_MAXIMO = 5 * 1024 * 1024; // 5 MB

        private readonly IWebHostEnvironment _entorno;

        public ImagenesController(IWebHostEnvironment entorno)
        {
            _entorno = entorno;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            if (Request.Form.Files.Count == 0)
                return BadRequest("No se recibió ningún archivo.");

            try
            {
                var archivo = Request.Form.Files[0];
                var nombreOriginal = archivo.FileName ?? "imagen";
                var extension = Path.GetExtension(nombreOriginal).ToLower();

                if (!EXTENSIONES_PERMITIDAS.Contains(extension))
                    return BadRequest("Solo se permiten imágenes JPG, PNG, WebP o GIF.");

                if (archivo.Length > TAMANO_MAXIMO)
                    return BadRequest("La imagen no puede superar 5 MB.");

                if (archivo.Length == 0)
                    return BadRequest("El archivo está vacío.");

                // Carpeta de destino (servida por Program.cs en /Uploads/productos)
                var carpeta = Path.Combine(_entorno.ContentRootPath, "Uploads", "productos");
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                // Nombre único para evitar colisiones
                var nombreArchivo = Guid.NewGuid().ToString("N") + extension;
                var rutaCompleta = Path.Combine(carpeta, nombreArchivo);
                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    await archivo.CopyToAsync(stream);

                // URL pública del archivo
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var url = $"{baseUrl}/Uploads/productos/{nombreArchivo}";

                return Ok(new { url, nombre = nombreArchivo });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al guardar la imagen: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                // Solo permitir nombres de archivo simples (sin rutas)
                if (id.Contains('/') || id.Contains('\\') || id.Contains(".."))
                    return BadRequest("Nombre de archivo inválido.");

                var carpeta = Path.Combine(_entorno.ContentRootPath, "Uploads", "productos");
                var ruta = Path.Combine(carpeta, id);

                if (System.IO.File.Exists(ruta))
                    System.IO.File.Delete(ruta);

                return Ok("Imagen eliminada.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al eliminar: " + ex.Message);
            }
        }
    }
}
