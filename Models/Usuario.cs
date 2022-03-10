using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reconocimientos.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Puesto { get; set; }
        public string RFC { get; set; }
        public string Departamento { get; set; }
        public string Oficina { get; set; }
        public string Area { get; set; }
        public string NivelPuesto { get; set; }
        public string Compania { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
        public string SueldoDiario { get; set; }
        public string Imss { get; set; }
        public string NombreCompleto { get { return Nombre + " " + Paterno + " " + Materno; } }
        public string Foto { get; set; }
    }
}
