using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewLife.Models
{
    public class Despacho
    {
        public int numero_despacho { get; set; }           // int ✅
        public DateTime fecha_despacho { get; set; }       // date ✅
        public DateTime? fecha_aprobacion { get; set; }    // datetime nullable -> DateTime?
        public string estado { get; set; }                 // varchar(15) ✅
        public DateTime? fecha_entrega { get; set; }       // datetime nullable -> DateTime?
        public string numero_factura { get; set; }         // varchar(20) ✅
        public string cc_responsable { get; set; }         // nvarchar(20) -> string
        public string cc_domiciliario { get; set; }        // nvarchar(20) -> string
    }
}