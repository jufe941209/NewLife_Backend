using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewLife.Models
{
    public class DetalleFactura
    {
        public int num_f_codigo { get; set; }              // int ✅
        public int cantidad { get; set; }                  // int ✅
        public decimal? descuento_porcentaje { get; set; } // decimal nullable -> decimal?
        public string numero_factura { get; set; }         // varchar(20) ✅
        public string codigo_prod { get; set; }            // varchar(20) ✅
    }
}
