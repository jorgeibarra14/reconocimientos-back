using System;

namespace Reconocimientos.Models
{
    public class ProductosPedido
    {
        public int id { get; set; }
        public int id_pedido { get; set; }
        public int producto_id { get; set; }
        public string producto_nombre { get; set; }
        public int producto_costo { get; set; }
        public string producto_imagen { get; set; }
        public int cantidad { get; set; }
        public bool activo { get; set; }
        public DateTime fecha_creacion{ get; set; }

    }
}