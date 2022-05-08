using System;
using System.Collections.Generic;

namespace Reconocimientos.Models
{
    public class Pedidos
    {
        public int id { get; set; }
        public string id_solicitante { get; set; }
        public string nombre_solicitante { get; set; }
        public string puesto_solicitante { get; set; }
        public string area_solicitante { get; set; }
        public string sistema_solicitante { get; set; }
        public string id_autorizador { get; set; }
        public string nombre_autorizador{ get; set; }
        public bool aprobado { get; set; }
        public string comentario_resolucion { get; set; }
        public DateTime fecha_resolucion{ get; set; }
        public bool activo { get; set; }
        public DateTime fecha_creacion{ get; set; }
        public List<ProductosPedido> productos { get; set; }
        public EstatusPedido estatusPedido { get; set; }

        public int celularEmpleado  { get; set; }
    }
}