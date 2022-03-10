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
    public class ProductosPedidoController : Controller
    {
        private readonly IProductosPedidoService _productosPedidoService;

        public ProductosPedidoController(IProductosPedidoService ProductosPedidoService)
        {
            _productosPedidoService = ProductosPedidoService;
        }

        // GET: api/<ProductosPedidoController>/ObtenerProductosPedido
        [HttpGet("ObtenerProductosPedido")]
        public ActionResult<IEnumerable<ProductosPedido>> ObtenerProductosPedido()
        {
            return Ok(_productosPedidoService.ObtenerAllProductosPedido());
        }

        // GET api/<ProductosPedidoController>/ObtenerProductosPedidoId
        [HttpGet("ObtenerProductosPedidoId")]
        public ActionResult<IEnumerable<ProductosPedido>> ObtenerProductosPedidoId(int id)
        {
            return Ok(_productosPedidoService.ObtenerProductosPedidoId(id));
        }

        // POST api/<ProductosPedidoController>/AgregarProductosPedido
        [HttpPost("AgregarProductosPedido")]
        public IActionResult AgregarProductosPedido([FromBody] ProductosPedido ProductosPedido)
        {
            return Ok(_productosPedidoService.InsertarProductosPedido(ProductosPedido));
        }

        // POST api/<ProductosPedidoController>/ModificarProductosPedido
        [HttpPost("ModificarProductosPedido")]
        public IActionResult ModificarProductosPedido([FromBody] ProductosPedido ProductosPedido)
        {
            return Ok(_productosPedidoService.ActulizarProductosPedido(ProductosPedido));
        }

        // POST api/<ProductosPedidoController>/EliminarProductosPedido
        [HttpGet("EliminarProductosPedido")]
        public IActionResult EliminarProductosPedido(int id)
        {
            return Ok(_productosPedidoService.EliminarProductosPedido(id));
        }


    }
}