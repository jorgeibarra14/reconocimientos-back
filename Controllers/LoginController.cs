using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ITGovApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Reconocimientos.Interfaces;
using Reconocimientos.Models;
using Reconocimientos.Utilities;

namespace Reconocimientos.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService,
            IConfiguration configuration)
        {
            _loginService = loginService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("authenticate")]
        public ActionResult<dynamic> ValidateUser()
        {
            try
            {
                var RolId = User.Identity.ObtenerRolId();

                var user = new User
                {
                    Id = /*User.Identity.ObtenerId()*/ "",
                    Nombre = User.Identity.ObtenerNombre(),
                    Apellidos = User.Identity.ObtenerApellidos(),
                    Puesto = User.Identity.ObtenerPuesto()
                };

                return user;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("startup")]
        public async Task<ActionResult> Startup(ITGovLogin login)
        {
            try
            {
                ApplicationConfiguration appConfig;

                var ReconocimientosUrlApp = _configuration.GetSection("UrlApps").GetValue<string>("ReconocimientosApp");
                var ITGovUrlApi = _configuration.GetSection("UrlApis").GetValue<string>("ITGovAPI");
                var Url = ITGovUrlApi + "/user/itgov";
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient client = new HttpClient(clientHandler);
                var request = (HttpWebRequest) WebRequest.CreateHttp(Url);
                request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                request.Method = "POST";
                request.Headers.Add("Authorization", "Bearer " + login.Token);
                request.ContentType = "application/json";

                var itGovRequest = new {AppId = login.ApplicationId, UserId = login.UserId};

                var itGovJson = JsonConvert.SerializeObject(itGovRequest);

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(itGovJson);
                }

                HttpWebResponse response;
                try
                {
                    response = (HttpWebResponse) request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK) {

                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var json = streamReader.ReadToEnd();
                            UserViewModel user = JsonConvert.DeserializeObject<UserViewModel>(json);
                            Role r = new Role
                            {
                                Id = 1,
                                Name = "Admin"
                            };
                            ItGovUser usr = new ItGovUser
                            {
                                Nombre = user.Name,
                                Paterno = user.FirstName,
                                Materno = user.LastName,
                                Email = user.Email,
                                Puesto = user.JobTitle,
                                NivelPuesto = user.JobLevel,
                                Id = user.UserId,
                                Role = r,
                                IsAdminAck = user.IsAdminAck,
                                IsAdminStore = user.IsAdminStore,
                                AppId = int.Parse(login.ApplicationId),
                                Avatar = user.Avatar

                            };

                            ApplicationConfiguration a = new ApplicationConfiguration
                            {
                                Usuario = usr,
                                Token = ""
                            };

                            appConfig = a;
                        }
                    }
                    else {
                        throw new Exception("Server responded with Status Code " + response.StatusCode + ": " +
                                            response.StatusDescription);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error en el servicio.", ex);
                }

                if (appConfig.Usuario == null)
                    return NotFound(new {errorMessage = "No se encontró el usuario para esta aplicación."});
                string encodedAvatar = string.Empty;
                if(appConfig.Usuario.Avatar != null)
                    encodedAvatar = Base64Encode(appConfig.Usuario.Avatar);
                var claims = new List<Claim>
                {
                    new Claim("AppId", appConfig.Usuario.AppId.ToString() ?? ""),
                    new Claim("Avatar", ""),
                    new Claim("Id", appConfig.Usuario.Id ?? ""),
                    new Claim("Nombre", appConfig.Usuario.Nombre ?? ""),
                    new Claim("Paterno", appConfig.Usuario.Paterno ?? ""),
                    new Claim("Materno", appConfig.Usuario.Materno ?? ""),
                    new Claim("Rol", appConfig.Usuario.Role.Name ?? ""),
                    new Claim("RolId", appConfig.Usuario.Role.Id.ToString() ?? ""),
                    new Claim("Email", appConfig.Usuario.Email ?? ""),
                    new Claim("Puesto", appConfig.Usuario.Puesto ?? ""),
                    new Claim("NivelPuesto", appConfig.Usuario.NivelPuesto ?? ""),
                    new Claim("IsAdminAck", appConfig.Usuario.IsAdminAck.ToString()),
                    new Claim("IsAdminStore", appConfig.Usuario.IsAdminStore.ToString()),



                };

                var token = string.Empty;
                try
                {
                    token = GenerateJSONWebToken(appConfig.Usuario, claims);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                var url = ReconocimientosUrlApp + "?access_token=" + token;

                return Ok(new {link = url});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public static string Base64Encode(string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        [HttpGet]
        [Route("redirecting")]
        public IActionResult Redirect()
        {
            var ITGovUrl = _configuration.GetSection("UrlApps").GetValue<string>("ITGovApp");
            return Redirect($"{ITGovUrl}/Login/Index?applicationName=Reconocimientos");
        }

        private string GenerateJSONWebToken(ItGovUser userInfo, List<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}