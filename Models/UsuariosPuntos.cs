using System;

namespace Reconocimientos.Models
{
    public class UsuariosPuntos
    {
        public int Id { get; set; }

        public string IdEmpleado { get; set; }
        public string IdEmpleadoOtorga { get; set; }


        public int Valor { get; set; }

        public string Tipo { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int IdPedido { get; set; }
        public int ConceptoId { get; set; }
        public string Justificacion { get; set; }
        public bool Activo { get; set; }

    }
}
