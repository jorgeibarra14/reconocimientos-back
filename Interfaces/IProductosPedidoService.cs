using System.Collections.Generic;
using Reconocimientos.Models;

namespace Reconocimientos.Interfaces
{
    public interface IProductosPedidoService
    {
        IEnumerable<ProductosPedido> ObtenerAllProductosPedido();

        IEnumerable<ProductosPedido> ObtenerProductosPedidoId(int id);

        int InsertarProductosPedido(ProductosPedido productosPedido);

        int ActulizarProductosPedido(ProductosPedido productosPedido);

        int EliminarProductosPedido(int id);
    }
}