using Dapper;
using Reconocimientos.Interfaces;
using Reconocimientos.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Reconocimientos.Services
{
    public class ColaboradoresService : IColaboradoresService
    {
        readonly IConfiguration _config;
        readonly IDbConnection con;
        public ColaboradoresService(IConfiguration configuration)
        {
            _config = configuration;
            con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        }
        public Colaboradores GetColaboradorIdUnico(string id)
        {
            try
            {
                string query = _config["QuerysColaboradores:SelectColaboradoresIdUnico"];
                return con.Query<Colaboradores>(sql: query, new { Id_MGA_PlazasMH = id }).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Colaboradores GetColaboradorIdCorp(string idCorporativo)
        {
            try
            {
                string query = _config["QuerysColaboradores:SelectColaboradores"];
                return con.Query<Colaboradores>(sql: query, new { IdCorporativo = idCorporativo }).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Colaboradores> GetAreasColaborador()
        {
            try
            {
                string query = _config["QuerysColaboradores:SelectAreasColaboradores"];
                var result = (List<Colaboradores>)con.Query<Colaboradores>(sql: query);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Colaboradores> GetAllColaboradores()
        {
            try
            {
                string query = _config["QuerysColaboradores:SelectAllColaboradores"];
                var result = (List<Colaboradores>)con.Query<Colaboradores>(sql: query);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Colaboradores> GetAllColaboradoresByName(string nombre)
        {
            try
            {
                string query = _config["QuerysColaboradores:SelectAllColaboradoresByName"];
                var result = (List<Colaboradores>)con.Query<Colaboradores>(sql: query, new { n = "%" + nombre + "%" });
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Colaboradores> GetColaboradorPuestoId(int puestoId,int periodoPadreID)
        {
            try
            {
                List<Colaboradores> result = (List<Colaboradores>)con.Query<Colaboradores>(sql: _config["QuerysColaboradores:SelectAllColaboradoresByPuestoId"], new { PuestoId = puestoId });


                return (result);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
