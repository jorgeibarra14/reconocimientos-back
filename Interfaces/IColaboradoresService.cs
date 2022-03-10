using Reconocimientos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reconocimientos.Interfaces
{
    public interface IColaboradoresService
    {
        Colaboradores GetColaboradorIdCorp(string idCorporativo);
        Colaboradores GetColaboradorIdUnico(string id);
        List<Colaboradores> GetAreasColaborador();
        List<Colaboradores> GetAllColaboradores();
        List<Colaboradores> GetAllColaboradoresByName(string nombre);
        List<Colaboradores> GetColaboradorPuestoId(int puestoId,int periodoPadreID);

    }
}
