using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Reconocimientos.Interfaces;
using Reconocimientos.Models;

namespace Reconocimientos.Services
{
    public class PuntosService : IPuntosService
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection con;

        public PuntosService(IConfiguration configuration)
        {
            _config = configuration;
            con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        }

        public int InsertarPuntos(Puntos puntos)
        {
            try
            {
                var affectedRows = 0;
                var query = _config["QuerysPuntos:InsertPuntos"];

                using (con)
                {
                    con.Open();

                    affectedRows = con.Execute(query,
                        new
                        {
                            IdEmpleado = puntos.id_empleado, 
                            Puntos = puntos.puntos, 
                            Periodo = puntos.periodo
                        });
                }

                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int ActulizarPuntos(Puntos puntos)
        {
            try
            {
                var affectedRows = 0;
                var query = _config["QuerysPuntos:UpdatePuntos"];
                using (con)
                {
                    affectedRows = con.Execute(query, new
                    {
                        Id = puntos.id, IdEmpleado = puntos.id_empleado, Puntos = puntos.puntos,
                        Periodo = puntos.periodo, Activo = Convert.ToInt32(puntos.activo)
                    });
                }

                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int ActualizarPuntosReconocimiento(Puntos puntos)
        {
            try
            {
                var affectedRows = 0;
                string query = _config["QuerysPuntos:DescontarPuntos"];
                using (con)
                {
                    affectedRows = con.Execute(query, new
                    {
                        IdEmpleado = puntos.id_empleado,
                        Puntos = puntos.puntos,
                        Activo = Convert.ToInt32(puntos.activo)
                    });
                }
                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int EliminarPuntos(int id)
        {
            try
            {
                var affectedRows = 0;
                var query = _config["QuerysPuntos:DeletePuntos"];
                using (con)
                {
                    affectedRows = con.Execute(query, new {Id = id});
                }

                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Puntos> ObtenerPuntos(string id_empleado)
        {
            bool activo = true;

            try
            {
                var query = _config["QuerysPuntos:SelectPuntos"];
                return con.Query<Puntos>(query, new {Activo = activo, IdEmpleado = id_empleado});
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int PuntosDisponibles(string id_empleado, bool activo)
        {
            try
            {
                var query = _config["QuerysPuntos:SelectPuntosId"];
                return con.ExecuteScalar<int>(query, new {IdEmpleado = id_empleado, Activo = activo});
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int CorteManualDePuntos()
        {
            try
            {
                var affectedRows = 0;
                using (con)
                {
                    affectedRows = con.Execute(_config["QuerysPuntos:UpdateOldPuntos"]);               
                    affectedRows = con.Execute(_config["QuerysPuntos:InsertNewPuntos"]);
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