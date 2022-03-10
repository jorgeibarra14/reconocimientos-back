using System.Collections.Generic;
using Reconocimientos.Models;

namespace Reconocimientos.Interfaces
{
    public interface IEstatusPedidoService
    {
        IEnumerable<EstatusPedido> ObtenerAllEstatusPedido();

        IEnumerable<EstatusPedido> ObtenerEstatusPedidoId(int id);

        int InsertarEstatusPedido(EstatusPedido estatusPedido);

        int ActulizarEstatusPedido(EstatusPedido estatusPedido);

        int EliminarEstatusPedido(int id);
    }
}