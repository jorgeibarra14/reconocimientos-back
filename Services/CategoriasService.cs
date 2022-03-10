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
    public class CategoriasService : ICategoriasService
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection con;

        public CategoriasService(IConfiguration configuration)
        {
            _config = configuration;
        }
        public IEnumerable<Categorias> ObtenerAllCategorias()
        {
            bool activo = true;
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysCategorias:SelectAllCategorias"];
                    return con.Query<Categorias>(query, new { Activo = activo });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Categorias> ObtenerCategoriasId(int id)
        {
            bool activo = true;
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysCategorias:SelectCategoriasId"];
                    return con.Query<Categorias>(query, new { Id = id, Activo = activo });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int InsertarCategorias(Categorias categorias)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysCategorias:InsertCategorias"];

                        affectedRows = con.Execute(query,
                            new
                            {
                                Nombre = categorias.nombre,
                                Descripcion = categorias.descripcion,
                                Img = categorias.img
                            });
                   
                    return affectedRows;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int ActulizarCategorias(Categorias categorias)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    affectedRows = con.Execute(_config["QuerysCategorias:UpdateCategorias"],
                        new
                        {
                            Id = categorias.id,
                            Nombre = categorias.nombre,
                            Descripcion = categorias.descripcion,             
                            Activo = Convert.ToInt32(categorias.activo),
                            Img = categorias.img
                        });
                }
                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int EliminarCategorias(int id)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                 affectedRows = con.Execute(_config["QuerysCategorias:DeleteCategorias"], new { Id = id });
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