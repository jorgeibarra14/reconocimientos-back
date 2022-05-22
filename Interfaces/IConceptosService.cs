using System.Collections.Generic;
using Reconocimientos.Models;

namespace Reconocimientos.Interfaces
{
    public interface IConceptosService
    {
        IEnumerable<Competencias> ObtenerAllCompetencias();

        IEnumerable<Competencias> ObtenerCompetencias(bool activo,string nivel);

        IEnumerable<Competencias> ObtenerCompetenciaId(int id);

        int ActulizarCompetencias(Competencias competencias);

        int EliminarCompetencias(int id);

        int InsertarCompetencias(Competencias competencias);

    }
}