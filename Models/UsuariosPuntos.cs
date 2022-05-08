using System;

namespace Reconocimientos.Models
{
    public class UsuariosPuntos
    {
        public int Id { get; set; }

        public string IdEmpleado { get; set; }

        public int Valor { get; set; }

        public string Tipo { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int IdPedido { get; set; }

    }
}
