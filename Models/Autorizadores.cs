using System;

namespace Reconocimientos.Models
{
    public class Autorizadores
    {
        public string id { get; set; }
        public string area { get; set; }
        public string region { get; set; }
        public string sistema { get; set; }
        public string idempleadoautorizador { get; set; }
        public string uen { get; set; }
        public bool activo { get; set; }
        public DateTime fecha_registro { get; set; }
        public string nombreempleadoautorizador { get; set; }

        
    }

    public class AutorizadoresDistintos
    {
        public int idEmpleadoAutorizador { get; set; }

        public string nombreAutorizador { get; set; }
    }
}