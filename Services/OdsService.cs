using Microsoft.Extensions.Configuration;
using Reconocimientos.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Reconocimientos.Models;
using System.Linq;
using System.Transactions;

namespace Reconocimientos.Services
{
    public class OdsService : IOdsService
    {
        private readonly IConfiguration _config;
        public OdsService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IEnumerable<InformacionOdsDetalle> ObtenerInformacionODSporId(string id_empleado)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    string query = _config["QuerysColaboradores:SelectMGA_PlazasMHid"];
                    return con.Query<InformacionOdsDetalle>(sql: query, new { Id = id_empleado });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public InformacionOdsDetalle ObtenerObjInformacionODSporId(string id_empleado)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    string query = _config["QuerysColaboradores:SelectMGA_PlazasMHid"];
                    return con.Query<InformacionOdsDetalle>(sql: query, new { Id = id_empleado }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<InformacionOdsDetalle> ObtenerInformacionODSporNombre(string nombre )
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    string query = _config["QuerysColaboradores:SelectMGA_PlazasMHNombre"];
                    return con.Query<InformacionOdsDetalle>(sql: query, new { Nombre = "%" + nombre + "%" });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<InformacionOdsDetalle> ObtenerInformacionODS()
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    string query = _config["QuerysColaboradores:SelectMGA_PlazasMH"];
                    return con.Query<InformacionOdsDetalle>(sql: query);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
