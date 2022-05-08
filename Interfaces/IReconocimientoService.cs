using Reconocimientos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reconocimientos.Interfaces
{
    public interface IReconocimientoService
    {
        IEnumerable<Models.Reconocimientos> ObtenerReconocimientos();

        int ObtenerPuntosAcumulados(string idempleadorecibe, bool activo);

        IEnumerable<Models.Reconocimientos> ObtenerReconocimientosId(int id, bool activo);

        IEnumerable<ReconocimientosPorAutorizar> ObtenerReconocimientosPorAutorizar(string id_empleado_autorizador, bool activo);

        IEnumerable<ReconocimientosPorAutorizar> ObtenerReconocimientosPorAutorizarAdmin(bool activo);

        int AprobarRechazarReconocimiento(Models.Reconocimientos reconocimientos);

        int ActualizarReconocimiento(Models.Reconocimientos reconocimientos);

        int InsertarReconocimiento(Models.Reconocimientos reconocimientos);

        int InsertarPuntos(UsuariosPuntos usuariosPuntos);
        int EliminarReconocimiento(int id);

        IEnumerable<MisReconocimientos> MisReconocimientos(string id_empleado_recibe, bool activo);

        IEnumerable<MisReconocimientosDetalle> MisReconocimientosPorCompetencia(string id_empleado_recibe, string nombreCompetencia, bool activo);

        IEnumerable<ReconocimientosEntregados> ReconocerAOtros(string id_empleado_envia, bool activo);

        IEnumerable<ReconocimientosEntregadosDetalles> ReconocerAOtrosPorCompetencia(string id_empleado_envia, string nombreCompetencia, bool activo);

        int ObtenerAutorizador(string area, string sistema, string regional);

        int ValidarReconociminetoEntregado(string id_empleado_recibe, string id_empleado_envia, bool activo);
        IEnumerable<TopReconocimiento> ObtenerTopReconocidos();
    }
}
