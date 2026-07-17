using System;
using System.Collections.Generic;
using System.Linq;

namespace NewLife.Models
{
    public class Domiciliario
    {
        public string cedula_domi { get; set; }      // nvarchar(20) -> string, no long
        public string nombres { get; set; }           // varchar(100) ✅
        public string telefono { get; set; }          // varchar(20) -> string, no long
        public DateTime fecha_registro { get; set; }  // date ✅
        public string disponibilidad { get; set; }    // varchar(15) ✅
        public string estado { get; set; }            // varchar(10) ✅
        public string contrasena { get; set; }        // varchar(100)
    }
}