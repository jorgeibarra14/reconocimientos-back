using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Reconocimientos.Interfaces;
using Reconocimientos.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DirectoryEntry = System.DirectoryServices.DirectoryEntry;

namespace Reconocimientos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ReconocimientoController : Controller
    {
        private readonly IReconocimientoService _reconocimientoservice;
        private readonly IConfiguration _configuration;
        private readonly IUsuarioRolService _usuarioRolService;
        private readonly IPuestosService _puestosService;
        private readonly IOdsService _odsService;
        private readonly ICompetenciaService _competenciaService;
        private readonly IPuntosService _puntoService;
        private string rolAdministrador = "Administrador";
        private string rolSuperAdministrador = "Superadministrador";

        public ReconocimientoController(IReconocimientoService reconocimientoservice,
            IConfiguration configuration,
            IUsuarioRolService usuarioRolService,
            IPuestosService puestosService,
            IOdsService odsService,
            ICompetenciaService competenciaService,
            IPuntosService puntoService
            )
        {
            _reconocimientoservice = reconocimientoservice;
            _configuration = configuration;
            _usuarioRolService = usuarioRolService;
            _puestosService = puestosService;
            _odsService = odsService;
            _competenciaService = competenciaService;
            _puntoService = puntoService;
        }

        // GET api/<ReconocimientoController>/ObtenerPuntosAcumulados
        [HttpGet("ObtenerPuntosAcumulados")]
        public int ObtenerPuntosAcumulados(string id_empleado_recibe, bool activo)
        {
            return _reconocimientoservice.ObtenerPuntosAcumulados(id_empleado_recibe, activo);
        }

        // POST api/<ReconocimientoController>/Reconocer
        [HttpPost("Reconocer")]
        public async Task<IActionResult> Reconocer([FromBody] Models.Reconocimientos reconocimiento)
        {
            try
            {
                //List<InformacionOdsDetalle> listaId = (List<InformacionOdsDetalle>)await ObtenerEmpleadosPorNombre(reconocimiento.id_empleado_recibe.ToString());
                List<Puntos> listaPuntos = (List<Puntos>)_puntoService.ObtenerPuntos(reconocimiento.id_empleado_envia);
                //if (listaId.Count > 0)
                //{
                //    reconocimiento.id_empleado_recibe = listaId[0].Id_MGA_PlazasMH.ToString();
                //}
                //else
                //{
                //    return BadRequest();
                //}
                

                if (listaPuntos.Count > 0)
                {
                    reconocimiento.id_puntos = listaPuntos[0].id;
                }
                else
                {
                    return BadRequest();
                }

                int contador = _reconocimientoservice.ValidarReconociminetoEntregado(reconocimiento.id_empleado_recibe, reconocimiento.id_empleado_envia, true);
                if (contador > 0)
                {
                    return StatusCode((int)HttpStatusCode.OK, "Rechazo por sistema");
                }
                else
                {
                    List<InformacionOdsDetalle> listaInformacionOdsDetalle = new List<InformacionOdsDetalle>();
                    listaInformacionOdsDetalle = (List<InformacionOdsDetalle>)_odsService.ObtenerInformacionODSporId(reconocimiento.id_empleado_recibe);

                    if (listaInformacionOdsDetalle.Count > 0)
                    {
                        //int idAutorizador = _reconocimientoservice.ObtenerAutorizador(listaInformacionOdsDetalle[0].Area, listaInformacionOdsDetalle[0].Sistema, listaInformacionOdsDetalle[0].Regional);
                        reconocimiento.id_empleado_autorizador = "39a5a798-7581-4094-b321-b12cca846428";
                    }
                    else
                    {
                        reconocimiento.id_empleado_autorizador = "";
                    }
                    var usuarioPuntos = new UsuariosPuntos{ IdEmpleado = reconocimiento.id_empleado_recibe, Valor = 1, Tipo = "Reconocimiento", IdPedido=0};
                    
                    _reconocimientoservice.InsertarPuntos(usuarioPuntos);
                    return Ok(_reconocimientoservice.InsertarReconocimiento(reconocimiento));
                }

            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        // POST api/<ReconocimientoController>/AprobarReconocimiento
        [HttpPost("AprobarReconocimiento")]
        public IActionResult AprobarReconocimiento([FromBody] Models.Reconocimientos reconocimiento)
        {
            return Ok(_reconocimientoservice.AprobarRechazarReconocimiento(reconocimiento));
        }

        // POST api/<ReconocimientoController>/RechazarReconocimiento
        [HttpPost("RechazarReconocimiento")]
        public IActionResult RechazarReconocimiento([FromBody] Models.Reconocimientos reconocimiento)
        {
            return Ok(_reconocimientoservice.AprobarRechazarReconocimiento(reconocimiento));
        }

        // POST api/<ReconocimientoController>/EliminarReconocimiento
        [HttpPost("EliminarReconocimiento")]
        public IActionResult EliminarReconocimiento(int id)
        {
            return Ok(_reconocimientoservice.EliminarReconocimiento(id));
        }

        // POST api/<ReconocimientoController>/EditarReconocimiento
        [HttpPost("EditarReconocimiento")]
        public IActionResult EditarReconocimiento([FromBody] Models.Reconocimientos reconocimiento)
        {
            List<Models.Reconocimientos> listaReconocimiento = (List<Models.Reconocimientos>)_reconocimientoservice.ObtenerReconocimientosId(reconocimiento.id, true);

            if (listaReconocimiento[0].id <= 0)
            {
                return StatusCode((int)HttpStatusCode.OK, "Reconocimiento no encontrado");
            }

            return Ok(_reconocimientoservice.ActualizarReconocimiento(reconocimiento));
        }

        // GET: api/<ReconocimientoController>/ObtenerReconocimientos
        [HttpGet("ObtenerReconocimientos")]
        public ActionResult<IEnumerable<Models.Reconocimientos>> ObtenerReconocimientos()
        {
            return Ok(_reconocimientoservice.ObtenerReconocimientos());
        }

        // GET: api/<ReconocimientoController>/ObtenerReconocimientosPorAutorizar
        [HttpGet("ObtenerReconocimientosPorAutorizar")]
        public ActionResult<IEnumerable<ReconocimientosPorAutorizar>> ObtenerReconocimientosPorAutorizar(string id_empleado_autorizador, bool activo)
        {
            List<ReconocimientosPorAutorizar> listaReconocimientoPorAutorizar = new List<ReconocimientosPorAutorizar>();

            List<UsuariosRoles> usuarioRolResult = (List<UsuariosRoles>)_usuarioRolService.ObtenerUsuarioRolIdEmpleado(id_empleado_autorizador);

            string nombreRol = "";

            if (usuarioRolResult.Count > 0)
            {
                nombreRol = usuarioRolResult[0].nombre;
            }

            if (nombreRol == rolAdministrador || nombreRol == rolSuperAdministrador)
            {
                listaReconocimientoPorAutorizar = (List<ReconocimientosPorAutorizar>)_reconocimientoservice.ObtenerReconocimientosPorAutorizarAdmin(activo);
            }
            else
            {
                listaReconocimientoPorAutorizar = (List<ReconocimientosPorAutorizar>)_reconocimientoservice.ObtenerReconocimientosPorAutorizar(id_empleado_autorizador, activo);
            }

            if (listaReconocimientoPorAutorizar.Count() > 0)
            {
                foreach (ReconocimientosPorAutorizar reconocimientoPorAutorizarItem in listaReconocimientoPorAutorizar)
                {
                    InformacionOdsDetalle info_empleado_envia = _odsService.ObtenerObjInformacionODSporId(reconocimientoPorAutorizarItem.id_empleado_envia);
                    if (info_empleado_envia != null)
                        reconocimientoPorAutorizarItem.enviadoPor = info_empleado_envia.NombreCompleto;
                    else
                        reconocimientoPorAutorizarItem.enviadoPor = "";

                    InformacionOdsDetalle info_empleado_recibe = _odsService.ObtenerObjInformacionODSporId(reconocimientoPorAutorizarItem.id_empleado_recibe);
                    if (info_empleado_recibe != null)
                        reconocimientoPorAutorizarItem.reconoceA = info_empleado_recibe.NombreCompleto;
                    else
                        reconocimientoPorAutorizarItem.reconoceA = "";

                    InformacionOdsDetalle info_empleado_autorizador = _odsService.ObtenerObjInformacionODSporId(reconocimientoPorAutorizarItem.id_empleado_autorizador);
                    if (info_empleado_autorizador != null)
                        reconocimientoPorAutorizarItem.autorizador = info_empleado_autorizador.NombreCompleto;
                    else
                        reconocimientoPorAutorizarItem.autorizador = "";
                }
            }

            return Ok(listaReconocimientoPorAutorizar);
        }

        // GET: api/<ReconocimientoController>/ObtenerMisReconocimientos
        [HttpGet("ObtenerMisReconocimientos")]
        public ActionResult<IEnumerable<MisReconocimientos>> ObtenerMisReconocimientos(string id_empleado_recibe, bool activo, string nombrePuesto)
        {
            List<MisReconocimientos> misReconocimientos = (List<MisReconocimientos>)_reconocimientoservice.MisReconocimientos(id_empleado_recibe, activo);
            List<InformacionOdsDetalle> empleado = _odsService.ObtenerInformacionODSporId(id_empleado_recibe).ToList();
            List<Competencias> listaCompetencoas = (List<Competencias>)_competenciaService.ObtenerCompetencias(activo, empleado[0].NivelPuesto);

            foreach (MisReconocimientos item in misReconocimientos)
            {
                foreach (Competencias itemCompetencia in listaCompetencoas)
                {
                    if (item.nombre == itemCompetencia.nombre)
                    {

                        item.img = itemCompetencia.img;
                    }
                }
            }

            return Ok(misReconocimientos);
        }

        // GET: api/<ReconocimientoController>/ObtenerMisReconocimientosComp
        [HttpGet("ObtenerMisReconocimientosComp")]
        public async Task<ActionResult<IEnumerable<MisReconocimientosDetalle>>> ObtenerMisReconocimientosComp(string id_empleado_recibe, string nombreCompetencia, bool activo)
        {
            List<MisReconocimientosDetalle> listaDetalle = (List<MisReconocimientosDetalle>)_reconocimientoservice.MisReconocimientosPorCompetencia(id_empleado_recibe, nombreCompetencia, activo);
            if (listaDetalle.Count() > 0)
            {

                foreach (MisReconocimientosDetalle item in listaDetalle)
                {
                    List<InformacionOdsDetalle> listaId = await ObtenerEmpleadosPorId(item.id.ToString());

                    if (listaId.Count > 0)
                    {
                        item.nombre = listaId[0].NombreCompleto;
                    }
                }
            }

            return Ok(listaDetalle);
        }

        // GET: api/<ReconocimientoController>/ObtenerReconocimientosEntregados
        [HttpGet("ObtenerReconocimientosEntregados")]
        public ActionResult<IEnumerable<ReconocimientosEntregados>> ObtenerReconocimientosEntregados(string id_empleado_envia, bool activo, string nombrePuesto)
        {
            List<ReconocimientosEntregados> reconocimientosentregados = (List<ReconocimientosEntregados>)_reconocimientoservice.ReconocerAOtros(id_empleado_envia, activo);
            List<InformacionOdsDetalle> empleado = _odsService.ObtenerInformacionODSporId(id_empleado_envia).ToList();
            List<Competencias> listaCompetencias = (List<Competencias>)_competenciaService.ObtenerCompetencias(activo, empleado[0].NivelPuesto);

            foreach (ReconocimientosEntregados item in reconocimientosentregados)
            {
                foreach (Competencias itemCompetencia in listaCompetencias)
                {
                    if (item.nombre == itemCompetencia.nombre)
                    {

                        item.img = itemCompetencia.img;
                    }
                }
            }

            return Ok(reconocimientosentregados);
        }

        // GET: api/<ReconocimientoController>/ObtenerReconocimientosEntregadosComp
        [HttpGet("ObtenerReconocimientosEntregadosComp")]
        public async Task<ActionResult<IEnumerable<ReconocimientosEntregadosDetalles>>> ObtenerReconocimientosEntregadosComp(string id_empleado_envia, string nombreCompetencia, bool activo)
        {
            List<ReconocimientosEntregadosDetalles> listaDetalle = (List<ReconocimientosEntregadosDetalles>)_reconocimientoservice.ReconocerAOtrosPorCompetencia(id_empleado_envia, nombreCompetencia, activo);
            if (listaDetalle.Count() > 0)
            {
                foreach (ReconocimientosEntregadosDetalles item in listaDetalle)
                {
                    List<InformacionOdsDetalle> listaId = await ObtenerEmpleadosPorId(item.id.ToString());

                    if (listaId.Count > 0)
                    {
                        item.nombre = listaId[0].NombreCompleto;
                        //item.img = listaId[0].Foto;
                    }
                }
            }

            return Ok(listaDetalle);
        }

        [HttpGet("ObtenerEmpleadosPorId")]
        public Task<List<InformacionOdsDetalle>> ObtenerEmpleadosPorId(string Id)
            {
            try
            {
                List<InformacionOdsDetalle> usuarios;
                usuarios = _odsService.ObtenerInformacionODSporId(Id).ToList();
                return Task.FromResult(usuarios);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        [HttpGet("ObtenerEmpleadosPorNombre")]
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

        [HttpGet("ObtenerEmpleadosReconocer")]
        public async Task<IEnumerable<InformacionOdsDetalle>> ObtenerEmpleadosReconocer(string Nombre, string id_empleado_envia)
        {
            try
            {
                List<InformacionOdsDetalle> usuarios = new List<InformacionOdsDetalle>();
                if (Nombre != "undefined")
                {
                    usuarios = _odsService.ObtenerInformacionODSporNombre(Nombre).ToList();
                    usuarios = usuarios.Where(x => x.Id_MGA_PlazasMH != id_empleado_envia).ToList();
                }
                return await Task.FromResult(usuarios);

            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        // GET api/<ReconocimientoController>/ValidarReconociminetoEntregado
        [HttpGet("ValidarReconociminetoEntregado")]
        public async Task<IActionResult> ValidarReconociminetoEntregado(string pId_empleado_recibe, string id_empleado_envia, bool activo)
        {
            //List<InformacionOdsDetalle> listaId = (List<InformacionOdsDetalle>)await ObtenerEmpleadosPorNombre(pId_empleado_recibe);
            //string id_empleado_recibe = 0;
            //if (listaId.Count > 0)
            //{
            //    id_empleado_recibe = listaId[0].Id;
            //}

            return Ok(_reconocimientoservice.ValidarReconociminetoEntregado(pId_empleado_recibe, id_empleado_envia, activo));
        }

        [HttpGet("ObtenerTopReconocidos")]
        public IActionResult ObtenerTopReconocidos()
        {
            return Ok( _reconocimientoservice.ObtenerTopReconocidos());
        }
    }
}
