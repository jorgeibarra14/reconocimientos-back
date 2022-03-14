using System;

namespace Reconocimientos.Models
{
    public class Puestos
    {
        public int id { get; set; }
        public int puestoId { get; set; }
        public string nombre { get; set; }
        public string nivel { get; set; }
        public int puntos { get; set; }
    }
}