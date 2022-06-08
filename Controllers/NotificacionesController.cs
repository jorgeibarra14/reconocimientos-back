using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

namespace Reconocimientos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class NotificacionesController : ControllerBase
    {
        private readonly INotificacionesService _notificacionesService;
        private readonly IConfiguration _configuration;
        readonly IReconocimientoService _reconocimientoservice;
        private readonly ICompetenciaService _competenciaService;
        private readonly IOdsService _odsService;

        public NotificacionesController(INotificacionesService notificacionesService, IConfiguration configuration, IReconocimientoService reconocimientoservice, ICompetenciaService competenciaService, IOdsService odsService)
        {
            _notificacionesService = notificacionesService;
            _configuration = configuration;
            _reconocimientoservice = reconocimientoservice;
            _competenciaService = competenciaService;
            _odsService = odsService;
        }

        // POST api/<NotificacionesController>/AgregarNotificacion
        [HttpPost("AgregarNotificacion")]
        public IActionResult AgregarNotificacion([FromBody] Notificaciones notificacion)
        {
            return Ok(_notificacionesService.InsertarNotificacion(notificacion));
        }

        // POST api/<NotificacionesController>/EditarNotificacion
        [HttpPost("EditarNotificacion")]
        public IActionResult EditarNotificacion([FromBody] Notificaciones notificacion)
        {
            return Ok(_notificacionesService.ActualizarNotificacion(notificacion));
        }

        // POST api/<NotificacionesController>/EliminarNotificacion
        [HttpPost("EliminarNotificacion")]
        public IActionResult EliminarNotificacion(int id)
        {
            return Ok(_notificacionesService.EliminarNotificacion(id));
        }

        // GET: api/<NotificacionesController>/ConsultarNotificaciones
        [HttpGet("ConsultarNotificaciones")]
        public ActionResult<IEnumerable<Competencias>> ConsultarNotificaciones()
        {
            return Ok(_notificacionesService.ObtenerNotificaciones());
        }

        // GET api/<NotificacionesController>/ConsultarNotificacionesIdEmpleado
        [HttpGet("ConsultarNotificacionesIdEmpleado")]
        public ActionResult<IEnumerable<Competencias>> ConsultarNotificacionesIdEmpleado(string id_empleado)
        {
            return Ok(_notificacionesService.ObtenerNotificacionesIdEmpleado(id_empleado));
        }
        // GET api/<NotificacionesController>/MarcarComoLeidoIdEmpleado
        [HttpGet("MarcarComoLeidoIdEmpleado")]
        public IActionResult MarcarComoLeidoIdEmpleado(string id_empleado)
        {
            return Ok(_notificacionesService.MarcarComoLeidoIdEmpleado(id_empleado));
        }

        // POST api/<NotificacionesController>/EnviarNotificacion
        [HttpPost("EnviarNotificacion")]
        public async Task<IActionResult> EnviarNotificacion([FromBody] Notificaciones notificacion)
        {
            try
            {
                CorreoNotificacion correoNotificacion = new CorreoNotificacion();

                //List<Usuario> usuario = (List<Usuario>)await ObtenerEmpleadosPorId(notificacion.id_empleado.ToString());
                List<InformacionOdsDetalle> usuario = (List<InformacionOdsDetalle>)await ObtenerEmpleadosPorId(notificacion.id_empleado.ToString());

                //if (string.IsNullOrEmpty(usuario[0].Email))
                //{
                //    //return StatusCode((int)HttpStatusCode.OK, "No existe email registrado para notificar al empleado");
                //}
                //else
                //{
                    if (notificacion.titulo == "Denegado")
                    {
                        correoNotificacion.ToMail = new List<string>();
                        correoNotificacion.ToMail.Add(usuario[0].Email);

                        correoNotificacion.reconocimientorechazado = new ReconocimientoRechazado();
                        correoNotificacion.reconocimientorechazado.body = notificacion.descripcion;
                        correoNotificacion.reconocimientorechazado.nombre = usuario[0].NombreCompleto;

                        correoNotificacion.TipoEvento = "ReconociminetoRechazado";
                        correoNotificacion.Subject = notificacion.titulo;
                    }
                    else
                    {
                        List<Models.Reconocimientos> reconocimiento = (List<Models.Reconocimientos>)_reconocimientoservice.ObtenerReconocimientosId(notificacion.id_reconocimiento, true);

                        if (reconocimiento[0].id <= 0)
                        {
                            return StatusCode((int)HttpStatusCode.OK, "Reconocimiento no encontrado, no se envio notificación por correo");
                        }

                    List<CompetencyViewModel> competencia = (List<CompetencyViewModel>)_competenciaService.obtenerCompetenciasITGov().ToList();

                    var c = competencia.Find(c => c.Id == reconocimiento[0].id_competencia);

                    if (c.Id <= 0)
                        {
                            return StatusCode((int)HttpStatusCode.OK, "Competencia no encontrada, no se envio notificación por correo");
                        }

                        correoNotificacion.ToMail = new List<string>();
                        correoNotificacion.ToMail.Add(usuario[0].Email);

                        correoNotificacion.reconocimientoaprobado = new ReconocimientoAprobado();
                        correoNotificacion.reconocimientoaprobado.competencia_id = c.Id.ToString();
                        correoNotificacion.reconocimientoaprobado.competencia_nombre = c.Name;
                        correoNotificacion.reconocimientoaprobado.competencia_descripcion = c.Description;
                        correoNotificacion.reconocimientoaprobado.recibe = usuario[0].NombreCompleto;

                        correoNotificacion.reconocimientoaprobado.nombre_quien_envia = notificacion.descripcion;
                        correoNotificacion.reconocimientoaprobado.comentario = reconocimiento[0].motivo + " y " + reconocimiento[0].logro;

                        correoNotificacion.TipoEvento = "ReconociminetoAprobado";
                        correoNotificacion.Subject = notificacion.titulo;

                    }
                //}

                return Ok(_notificacionesService.EnviarNotificacion(correoNotificacion));
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        public Task<IEnumerable<InformacionOdsDetalle>> ObtenerEmpleadosPorId(string Id)
         {
            try
            {
                //string[] empleadosId = new string[1];
                //empleadosId[0] = Id;

                //var UrlApi = _configuration.GetSection("UrlApis").GetValue<string>("ITGovAPI");
                //var Url = UrlApi + "/user/byemployeesid";

                IEnumerable<InformacionOdsDetalle> usuarios;
                //using (HttpClient client = new HttpClient())
                //{
                //    var itgovResponse = await client.PostAsJsonAsync(Url, new { Users = empleadosId.Select(x => new { Id = x }) });
                //    usuarios = JsonConvert.DeserializeObject<List<Usuario>>(await itgovResponse.Content.ReadAsStringAsync());


                usuarios = _odsService.ObtenerInformacionODSporId(Id).ToList();

                //foreach (InformacionOdsDetalle item in usuarios)
                //{
                //BaseUser user = new BaseUser();
                //user = GetUserByID(item.Id);
                //if (user == null)
                //{
                //item.Foto = "../../../assets/img/avatar.png";
                //}
                //else if (!string.IsNullOrEmpty(user.Foto))
                //{
                //    item.Foto = user.Foto;
                //}
                //}
                //}
                return Task.FromResult(usuarios);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        //public static BaseUser GetUserByID(int userID)
        //{
        //    BaseUser user = null;
        //    using (DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://h1dcgdl.nhelios.com.mx"))
        //    {
        //        using (DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry))
        //        {
        //            directorySearcher.Filter = string.Format("(initials=" + userID + ")");
        //            SearchResult searchResult = directorySearcher.FindOne();
        //            if (searchResult != null)
        //            {
        //                DirectoryEntry ad = searchResult.GetDirectoryEntry();

        //                object objData = ad.Properties["objectguid"].Value;
        //                byte[] binaryData = objData as byte[];
        //                Guid guid = new Guid(binaryData);

        //                user = new BaseUser()
        //                {
        //                    Id = ad.Properties.Contains("initials") ? int.Parse(ad.Properties["initials"].Value.ToString()) : 0,
        //                    UserName = ad.Properties.Contains("sAMAccountName") ? ad.Properties["sAMAccountName"].Value.ToString() : string.Empty,
        //                    Nombre = ad.Properties.Contains("givenname") ? ad.Properties["givenname"].Value.ToString() : string.Empty,
        //                    Apellidos = ad.Properties.Contains("sn") ? ad.Properties["sn"].Value.ToString() : string.Empty,
        //                    Empresa = ad.Properties.Contains("company") ? ad.Properties["company"].Value.ToString() : string.Empty,
        //                    Mail = ad.Properties.Contains("mail") ? ad.Properties["mail"].Value.ToString() : string.Empty,
        //                    Puesto = ad.Properties.Contains("title") ? ad.Properties["title"].Value.ToString() : string.Empty,
        //                    Departamento = ad.Properties.Contains("department") ? ad.Properties["department"].Value.ToString() : string.Empty,
        //                    Telefono = ad.Properties.Contains("telephoneNumber") ? ad.Properties["telephoneNumber"].Value.ToString() : string.Empty,
        //                    Mobile = ad.Properties.Contains("mobile") ? ad.Properties["mobile"].Value.ToString() : string.Empty,
        //                    Foto = ad.Properties.Contains("thumbnailPhoto") ? "data:image/jpeg;base64," + System.Convert.ToBase64String(ad.Properties["thumbnailPhoto"].Value as byte[]) : string.Empty
        //                };
        //            }
        //        }
        //    }

        //    return user;
        //}
    }
}
