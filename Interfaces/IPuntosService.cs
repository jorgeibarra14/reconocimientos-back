using Reconocimientos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reconocimientos.Interfaces
{
    public interface IPuntosService
    {
        IEnumerable<Puntos> ObtenerPuntos(string id_empleado);

        int PuntosDisponibles(string id,bool activo);

        int InsertarPuntos(Puntos puntos);

        int ActulizarPuntos(Puntos puntos);

        int EliminarPuntos(int id);

        int ActualizarPuntosReconocimiento(Puntos puntos);

        int CorteManualDePuntos();

        int InsertarPuntosTienda(UsuariosPuntos usuariosPuntos);

    }
}
