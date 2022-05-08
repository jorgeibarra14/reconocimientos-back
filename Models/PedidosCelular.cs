using System;

namespace Reconocimientos.Models
{
    public class PedidosCelular
    {
        public int Id { get; set; }

        public int IdPedido { get; set; }

        public int Celular { get; set; }

        public DateTime FechaCreacion { get; set; }

    }
}
