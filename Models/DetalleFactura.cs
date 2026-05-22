using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewLife.Models
{
    public class DetalleFactura
    {
        public int num_f_codigo { get; set; }
        public int cantidad { get; set; }
        public decimal? descuento_porcentaje { get; set; }
        public string numero_factura { get; set; }
        public string codigo_prod { get; set; }
        public decimal precio_unitario { get; set; }
    }
}
