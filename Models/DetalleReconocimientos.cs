namespace Reconocimientos.Models
{
    public class MisReconocimientos
    {
        public int id_competencia { get; set; }

        public string nombre { get; set; }

        public string img { get; set; }

        public int cantidad { get; set; }
    }

    public class MisReconocimientosDetalle
    {
        public string id { get; set; }

        public string nombre { get; set; }

        public string competencia { get; set; }

        public int competenciaId { get; set; }

        public string motivo { get; set; }

        public string img { get; set; }
    }

    public class ReconocimientosEntregados
    {
        public int id_competencia { get; set; }

        public string nombre { get; set; }

        public string img { get; set; }

        public int cantidad { get; set; }
    }

    public class ReconocimientosEntregadosDetalles
    {
        public string id { get; set; }

        public string nombre { get; set; }

        public string competencia { get; set; }

        public int competenciaId { get; set; }

        public string motivo { get; set; }

        public string img { get; set; }
    }

    public class InformacionOdsDetalle
    {
        //public int Id { get; set; }
        //public string Nombre { get; set; }
        //public string Paterno { get; set; }
        //public string Materno { get; set; }
        //public string Email { get; set; }
        //public string Telefono { get; set; }
        //public string Puesto { get; set; }
        //public string RFC { get; set; }
        //public string Departamento { get; set; }
        //public string Oficina { get; set; }
        //public string Area { get; set; }
        //public string NivelPuesto { get; set; }
        //public string Compania { get; set; }
        //public int RoleId { get; set; }
        //public string Role { get; set; }
        //public string SueldoDiario { get; set; }
        //public string Imss { get; set; }
        //public string Sistema { get; set; }
        //public string Regional { get; set; }
        //public int puestoId { get; set; }
        //public string NombreCompleto { get { return Nombre + " " + Paterno + " " + Materno; } }
        //public string Foto { get; set; }

        public string Id { get; set; }
        public string Id_MGA_PlazasMH { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string NombreCompleto => Nombre + ' ' + Paterno + ' ' + Materno;
        public string Area { get; set; }
        public string Sistema { get; set; }
        public string Regional { get; set; }
        public string Uen { get; set; }
        public int Cve_Puesto { get; set; }
        public string Puesto { get; set; }
        public string Email { get; set; }
        public string Id_Autorizador { get; set; }
        public string Nombre_Autorizador { get; set; }
        public int IsObjetivoTemprano { get; set; }
        public int Activo { get; set; }
        public string NivelPuesto { get; set; }
        public string Foto { get; set; }
    }


}
