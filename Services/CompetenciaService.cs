using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Reconocimientos.Interfaces;
using Reconocimientos.Models;

namespace Reconocimientos.Services
{
    public class CompetenciaService : ICompetenciaService
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection con;

        public CompetenciaService(IConfiguration configuration)
        {
            _config = configuration;
            con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        }
        public IEnumerable<Competencias> ObtenerAllCompetencias()
        {
            bool activo = true;
            try
            {
                var query = _config["QuerysCompetencias:SelectAllCompetencias"];
                return con.Query<Competencias>(query, new { Activo = activo});
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Competencias> ObtenerCompetencias(bool activo, string nivel)
        {
            try
            {
                var query = _config["QuerysCompetencias:SelectCompetencias"];
                return con.Query<Competencias>(query, new { Activo = activo, Nivel = nivel });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Competencias> ObtenerCompetenciaId(int id)
        {
            bool activo = true;
            try
            {
                var query = _config["QuerysCompetencias:SelectCompetenciaId"];
                return con.Query<Competencias>(query, new { Id = id, Activo = activo });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int InsertarCompetencias(Competencias competencias)
        {
            try
            {
                var affectedRows = 0;
                var query = _config["QuerysCompetencias:InsertCompetencias"];

                using (con)
                {
                    con.Open();

                    affectedRows = con.Execute(query,
                        new
                        {
                            Nombre = competencias.nombre,
                            Descripcion = competencias.descripcion,
                            Nivel = competencias.nivel,
                            Img = competencias.img
                        });
                }

                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int ActulizarCompetencias(Competencias competencias)
        {
            try
            {
                var affectedRows = 0;
                var affectedRowsImg = 0;
                //var query = _config["QuerysCompetencias:UpdateCompetencias"];
                using (con)
                {
                    affectedRows = con.Execute(_config["QuerysCompetencias:UpdateCompetencias"],
                        new
                        {
                            Id = competencias.id,
                            Nombre = competencias.nombre,
                            Descripcion = competencias.descripcion,
                            Nivel = competencias.nivel,
                            Activo = Convert.ToInt32(competencias.activo)
                        });

                    affectedRowsImg = con.Execute(_config["QuerysCompetencias:UpdateCompetenciasImg"],
                        new
                        {
                            Nombre = competencias.nombre,
                            Img = competencias.img
                        });
                }

                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int EliminarCompetencias(int id)
        {
            try
            {
                var affectedRows = 0;
                var query = _config["QuerysCompetencias:DeleteCompetencias"];
                using (con)
                {
                    affectedRows = con.Execute(query, new { Id = id });
                }

                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<BussinessPractice> obtenerCompetenciasITGov()
        {
            var ITGovUrlApi = _config.GetSection("UrlApis").GetValue<string>("ITGovAPI");
            var Url = ITGovUrlApi + "/BussinessPractices";
            // Create a request for the URL.
            var request = WebRequest.CreateHttp(Url);

            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;

            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            var res = new List<BussinessPractice>();
            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                var stringRes = reader.ReadToEnd();
                res = JsonConvert.DeserializeObject<List<BussinessPractice>>(stringRes);
            }

            response.Close();
            return res;
        }
    }
}