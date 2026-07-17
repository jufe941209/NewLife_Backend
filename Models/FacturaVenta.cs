using System;
using System.Collections.Generic;
using System.Linq;

namespace NewLife.Models
{
    public class FacturaVenta
    {
        public string numero_factura { get; set; }    // varchar(20) ✅
        public DateTime fecha { get; set; }            // date ✅
        public string metodo_pago { get; set; }        // varchar(50) ✅
        public string estado_pago { get; set; }        // varchar(15) ✅
        public string direccion_envio { get; set; }    // varchar(255) ✅
        public string cedula_cli { get; set; }         // nvarchar(20) 
    }
}