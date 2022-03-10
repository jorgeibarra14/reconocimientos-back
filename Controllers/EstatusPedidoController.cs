using System.Collections.Generic;
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
    public class EstatusPedidoController : Controller
    {
        private readonly IEstatusPedidoService _estatusPedidoService;

        public EstatusPedidoController(IEstatusPedidoService EstatusPedidoService)
        {
            _estatusPedidoService = EstatusPedidoService;
        }

        // GET: api/<EstatusPedidoController>/ObtenerEstatusPedido
        [HttpGet("ObtenerEstatusPedido")]
        public ActionResult<IEnumerable<EstatusPedido>> ObtenerEstatusPedido()
        {
            return Ok(_estatusPedidoService.ObtenerAllEstatusPedido());
        }

        // GET api/<EstatusPedidoController>/ObtenerEstatusPedidoId
        [HttpGet("ObtenerEstatusPedidoId")]
        public ActionResult<IEnumerable<EstatusPedido>> ObtenerEstatusPedidoId(int id)
        {
            return Ok(_estatusPedidoService.ObtenerEstatusPedidoId(id));
        }

        // POST api/<EstatusPedidoController>/AgregarEstatusPedido
        [HttpPost("AgregarEstatusPedido")]
        public IActionResult AgregarEstatusPedido([FromBody] EstatusPedido EstatusPedido)
        {
            return Ok(_estatusPedidoService.InsertarEstatusPedido(EstatusPedido));
        }

        // POST api/<EstatusPedidoController>/ModificarEstatusPedido
        [HttpPost("ModificarEstatusPedido")]
        public IActionResult ModificarEstatusPedido([FromBody] EstatusPedido EstatusPedido)
        {
            return Ok(_estatusPedidoService.ActulizarEstatusPedido(EstatusPedido));
        }

        // GET api/<EstatusPedidoController>/EliminarEstatusPedido
        [HttpGet("EliminarEstatusPedido")]
        public IActionResult EliminarEstatusPedido(int id)
        {
            return Ok(_estatusPedidoService.EliminarEstatusPedido(id));
        }


    }
}