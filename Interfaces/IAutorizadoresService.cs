using System.Collections.Generic;
using System.Threading.Tasks;
using Reconocimientos.Models;

namespace Reconocimientos.Interfaces
{
    public interface IAutorizadoresService
    {
        IEnumerable<Autorizadores> ObtenerAutorizadores();
        IEnumerable<AutorizadoresDistintos> ObtenerAutorizadoresDistintos();
        IEnumerable<Autorizadores> ObtenerAutorizadoresId(string id);
        int InsertarAutorizadores(Autorizadores autorizadores);
        int ActualizarAutorizadores(Autorizadores autorizadores);
        int EliminarAutorizadores(int id);
    }
}