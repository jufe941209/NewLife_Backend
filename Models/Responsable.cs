using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewLife.Models
{
    public class Responsable
    {
        public string cedula_resp { get; set; }       // nvarchar(20) ✅
        public string nombres { get; set; }            // varchar(100) ✅
        public string telefono { get; set; }           // varchar(20) -> string, no long
        public string correo { get; set; }             // varchar(100) ✅
        public DateTime fecha_registro { get; set; }   // date ✅
        public string estado { get; set; }             // varchar(10) ✅
    }
}