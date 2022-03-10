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
    public class PuestosService : IPuestosService
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection con;

        public PuestosService(IConfiguration configuration)
        {
            _config = configuration;
            con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        }

        public IEnumerable<Puestos> ObtenerPuestos()
        {
            bool activo = true;
            try
            {
                var query = _config["QuerysPuestos:SelectPuestos"];
                return con.Query<Puestos>(query, new { Activo = activo });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Puestos> ObtenerPuestosIdPuesto(int id_puesto)
        {
            bool activo = true;
            try
            {
                var query = _config["QuerysPuestos:SelectPuestosIdPuesto"];
                return con.Query<Puestos>(query, new { IdPuesto = id_puesto, Activo = activo });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Puestos> ObtenerPuestosNombre(string nombre)
        {
            bool activo = true;
            try
            {
                var query = _config["QuerysPuestos:SelectPuestosNombre"];
                return con.Query<Puestos>(query, new { Nombre = nombre, Activo = activo });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int ActualizarPuestos(Puestos puesto)
        {
            try
            {
                var affectedRows = 0;
                var query = _config["QuerysPuestos:UpdatePuestos"];
                using (con)
                {
                    affectedRows = con.Execute(query, new
                    {
                        Id = puesto.id,
                        IdPuesto = puesto.puestoId,
                        Nombre = puesto.nombre,
                        Nivel = puesto.nivel,
                        Puntos = puesto.puntos,
                        Uen = puesto.uen,
                        Jerarquia= puesto.jerarquia,
                        Activo = Convert.ToInt32(puesto.activo)
                    });
                }

                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int EliminarPuestos(int id)
        {
            try
            {
                var affectedRows = 0;
                var query = _config["QuerysPuestos:DeletePuestos"];
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

        public int InsertarPuestos(Puestos puesto)
        {
            try
            {
                var affectedRows = 0;
                var query = _config["QuerysPuestos:InsertPuestos"];

                using (con)
                {
                    con.Open();

                    affectedRows = con.Execute(query,
                        new
                        {
                            IdPuesto = puesto.puestoId,
                            Nombre = puesto.nombre,
                            Nivel = puesto.nivel,
                            Puntos = puesto.puntos,
                            Uen = puesto.uen,
                            Jerarquia = puesto.jerarquia
                        });
                }

                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
