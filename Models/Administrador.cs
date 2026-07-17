using System;
using System.Collections.Generic;
using System.Linq;

namespace NewLife.Models
{
    public class Administrador
    {
        public string cedula_adm { get; set; }
        public string correo { get; set; }
        public string contrasena { get; set; }
        public string nombres { get; set; }
        public DateTime fecha_registro { get; set; }
        public string estado { get; set; } // 'Activo' o 'Inactivo'

    }
}