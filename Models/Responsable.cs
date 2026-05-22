using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewLife.Models
{
    public class Responsable
    {
        public string cedula_resp { get; set; }
        public string nombres { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string contrasena { get; set; }
        public DateTime fecha_registro { get; set; }
        public string estado { get; set; }
    }
}