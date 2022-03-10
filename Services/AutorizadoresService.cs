using Dapper;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Utils;
using Reconocimientos.Interfaces;
using Reconocimientos.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace Reconocimientos.Services
{
    public class AutorizadoresService : IAutorizadoresService
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection con;

        public AutorizadoresService(IConfiguration configuration)
        {
            _config = configuration;
            con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        }

        public IEnumerable<Autorizadores> ObtenerAutorizadores()
        {
            bool activo = true;
            try
            {
                var query = _config["QuerysAutorizadores:SelectAutorizadores"];
                return con.Query<Autorizadores>(query, new { Activo = activo });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Autorizadores> ObtenerAutorizadoresId(string id)
        {
            bool activo = true;
            try
            {
                var query = _config["QuerysAutorizadores:SelectAutorizadoresId"];
                return con.Query<Autorizadores>(query, new { Id = id, Activo = activo });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public int ActualizarAutorizadores(Autorizadores autorizadores)
        {
            try
            {
                var affectedRows = 0;
                var query = _config["QuerysAutorizadores:UpdateAutorizadores"];
                using (con)
                {
                    affectedRows = con.Execute(query, new
                    {
                        Id = autorizadores.id,
                        Area = autorizadores.area,
                        Region = autorizadores.region,
                        Sistema = autorizadores.sistema,
                        IdEmpleadoAutorizador = autorizadores.idempleadoautorizador,
                        Uen = autorizadores.uen,
                        Activo = Convert.ToInt32(autorizadores.activo)
                    });
                }

                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int EliminarAutorizadores(int id)
        {
            try
            {
                var affectedRows = 0;
                var query = _config["QuerysAutorizadores:DeleteAutorizadores"];
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

        public int InsertarAutorizadores(Autorizadores autorizadores)
        {
            try
            {
                var affectedRows = 0;
                var query = _config["QuerysAutorizadores:InsertAutorizadores"];

                using (con)
                {
                    con.Open();

                    affectedRows = con.Execute(query,
                        new
                        {
                            Id = autorizadores.id,
                            Area = autorizadores.area,
                            Region = autorizadores.region,
                            Sistema = autorizadores.sistema,
                            IdEmpleadoAutorizador = autorizadores.idempleadoautorizador,
                            Uen = autorizadores.uen,
                        });
                }

                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<AutorizadoresDistintos> ObtenerAutorizadoresDistintos()
        {
            bool activo = true;
            try
            {
                var query = _config["QuerysAutorizadores:SelectAutorizadoresDistintos"];
                return con.Query<AutorizadoresDistintos>(query, new { Activo = activo });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
