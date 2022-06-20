using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reconocimientos.Models
{
    public class Colaboradores
    {
        public string ID { get; set; }
        public string Id_Mga_PlazasMh { get; set; }
        public string Ava { get; set; }
        public string Nombre { get; set; }
        public string Area { get; set; }
        public string Sistema { get; set; }
        public string Uen { get; set; }
        public string Cve_puesto { get; set; }
        public string Puesto { get; set; }
        public string Id_Autorizador { get; set; }
        public string Nombre_Autorizador { get; set; }
        public bool IsObjetivoTemprano { get; set; }
        public bool Activo { get; set; }
        public string Regional { get; set; }
        public string Email { get; set; }
        public string NivelPuesto { get; set; }
        public string Foto { get; set; }
    }
}
