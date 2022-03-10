using System.Collections.Generic;
using System.Threading.Tasks;
using Reconocimientos.Models;

namespace Reconocimientos.Interfaces
{
    public interface IPuestosService
    {
        IEnumerable<Puestos> ObtenerPuestos();
        IEnumerable<Puestos> ObtenerPuestosIdPuesto(int id_puesto);
        IEnumerable<Puestos> ObtenerPuestosNombre(string nombre);
        int InsertarPuestos(Puestos puesto);
        int ActualizarPuestos(Puestos puesto);
        int EliminarPuestos(int id);
    }
}