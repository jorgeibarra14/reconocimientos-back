using System;

namespace Reconocimientos.Models
{
    public class Competencias
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string nivel { get; set; }
        public string img { get; set; }
        public bool activo { get; set; }
        public DateTime fecha_registro { get; set; }
    }
}