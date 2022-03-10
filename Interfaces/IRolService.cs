using System.Collections.Generic;
using Reconocimientos.Models;

namespace Reconocimientos.Interfaces
{
    public interface IRolService
    {
        IEnumerable<Roles> ObtenerRoles(bool activo);
        IEnumerable<Roles> ObtenerRolId(int id, bool activo);
        int InsertarRol(Roles rol);
        int ActualizarRol(Roles rol);
        int EliminarRol(int id);
    }
}