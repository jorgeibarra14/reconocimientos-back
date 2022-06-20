using Reconocimientos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reconocimientos.Interfaces
{
    public interface IOdsService
    {
        IEnumerable<InformacionOdsDetalle> ObtenerInformacionODSporId(string id_empleado);
        InformacionOdsDetalle ObtenerObjInformacionODSporId(string id_empleado);
        IEnumerable<InformacionOdsDetalle> ObtenerInformacionODS(string userId, int companyId);
        IEnumerable<InformacionOdsDetalle> ObtenerInformacionODSporNombre(string nombre);
        IEnumerable<InformacionOdsDetalle> GetAllUsers();
    }
}
