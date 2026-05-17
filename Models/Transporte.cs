using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewLife.Models
{
    public class Transporte
    {
        public string cedula_domi { get; set; }    // nvarchar(20) -> string, no long
        public string placa { get; set; }           // varchar(10) ✅
        public string tipo { get; set; }            // varchar(50) -> nombre correcto es 'tipo' no 'tipo_vehiculo'
        public string descripcion { get; set; }     // varchar(255) nullable ✅
        public string estado { get; set; }          // varchar(20) -> nombre correcto es 'estado' no 'estado_vehiculo'
    }
}