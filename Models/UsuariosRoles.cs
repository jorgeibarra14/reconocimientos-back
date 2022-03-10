using System;

namespace Reconocimientos.Models
{
    public class UsuariosRoles
    {
        public int id { get; set; }
        public string id_empleado { get; set; }
        public int id_rol { get; set; }
        public bool activo { get; set; }
        public DateTime fecha_registro { get; set; }
        public string nombre { get; set; }
        public Roles roles { get; set; }
        public string activoDesc { get { return activo ? "Si" : "No"; } }
    }
}