using System.Collections.Generic;
using Reconocimientos.Models;

namespace Reconocimientos.Interfaces
{
    public interface ICategoriasService
    {
        IEnumerable<Categorias> ObtenerAllCategorias();

        IEnumerable<Categorias> ObtenerCategoriasId(int id);

        int InsertarCategorias(Categorias categorias);

        int ActulizarCategorias(Categorias categorias);

        int EliminarCategorias(int id);
    }
}