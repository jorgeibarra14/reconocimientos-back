using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reconocimientos.Interfaces;
using Reconocimientos.Models;
using Reconocimientos.Services;

namespace Reconocimientos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductosController : Controller
    {
        private readonly IProductosService _productoservice;

        public ProductosController(IProductosService productoservice)
        {
            _productoservice = productoservice;
        }

        // GET: api/<ProductosController>/ObtenerProductos
        [HttpGet("ObtenerProductos")]
        public ActionResult<IEnumerable<Productos>> ObtenerProductos()
        {
            return Ok(_productoservice.getAllProducts());
        }

        // GET api/<ProductosController>/ObtenerProductosPorId
        [HttpGet("ObtenerProductosPorId")]
        public ActionResult<IEnumerable<Productos>> ObtenerProductosPorId(int id)
        {
            return Ok(_productoservice.getProductsById(id));
        }

        // POST api/<ProductosController>/AgregarProductos
        [HttpPost("AgregarProductos")]
        public IActionResult AgregarProductos([FromBody] Productos productos)
        {
            return Ok(_productoservice.InsertProducts(productos));
        }

        // POST api/<ProductosController>/ModificarProductos
        [HttpPost("ModificarProductos")]
        public IActionResult ModificarProductos([FromBody] Productos productos)
        {
            return Ok(_productoservice.UpdateProducts(productos));
        }

        // POST api/<ProductosController>/EliminarProductos
        [HttpGet("EliminarProductos")]
        public IActionResult EliminarProductos(int id)
        {
            return Ok(_productoservice.DeleteProducts(id));
        }

        // GET api/<ProductosController>/ObtenerProductosPorCategoriaId
        [HttpGet("ObtenerProductosPorCategoriaId")]
        public ActionResult<IEnumerable<Productos>> ObtenerProductosPorCategoriaId(int categoriaId)
        {
            return Ok(_productoservice.getProductsByCategoryId(categoriaId));
        }


    }
}