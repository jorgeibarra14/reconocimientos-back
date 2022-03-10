using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Reconocimientos.Interfaces;
using Reconocimientos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Reconocimientos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PuestosController : Controller
    {
        private readonly IPuestosService _puestosService;
        private readonly IConfiguration _configuration;

        public PuestosController(IPuestosService puestosService, IConfiguration configuration)
        {
            _puestosService = puestosService;
            _configuration = configuration;
        }

        // POST api/<PuestosController>/AgregarPuesto
        [HttpPost("AgregarPuesto")]
        public IActionResult AgregarPuesto([FromBody] Puestos puesto)
        {

            return Ok(_puestosService.InsertarPuestos(puesto));
        }

        // POST api/<PuestosController>/EditarPuesto
        [HttpPost("EditarPuesto")]
        public IActionResult EditarPuesto([FromBody] Puestos puesto)
        {
            return Ok(_puestosService.ActualizarPuestos(puesto));
        }

        // POST api/<PuestosController>/EliminarPuesto
        [HttpPost("EliminarPuesto")]
        public IActionResult EliminarPuesto(int id)
        {
            return Ok(_puestosService.EliminarPuestos(id));
        }

        // GET: api/<PuestosController>/ConsultarPuestos
        [HttpGet("ConsultarPuestos")]
        public ActionResult<IEnumerable<Puestos>> ConsultarPuestos()
        {
            return Ok(_puestosService.ObtenerPuestos());
        }

        // GET api/<PuestosController>/ConsultarPuestoPorIdPuesto
        [HttpGet("ConsultarPuestoPorIdPuesto")]
        public ActionResult<IEnumerable<Puestos>> ConsultarPuestoPorIdPuesto(int id_puesto)
        {
            return Ok(_puestosService.ObtenerPuestosIdPuesto(id_puesto));
        }

        // GET api/<PuestosController>/ConsultarPuestoPorNombre
        [HttpGet("ConsultarPuestoPorNombre")]
        public ActionResult<IEnumerable<Puestos>> ConsultarPuestoPorNombre(string nombre)
        {
            return Ok(_puestosService.ObtenerPuestosNombre(nombre));
        }

        // GET api/<PuestosController>/ConsultarNivelPuestoPorNombre
        [HttpGet("ConsultarNivelPuestoPorNombre")]
        public string ConsultarNivelPuestoPorNombre(string nombre)
        {
            List<Puestos> puesto = new List<Puestos>();
            string nivel = "";
            puesto = (List<Puestos>)_puestosService.ObtenerPuestosNombre(nombre);
            if (puesto.Count <= 0)
            {
                return ("Puesto no encontrado");
            }
            else
            {
                nivel = puesto[0].nivel;
            }
            return (nivel);
        }
    }
}
