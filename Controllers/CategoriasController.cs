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
    public class CategoriasController : Controller
    {
        private readonly ICategoriasService _categoriasService;

        public CategoriasController(ICategoriasService categoriasService)
        {
            _categoriasService = categoriasService;
        }

        // GET: api/<CategoriasController>/ObtenerCategorias
        [HttpGet("ObtenerCategorias")]
        public ActionResult<IEnumerable<Categorias>> ObtenerCategorias()
        {
            return Ok(_categoriasService.ObtenerAllCategorias());
        }

        // GET api/<CategoriasController>/ObtenerCategoriasId
        [HttpGet("ObtenerCategoriasId")]
        public ActionResult<IEnumerable<Categorias>> ObtenerCategoriasId(int id)
        {
            return Ok(_categoriasService.ObtenerCategoriasId(id));
        }

        // POST api/<CategoriasController>/AgregarCategoria
        [HttpPost("AgregarCategoria")]
        public IActionResult AgregarCategoria([FromBody] Categorias categorias)
        {
            return Ok(_categoriasService.InsertarCategorias(categorias));
        }

        // POST api/<CategoriasController>/ModificarCategorias
        [HttpPost("ModificarCategorias")]
        public IActionResult ModificarCategorias([FromBody] Categorias categorias)
        {
            return Ok(_categoriasService.ActulizarCategorias(categorias));
        }

        // POST api/<CategoriasController>/EliminarCategorias
        [HttpGet("EliminarCategorias")]
        public IActionResult EliminarCategorias(int id)
        {
            return Ok(_categoriasService.EliminarCategorias(id));
        }


    }
}