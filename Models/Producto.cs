using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewLife.Models
{
    public class Producto
    {
        public string codigo_prod { get; set; }                    // varchar(20) ✅
        public string nombres { get; set; }                        // varchar(150) ✅
        public string descripcion { get; set; }                    // varchar(255) nullable ✅
        public int stock_min { get; set; }                         // int ✅
        public int stock_real { get; set; }                        // int — inventario actual
        public string img_url { get; set; }                        // varchar(255) nullable ✅
        public DateTime fecha_registro { get; set; }               // date ✅
        public DateTime? fecha_ultima_modificacion { get; set; }   // datetime nullable -> DateTime?
        public decimal precio { get; set; }                        // decimal -> no double
        public string estado { get; set; }                         // varchar(10) ✅
        public string capacidad { get; set; }                      // varchar(20) nullable ✅
        public string temperatura_uso { get; set; }                // varchar(10) nullable ✅
        public int numero_categoria { get; set; }                  // int ✅
        public int id_tipo_producto { get; set; }

    }
}