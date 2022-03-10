using Reconocimientos.Interfaces;
using Reconocimientos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reconocimientos.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ColaboradoresController : Controller
    {
        private readonly IColaboradoresService _colaboradoresService;
        private readonly IConfiguration _config;

        public ColaboradoresController(IConfiguration configuration,
            IColaboradoresService colaboradoresService)
        {
            _config = configuration;
            _colaboradoresService = colaboradoresService;
        }

        // GET api/<ColaboradoresController>/GetColaboradorByIdCorp
        [HttpGet("GetColaboradorByIdCorp")]
        public IActionResult GetColaboradorByIdCorp(string UserId)
        {
            try
            {
                Colaboradores resultColaborador = _colaboradoresService.GetColaboradorIdUnico(UserId);
                return Ok(resultColaborador);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<ColaboradoresController>/ObtenerAreasColaborador
        [HttpGet("ObtenerAreasColaborador")]
        public IActionResult ObtenerAreasColaborador()
        {
            try
            {
                List<Colaboradores> resultColaborador = _colaboradoresService.GetAreasColaborador();
                List<Area> areasDiponibles = new List<Area>();

                foreach (Colaboradores item in resultColaborador)
                {
                    areasDiponibles.Add(new Area() {Nombre= item.Area });
                }

                return Ok(areasDiponibles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<ColaboradoresController>/ObtenerColaboradores
        [HttpGet("ObtenerColaboradores")]
        public IActionResult ObtenerColaboradores()
        {
            try
            {
                List<Colaboradores> resultColaborador = _colaboradoresService.GetAllColaboradores();
                return Ok(resultColaborador);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<ColaboradoresController>/ObtenerColaboradoresPorNombre
        [HttpGet("ObtenerColaboradoresPorNombre")]
        public IActionResult ObtenerColaboradoresPorNombre(string nombre)
        {
            try
            {
                List<Colaboradores> resultColaborador = _colaboradoresService.GetAllColaboradoresByName( nombre);
                return Ok(resultColaborador);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
