using System;

namespace Reconocimientos.Models
{
    public class Categorias
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public bool activo { get; set; }
        public DateTime fecha_creacion{ get; set; }
        public string img { get; set; }
    }
}