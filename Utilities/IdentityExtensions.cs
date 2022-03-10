using System.Security.Claims;
using System.Security.Principal;

namespace Reconocimientos.Utilities
{
    public static class IdentityExtensions
    {
        public static string ObtenerNombre(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.Name);

            return claim?.Value ?? string.Empty;
        }

        public static string ObtenerApellidos(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst("Lastname");

            return claim?.Value ?? string.Empty;
        }

        public static string ObtenerPuesto(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst("Puesto");

            return claim?.Value ?? string.Empty;
        }

        public static int ObtenerId(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst("Id");

            return int.TryParse(claim?.Value, out var x) ? x : 0;
        }

        public static string ObtenerRol(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.Role);

            return claim?.Value ?? string.Empty;
        }

        public static int ObtenerRolId(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst("RolId");

            return int.TryParse(claim?.Value, out var x) ? x : 0;
        }
    }
}