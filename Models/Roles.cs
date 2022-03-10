using System;

namespace Reconocimientos.Models
{
    public class Roles
    {
        public int id { set; get; }
        public string nombre { set; get; }
        public string descripcion { set; get; }
        public bool activo { set; get; }
        public DateTime fecha_registro { set; get; }
        public string activoDesc { get { return activo ? "Si" : "No"; } }
    }
}