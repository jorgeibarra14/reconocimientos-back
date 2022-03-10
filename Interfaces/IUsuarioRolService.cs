using System.Collections.Generic;
using Reconocimientos.Models;

namespace Reconocimientos.Interfaces
{
    public interface IUsuarioRolService
    {
        IEnumerable<UsuariosRoles> ObtenerUsuarioRol(bool activo);
        IEnumerable<UsuariosRoles> ObtenerUsuarioRolIdEmpleado(string id_empleado);
        IEnumerable<UsuariosRoles> ObtenerUsuarioRolIdById(int id);
        int InsertarUsuarioRol(UsuariosRoles usuarioRol);
        int ActualizarUsuarioRol(UsuariosRoles usuarioRol);
        int EliminarUsuarioRol(int id);
    }
}