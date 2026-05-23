using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewLife.Models
{
    public class Cliente
    {
        public string numero_identificacion { get; set; }
        public string nombres { get; set; }
        public string tipo_documento { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string contrasena { get; set; }

        public DateTime fecha_registro { get; set; }
        public string tipo_cliente { get; set; }
        public string estado { get; set; }
        public string cedula_adm { get; set; }
    }
}