using NewLife.Data;
using NewLife.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace NewLife.Controllers
{
    public class TipoProductoController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<TipoProducto> lista = TipoProductoData.ListarTiposProducto();
            if (lista.Count > 0)
                return Ok(lista);
            else
                return NotFound();
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            TipoProducto oTipoProducto = TipoProductoData.ConsultarTipoProducto(id);
            if (oTipoProducto != null)
                return Ok(oTipoProducto);
            else
                return NotFound();
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] TipoProducto oTipoProducto)
        {
            if (oTipoProducto == null)
                return BadRequest("Datos inválidos.");

            bool resultado = TipoProductoData.InsertarTipoProducto(oTipoProducto);
            if (resultado)
                return Ok("Tipo de producto registrado exitosamente.");
            else
                return BadRequest(TipoProductoData.ultimoError);
        }

        [HttpPut]
        public IHttpActionResult Put([FromBody] TipoProducto oTipoProducto)
        {
            if (oTipoProducto == null)
                return BadRequest("Datos inválidos.");

            bool resultado = TipoProductoData.ActualizarTipoProducto(oTipoProducto);
            if (resultado)
                return Ok("Tipo de producto actualizado exitosamente.");
            else
                return BadRequest(TipoProductoData.ultimoError);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            bool resultado = TipoProductoData.EliminarTipoProducto(id);
            if (resultado)
                return Ok("Tipo de producto eliminado exitosamente.");
            else
                return BadRequest(TipoProductoData.ultimoError);
        }
    }
}