using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Reconocimientos.Interfaces;
using Reconocimientos.Models;

namespace Reconocimientos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CompetenciaController : Controller
    {
        private readonly ICompetenciaService _competenciaService;
        private readonly IConfiguration _configuration;

        public CompetenciaController(ICompetenciaService competenciaService, IConfiguration configuration)
        {
            _configuration = configuration;
            _competenciaService = competenciaService;
        }

        // POST api/<CompetenciaController>/AgregarCompetencia
        [HttpPost("AgregarCompetencia")]
        public IActionResult AgregarCompetencia([FromBody] Competencias competencia)
        {
            return Ok(_competenciaService.InsertarCompetencias(competencia));
        }

        // POST api/<CompetenciaController>/ModificarCompetencia
        [HttpPost("ModificarCompetencia")]
        public IActionResult ModificarCompetencia([FromBody] Competencias competencias)
        {
            return Ok(_competenciaService.ActulizarCompetencias(competencias));
        }

        // POST api/<CompetenciaController>/EliminarCompetencia
        [HttpPost("EliminarCompetencia")]
        public IActionResult EliminarCompetencia(int id)
        {
            return Ok(_competenciaService.EliminarCompetencias(id));
        }

        // GET: api/<CompetenciaController>/ObtenerTodasCompetencias
        [HttpGet("ObtenerTodasCompetencias")]
        public ActionResult<IEnumerable<Competencias>> ObtenerTodasCompetencias()
        {
            return Ok(_competenciaService.ObtenerAllCompetencias());
        }

        // GET: api/<CompetenciaController>/ObtenerCompetencias
        [HttpGet("ObtenerCompetencias")]
        public ActionResult<IEnumerable<CompetencyViewModel>> ObtenerCompetencias(bool activo,string nivel)
        {
            return Ok(_competenciaService.ObtenerCompetencias(activo, nivel));
        }

        // GET api/<CompetenciaController>/ObtenerCompetenciaId
        [HttpGet("ObtenerCompetenciaId")]
        public ActionResult<IEnumerable<Competencias>> ObtenerCompetenciaId(int id)
        {
            return Ok(_competenciaService.ObtenerCompetenciaId(id));
        }

    }
}