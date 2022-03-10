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
    public class UsuarioRolController : Controller
    {
        private readonly IUsuarioRolService _usuarioRolService;

        public UsuarioRolController(IUsuarioRolService usuarioRolService)
        {
            _usuarioRolService = usuarioRolService;
        }

        // GET: api/<UsuarioRolController>/ObtenerUsuarioRol
        [HttpGet("ObtenerUsuarioRol")]
        public ActionResult<IEnumerable<UsuariosRoles>> ObtenerUsuarioRol()
        {
            bool activo = true;
            var result = _usuarioRolService.ObtenerUsuarioRol(activo);
            return Ok(result.OrderBy(c => c.nombre));
        }

        // GET api/<UsuarioRolController>/ObtenerUsuarioRolIdEmpleado
        [HttpGet("ObtenerUsuarioRolIdEmpleado")]
        public ActionResult<IEnumerable<UsuariosRoles>> ObtenerUsuarioRolIdEmpleado(string id_empleado)
        {
            List<UsuariosRoles> usuarioRolResult = (List<UsuariosRoles>)_usuarioRolService.ObtenerUsuarioRolIdEmpleado(id_empleado);
            int rolId = 0;

            if (usuarioRolResult.Count > 0)
            {
                rolId = usuarioRolResult[0].id_rol;
            }
            return Ok(rolId);
        }


        // GET api/<UsuarioRolController>/ObtenerUsuarioRolById
        [HttpGet("ObtenerUsuarioRolById")]
        public ActionResult<IEnumerable<UsuariosRoles>> ObtenerUsuarioRolById(int id)
        {
            List<UsuariosRoles> usuarioRolResult = (List<UsuariosRoles>)_usuarioRolService.ObtenerUsuarioRolIdById(id);
            return Ok(usuarioRolResult);
        }

        // POST api/<UsuarioRolController>/AgregarUsuarioRol
        [HttpPost("AgregarUsuarioRol")]
        public IActionResult AgregarUsuarioRol([FromBody] UsuariosRoles usurioRol)
        {

            List<UsuariosRoles> usuarioRolResult = (List<UsuariosRoles>)_usuarioRolService.ObtenerUsuarioRolIdEmpleado(usurioRol.id_empleado);
            int result = 0;

            if (usuarioRolResult.Count == 0)
            {
                result = _usuarioRolService.InsertarUsuarioRol(usurioRol);
            }
            else
            {
                return BadRequest("Usuario duplicado");
            }

            return Ok(result);
        }

        // POST api/<UsuarioRolController>/EditarUsuarioRol
        [HttpPost("EditarUsuarioRol")]
        public IActionResult EditarUsuarioRol([FromBody] UsuariosRoles usurioRol)
        {
            return Ok(_usuarioRolService.ActualizarUsuarioRol(usurioRol));
        }

        // POST api/<UsuarioRolController>/EliminaUsuarioRol
        [HttpGet("EliminaUsuarioRol")]
        public IActionResult EliminaUsuarioRol(int id)
        {
            return Ok(_usuarioRolService.EliminarUsuarioRol(id));
        }

    }
}