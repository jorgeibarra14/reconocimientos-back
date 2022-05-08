using System.Collections.Generic;
using Reconocimientos.Models;

namespace Reconocimientos.Interfaces
{
    public interface IPedidosService
    {
        IEnumerable<Pedidos> ObtenerAllPedidos();

        IEnumerable<Pedidos> ObtenerPedidosId(int id);
        IEnumerable<Pedidos> ObtenerPedidosSolicitante(int id_solicitante);

        int InsertarPedidos(Pedidos pedidos);

        int ActulizarPedidos(Pedidos pedidos);

        int EliminarPedidos(int id);
        int InsertarPedidoCelular(PedidosCelular pedidosCelular);
    }
}