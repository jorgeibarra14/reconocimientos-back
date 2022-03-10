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
    public class EstatusPedidoService : IEstatusPedidoService
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection con;

        public EstatusPedidoService(IConfiguration configuration)
        {
            _config = configuration;
        }
        public IEnumerable<EstatusPedido> ObtenerAllEstatusPedido()
        {
            bool activo = true;
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysEstatusPedido:SelectAllEstatusPedido"];
                    return con.Query<EstatusPedido>(query, new { Activo = activo });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<EstatusPedido> ObtenerEstatusPedidoId(int id)
        {
            bool activo = true;
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysEstatusPedido:SelectEstatusPedidoId"];
                    return con.Query<EstatusPedido>(query, new { IdPedido = id, Activo = activo });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int InsertarEstatusPedido(EstatusPedido estatusPedido)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysEstatusPedido:InsertEstatusPedido"];

                        affectedRows = con.Execute(query,
                            new
                            {
                                idPedido = estatusPedido.id_pedido,
                                Estado = estatusPedido.estado
                            });
                   
                    return affectedRows;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int ActulizarEstatusPedido(EstatusPedido estatusPedido)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    affectedRows = con.Execute(_config["QuerysEstatusPedido:UpdateEstatusPedido"],
                        new
                        {
                            idPedido = estatusPedido.id_pedido,
                            Estado = estatusPedido.estado
                        });
                }
                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int EliminarEstatusPedido(int id)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                 affectedRows = con.Execute(_config["QuerysEstatusPedido:DeleteEstatusPedido"], new { IdPedido = id });
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