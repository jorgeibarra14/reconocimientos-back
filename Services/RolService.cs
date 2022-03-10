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
    public class RolService : IRolService
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection con;

        public RolService(IConfiguration configuration)
        {
            _config = configuration;
            con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        }

        public IEnumerable<Roles> ObtenerRoles(bool activo)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysRoles:SelectRol"];
                    return con.Query<Roles>(query, new { Activo = activo });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Roles> ObtenerRolId(int id, bool activo)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysRoles:SelectRolId"];
                    return con.Query<Roles>(query, new { Id = id, Activo = activo });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int ActualizarRol(Roles rol)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    affectedRows = con.Execute(_config["QuerysRoles:UpdateRol"], new
                    {
                        Id = rol.id,
                        Nombre = rol.nombre,
                        Descripcion = rol.descripcion,
                        Activo = Convert.ToInt32(rol.activo)
                    });

                    return affectedRows;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int EliminarRol(int id)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    affectedRows = con.Execute(_config["QuerysRoles:DeleteRol"], new { Id = id });
                    return affectedRows;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int InsertarRol(Roles rol)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    affectedRows = con.Execute(_config["QuerysRoles:InsertRol"],
                        new
                        {
                            Nombre = rol.nombre,
                            Descripcion = rol.descripcion,
                            Activo = Convert.ToInt32(rol.activo)
                        });

                    return affectedRows;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
