using System;

namespace Reconocimientos.Models
{
    public class Puntos
    {
        public int id { get; set; }
        public string id_empleado { get; set; }
        public int puntos { get; set; }
        public string periodo { get; set; }
        public bool activo { get; set; }
        public DateTime fecha_registro { get; set; }
    }
}