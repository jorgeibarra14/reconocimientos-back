using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reconocimientos.Interfaces;
using Reconocimientos.Models;

namespace Reconocimientos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RolController : Controller
    {
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }


        // GET: api/<RolController>/ObtenerRoles
        [HttpGet("ObtenerRoles")]
        public ActionResult<IEnumerable<Roles>> ObtenerRoles()
        {
            bool activo = true;
            var result = _rolService.ObtenerRoles(activo);

            return Ok(result.OrderBy(c => c.nombre));
        }

        // GET api/<RolController>/ObtenerRolId
        [HttpGet("ObtenerRolId")]
        public ActionResult<IEnumerable<Roles>> ObtenerRolId(int id)
        {
            bool activo = true;
            return Ok(_rolService.ObtenerRolId(id, activo));
        }

        // POST api/<RolController>/AgregarRol
        [HttpPost("AgregarRol")]
        public IActionResult AgregarRol([FromBody] Roles rol)
        {
            return Ok(_rolService.InsertarRol(rol));
        }

        // POST api/<RolController>/EditarRol
        [HttpPost("EditarRol")]
        public IActionResult EditarRol([FromBody] Roles rol)
        {
            return Ok(_rolService.ActualizarRol(rol));
        }

        // POST api/<RolController>/EliminaRol
        [HttpGet("EliminaRol")]
        public IActionResult EliminaRol(int id)
        {
            return Ok(_rolService.EliminarRol(id));
        }
    }
}