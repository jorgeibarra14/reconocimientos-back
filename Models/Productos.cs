using System;

namespace Reconocimientos.Models
{
    public class Productos
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int costo { get; set; }
        public int stock { get; set; }
        public string imagen { get; set; }
        public int categoria_id { get; set; }
        public bool activo { get; set; }
        public DateTime fecha_creacion { get; set; }
        public Categorias categoria { get; set; }
    }
}