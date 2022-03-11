using Newtonsoft.Json;

namespace Reconocimientos.Models
{
    public class ITGovLogin
    {
        public string ApplicationId { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
    }

    public class ApplicationConfiguration
    {
        [JsonProperty("User")] public ItGovUser Usuario { get; set; }

        [JsonProperty("Token")] public string Token { get; set; }
    }

    public class ItGovUser
    {
        public Role Role;
        public string Id { get; set; }
        public int AppId { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Email { get; set; }
        public string Puesto { get; set; }
        public string NivelPuesto { get; set; }
        public string RFC { get; set; }
        public string Departamento { get; set; }
        public string Oficina { get; set; }
        public string Fullname => Nombre + " " + Paterno + " " + Materno;
        public string Foto { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}