using System.Collections.Generic;
using System.Threading.Tasks;
using Reconocimientos.Models;

namespace Reconocimientos.Interfaces
{
    public interface INotificacionesService
    {
        IEnumerable<Notificaciones> ObtenerNotificaciones();
        IEnumerable<Notificaciones> ObtenerNotificacionesIdEmpleado(string id_empleado);
        int InsertarNotificacion(Notificaciones notificacion);
        int MarcarComoLeidoIdEmpleado(string id_empleado);
        int ActualizarNotificacion(Notificaciones notificacion);
        int EliminarNotificacion(int id);
        Task EnviarNotificacion(CorreoNotificacion notificacion);
    }
}