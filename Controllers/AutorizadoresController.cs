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
    public class AutorizadoresController : Controller
    {
        private readonly IAutorizadoresService _AutorizadoresService;
        private readonly IOdsService _odsService;
        private readonly IConfiguration _configuration;

        public AutorizadoresController(IAutorizadoresService AutorizadoresService, IOdsService OdsService, IConfiguration configuration)
        {
            _AutorizadoresService = AutorizadoresService;
            _odsService = OdsService;
            _configuration = configuration;
        }

        // POST api/<AutorizadoresController>/AgregarAutorizador
        [HttpPost("AgregarAutorizador")]
        public IActionResult AgregarAutorizador([FromBody] Autorizadores autorizador)
        {
            return Ok(_AutorizadoresService.InsertarAutorizadores(autorizador));
        }

        // POST api/<AutorizadoresController>/EditarAutorizador
        [HttpPost("EditarAutorizador")]
        public async Task<IActionResult> EditarAutorizadorAsync([FromBody] Autorizadores autorizador)
        {
            List<InformacionOdsDetalle> usuario = (List<InformacionOdsDetalle>)await ObtenerEmpleadosPorNombre(autorizador.nombreempleadoautorizador);

            if (usuario.Count() > 0)
            {
                autorizador.idempleadoautorizador = usuario[0].Id;
            }
            else
            {
                autorizador.idempleadoautorizador = "";
            }

            return Ok(_AutorizadoresService.ActualizarAutorizadores(autorizador));
        }

        // POST api/<AutorizadoresController>/EliminarAutorizador
        [HttpPost("EliminarAutorizador")]
        public IActionResult EliminarAutorizador(int id)
        {
            return Ok(_AutorizadoresService.EliminarAutorizadores(id));
        }

        // GET: api/<AutorizadoresController>/ConsultarAutorizadores
        [HttpGet("ConsultarAutorizadores")]
        //[AllowAnonymous]
        public ActionResult<IEnumerable<Autorizadores>> ConsultarAutorizadores()
        {
     
            List<Autorizadores> listaAutorizadores = (List<Autorizadores>)_AutorizadoresService.ObtenerAutorizadores();
            return Ok(listaAutorizadores);
        }

        // GET api/<AutorizadoresController>/ConsultarAutorizadorPorId
        [HttpGet("ConsultarAutorizadorPorId")]
        public ActionResult<IEnumerable<Autorizadores>> ConsultarAutorizadorPorId(string id)
        {
            return Ok(_AutorizadoresService.ObtenerAutorizadoresId(id));
        }

        public ActionResult<IEnumerable<InformacionOdsDetalle>> ObtenerInformacionODSporId(string id_empleado)
        {
            return Ok(_odsService.ObtenerInformacionODSporId(id_empleado));
        }

        [HttpGet("ObtenerInformacionODS/{id_empleado}/{companyId}")]
        public ActionResult<IEnumerable<InformacionOdsDetalle>> ObtenerInformacionODS(string id_empleado, int companyId)
        {
            return Ok(_odsService.ObtenerInformacionODS(id_empleado, companyId));
        }

        // GET: api/<AutorizadoresController>/ConsultarAutorizadoresDistintos
        [HttpGet("ConsultarAutorizadoresDistintos")]
        public ActionResult<IEnumerable<AutorizadoresDistintos>> ConsultarAutorizadoresDistintos()
        {
            List<AutorizadoresDistintos> listaAutorizadores = (List<AutorizadoresDistintos>)_AutorizadoresService.ObtenerAutorizadoresDistintos();
            return Ok(listaAutorizadores);
        }

        [HttpGet("ObtenerEmpleadosPorNombre/{id_empleado}/{companyId}")]
        public async Task<IEnumerable<InformacionOdsDetalle>> ObtenerEmpleadosPorNombre(string Nombre)
        {
            try
            {
                IEnumerable<InformacionOdsDetalle> usuarios;

                usuarios = _odsService.ObtenerInformacionODSporNombre(Nombre).ToList();
                return usuarios;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
    }
}
