using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reconocimientos.Interfaces;
using Reconocimientos.Models;
using System.Collections.Generic;
using System.Net;

namespace Reconocimientos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PuntosController : Controller
    {
        readonly IPuntosService _puntoService;
        public PuntosController(IPuntosService puntoService)
        {
            _puntoService = puntoService;
        }

        // POST api/<PuntosController>/AgregarPuntos
        [HttpPost("AgregarPuntos")]
        public IActionResult AgregarPuntos([FromBody] Puntos puntos)
        {
            return Ok(_puntoService.InsertarPuntos(puntos));
        }

        // POST api/<PuntosController>/ModificarPuntos
        [HttpPost("ModificarPuntos")]
        public IActionResult ModificarPuntos([FromBody] Puntos puntos)
        {
            return Ok(_puntoService.ActulizarPuntos(puntos));
        }

        // POST api/<PuntosController>/EliminarPuntos
        [HttpPost("EliminarPuntos")]
        public IActionResult EliminarPuntos(int id)
        {
            return Ok(_puntoService.EliminarPuntos(id));
        }

        // GET: api/<PuntosController>/ObtenerPuntos
        [HttpGet("ObtenerPuntos")]
        public ActionResult<IEnumerable<Puntos>> ObtenerPuntos(string id_empleado)
        {
            return Ok(_puntoService.ObtenerPuntos(id_empleado));
        }

        // GET api/<PuntosController>/ObtenerPuntosDisponibles
        [HttpGet("ObtenerPuntosDisponibles")]
        public int ObtenerPuntosDisponibles(string id_empleado, bool activo)
        {
            return _puntoService.PuntosDisponibles(id_empleado,activo);
        }

        // POST api/<PuntosController>/ActualizarPuntosReconocimiento
        [HttpPost("ActualizarPuntosReconocimiento")]
        public IActionResult ActualizarPuntosReconocimiento([FromBody] Puntos puntos)
        {
            return Ok(_puntoService.ActualizarPuntosReconocimiento(puntos));
        }

        // GET api/<PuntosController>/CorteManualDePuntos
        [HttpGet("CorteManualDePuntos")]
        public IActionResult CorteManualDePuntos()
        {
            return Ok(_puntoService.CorteManualDePuntos());
        }

        [HttpPost("agregarPuntosTienda")]

        public IActionResult AgregarPuntos([FromBody] UsuariosPuntos usuariosPuntos)
        {
            var usuarioPuntos = new UsuariosPuntos { IdEmpleado = usuariosPuntos.IdEmpleado, Valor = usuariosPuntos.Valor, Tipo = usuariosPuntos.Tipo, IdPedido = 0, Justificacion = usuariosPuntos.Justificacion, ConceptoId = usuariosPuntos.ConceptoId, IdEmpleadoOtorga = usuariosPuntos.IdEmpleadoOtorga };


            return Ok(_puntoService.InsertarPuntosTienda(usuarioPuntos));
        }
    }
}
