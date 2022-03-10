using System;

namespace Reconocimientos.Models
{
    public class EstatusPedido
    {
        public int id { get; set; }
        public int id_pedido { get; set; }
        public string estado { get; set; }
        public bool activo { get; set; }
        public DateTime fecha_creacion{ get; set; }
    }
}