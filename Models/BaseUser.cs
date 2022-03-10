using System;

namespace Reconocimientos.Models
{
    public class BaseUser
    {
        public string Antiguedad { get; set; }
        public string Apellidos { get; set; }
        public string Departamento { get; set; }
        public string Empresa { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Foto { get; set; }
        public string Id { get; set; }
        public bool? IsJefe { get; set; }
        public string JefeUserName { get; set; }
        public string Mail { get; set; }
        public string Mobile { get; set; }
        public string Nombre { get; set; }
        public int RolId { get; set; }
        public string Puesto { get; set; }
        public string Telefono { get; set; }
        public string UserName { get; set; }
        public string Oficina { get; set; }
        public string Area { get; set; }
        public string Direccion { get; set; }
        public string NivelOrganizacional { get; set; }
    }
}