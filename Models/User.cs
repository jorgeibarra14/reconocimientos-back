using Microsoft.IdentityModel.JsonWebTokens;

namespace Reconocimientos.Models
{
    public class User : BaseUser
    {
        public static object Identity { get; set; }
        public string Responsabilidad { get; set; }
        public string Responsabilidades { get; set; }
        public bool IsAdmin { get; set; }
        public JsonWebToken Token { get; set; }

        public override string ToString()
        {
            return $"{Nombre} {Apellidos}";
        }
    }
}