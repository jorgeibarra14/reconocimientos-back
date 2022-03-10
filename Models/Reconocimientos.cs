using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reconocimientos.Models
{
    public class Reconocimientos
    {
        public int id { get; set; }
        public string id_empleado_envia { get; set; }
        public string id_empleado_recibe { get; set; }
        public int id_competencia { get; set; }
        public string motivo { set; get; }
        public string logro { set; get; }
        public string id_empleado_autorizador { get; set; }
        public bool aprobado { get; set; }
        public string comentario_resolucion { get; set; }
        public DateTime fecha_resolucion { get; set; }
        public bool activo { get; set; }
        public DateTime fecha_registro { get; set; }

        public int id_puntos { get; set; }
    }


    public class ReconocimientosPorAutorizar
    {
        public int id { get; set; }
        public string id_empleado_envia { get; set; }

        public string enviadoPor { get; set; }

        public string id_empleado_recibe { get; set; }

        public string reconoceA { get; set; }

        public int id_competencia { get; set; }

        public string competencia { get; set; }

        public string motivo { set; get; }

        public string logro { set; get; }

        public string id_empleado_autorizador { get; set; }

        public string autorizador { set; get; }

        public bool aprobado { get; set; }

        public string comentario_resolucion { get; set; }

        public DateTime fecha_resolucion { get; set; }

        public bool activo { get; set; }

        public DateTime fecha_registro { get; set; }
    }
}
