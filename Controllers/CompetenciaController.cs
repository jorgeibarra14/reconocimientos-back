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
    public class CompetenciaController : Controller
    {
        private readonly ICompetenciaService _competenciaService;

        public CompetenciaController(ICompetenciaService competenciaService)
        {
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
        public ActionResult<IEnumerable<Competencias>> ObtenerCompetencias(bool activo,string nivel)
        {
            return Ok(_competenciaService.ObtenerCompetencias(activo,nivel));
        }

        // GET api/<CompetenciaController>/ObtenerCompetenciaId
        [HttpGet("ObtenerCompetenciaId")]
        public ActionResult<IEnumerable<Competencias>> ObtenerCompetenciaId(int id)
        {
            return Ok(_competenciaService.ObtenerCompetenciaId(id));
        }

    }
}