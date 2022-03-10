using System.Collections.Generic;
using Reconocimientos.Models;

namespace Reconocimientos.Interfaces
{
    public interface IProductosService
    {
        IEnumerable<Productos> getAllProducts();

        IEnumerable<Productos> getProductsById(int id);

        int UpdateProducts(Productos productos);

        int DeleteProducts(int id);

        int InsertProducts(Productos productos);

        IEnumerable<Productos> getProductsByCategoryId(int categoriaId);
        int ActualizarStockProducto(Productos productos);

    }
}