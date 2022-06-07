using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using Reconocimientos.Interfaces;
using Reconocimientos.Models;

namespace Reconocimientos.Services
{
    public class PedidosService : IPedidosService
    {
        private readonly IConfiguration _config;
        private readonly IProductosPedidoService _productosPedidosService;
        private readonly IEstatusPedidoService _estatusPedidosService;
        private readonly IProductosService _productosService;

        public PedidosService(IConfiguration configuration,
             IProductosPedidoService productosPedidosService,
             IProductosService productosService,
             IEstatusPedidoService estatusPedidosService)
        {
            _config = configuration;
            _productosPedidosService = productosPedidosService;
            _productosService = productosService;
            _estatusPedidosService = estatusPedidosService;
        }
        public IEnumerable<Pedidos> ObtenerAllPedidos()
        {
            bool activo = true;
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysPedidos:SelectAllProductosPedidoNuevo"];

                    var result = con.Query<Pedidos>(query, new { Activo = activo }).ToList();

                    // foreach (Pedidos item in result)
                    // {
                    //     var resultProductos = (List<ProductosPedido>)con.Query<ProductosPedido>(sql: _config["QuerysProductosPedido:SelectProductosPedidoId"],
                    //         new { IdPedido = item.id, Activo = activo });
                    //
                    //     item.productos = resultProductos;
                    //
                    //     List<EstatusPedido> resultEsatusPedido = (List<EstatusPedido>)con.Query<EstatusPedido>(sql: _config["QuerysEstatusPedido:SelectEstatusPedidoId"],
                    //         new { IdPedido = item.id, Activo = activo });
                    //
                    //     item.estatusPedido = new EstatusPedido()
                    //     {
                    //         id = resultEsatusPedido[0].id,
                    //         id_pedido = resultEsatusPedido[0].id_pedido,
                    //         estado = resultEsatusPedido[0].estado,
                    //         activo = resultEsatusPedido[0].activo,
                    //         fecha_creacion = resultEsatusPedido[0].fecha_creacion
                    //     };
                    // }

                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Pedidos> ObtenerPedidosId(int id)
        {
            bool activo = true;
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysPedidos:SelectPedidosId"];
                    var result = (List<Pedidos>)con.Query<Pedidos>(query, new { Id = id, Activo = activo });

                    foreach (Pedidos item in result)
                    {
                        var resultProductos = (List<ProductosPedido>)con.Query<ProductosPedido>(sql: _config["QuerysProductosPedido:SelectProductosPedidoId"],
                            new { IdPedido = item.id, Activo = activo });

                        item.productos = resultProductos;

                        EstatusPedido resultEsatusPedido = (EstatusPedido)con.Query<EstatusPedido>(sql: _config["QuerysEstatusPedido:SelectEstatusPedidoId"],
                            new { IdPedido = item.id, Activo = activo });

                        item.estatusPedido = new EstatusPedido()
                        {
                            id = resultEsatusPedido.id,
                            id_pedido = resultEsatusPedido.id_pedido,
                            estado = resultEsatusPedido.estado,
                            activo = resultEsatusPedido.activo,
                            fecha_creacion = resultEsatusPedido.fecha_creacion
                        };
                    }

                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Pedidos> ObtenerPedidosSolicitante(int id_solicitante)
        {
            bool activo = true;
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysPedidos:SelectPedidosIdSolicitante"];
                    var result = (List<Pedidos>)con.Query<Pedidos>(query, new { IdSolicitante = id_solicitante, Activo = activo });

                    foreach (Pedidos item in result)
                    {
                        var resultProductos = (List<ProductosPedido>)con.Query<ProductosPedido>(sql: _config["QuerysProductosPedido:SelectProductosPedidoId"],
                            new { IdPedido = item.id, Activo = activo });

                        item.productos = resultProductos;

                        EstatusPedido resultEsatusPedido = (EstatusPedido)con.Query<EstatusPedido>(sql: _config["QuerysEstatusPedido:SelectEstatusPedidoId"],
                            new { IdPedido = item.id, Activo = activo });

                        item.estatusPedido = new EstatusPedido()
                        {
                            id = resultEsatusPedido.id,
                            id_pedido = resultEsatusPedido.id_pedido,
                            estado = resultEsatusPedido.estado,
                            activo = resultEsatusPedido.activo,
                            fecha_creacion = resultEsatusPedido.fecha_creacion
                        };
                    }

                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int InsertarPedidos(Pedidos pedido)
        {
            try
            {
                var resultado = 0;
                int newId = 0;
                string query = _config["QuerysPedidos:InsertPedidos"];

                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var result = con.ExecuteScalar(query + " SELECT SCOPE_IDENTITY()",
                         new
                         {
                             IdSolicitante = pedido.id_solicitante,
                             NombreSolicitante = pedido.nombre_solicitante,
                             PuestoSolicitante = pedido.puesto_solicitante,
                             AreaSolicitante = "TI",
                             SistemaSolicitante = "URREA",
                             IdAutorizador = pedido.id_autorizador,
                             NombreAutorizador = pedido.nombre_autorizador
                         });

                    if (result != null)
                    {
                        newId = Convert.ToInt32(result);

                        foreach (ProductosPedido producto in pedido.productos)
                        {
                            producto.id_pedido = newId;
                            resultado = _productosPedidosService.InsertarProductosPedido(producto);

                            //Descontar stock
                            if (resultado ==1)
                            {
                                List<Productos> Stockproducto = (List<Productos>)_productosService.getProductsById(producto.producto_id);

                                Stockproducto[0].stock = Stockproducto[0].stock - producto.cantidad;
                                resultado = _productosService.ActualizarStockProducto(Stockproducto[0]);
                            }
                        }

                        EstatusPedido estatusPedido = new EstatusPedido();
                        estatusPedido.id_pedido = newId;
                        estatusPedido.estado = "Pendiente de autorización";

                        resultado = _estatusPedidosService.InsertarEstatusPedido(estatusPedido);

                        // RETORNA ID DEL PEDIDO
                        result = newId;
                    }
                    else
                    {
                        result = false;
                        throw new Exception("Error al procesar InsertPedidos, Contacte al administrador.");
                    }
                }
                return newId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int ActulizarPedidos(Pedidos pedidos)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    affectedRows = con.Execute(_config["QuerysPedidos:UpdatePedidos"],
                        new
                        {
                            Id = pedidos.id,
                            Aprobado = pedidos.aprobado,
                            ComentarioResolucion = pedidos.comentario_resolucion,
                            FechaResolucion = DateTime.Now
                        });
                }
                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int EliminarPedidos(int id)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    affectedRows = con.Execute(_config["QuerysPedidos:DeletePedidos"], new { Id = id });
                }
                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int InsertarPedidoCelular(PedidosCelular pedidosCelular)
        {

                try
                {
                    var affectedRows = 0;
                    var query = _config["QuerysPedidosCelular:InsertarPedidoCelular"];

                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                        con.Open();

                        affectedRows = con.Execute(query,
                            new
                            {
                                IdPedido = pedidosCelular.IdPedido,
                                Celular = pedidosCelular.Celular
                            });
                    }

                    return affectedRows;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }           
        }

        public List<Pedidos> ObtenerPedidosByUser(string userId)
        {
            bool activo = true;
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysPedidos:SelectPedidosByUserId"];
                    //return con.Query<Pedidos>(query, new { Activo = activo });

                    var result = (List<Pedidos>)con.Query<Pedidos>(query, new { Activo = activo, UserId = userId });

                    foreach (Pedidos item in result)
                    {
                        var resultProductos = (List<ProductosPedido>)con.Query<ProductosPedido>(sql: _config["QuerysProductosPedido:SelectProductosPedidoId"],
                            new { IdPedido = item.id, Activo = activo });

                        item.productos = resultProductos;

                        List<EstatusPedido> resultEsatusPedido = (List<EstatusPedido>)con.Query<EstatusPedido>(sql: _config["QuerysEstatusPedido:SelectEstatusPedidoId"],
                            new { IdPedido = item.id, Activo = activo });

                        item.estatusPedido = new EstatusPedido()
                        {
                            id = resultEsatusPedido[0].id,
                            id_pedido = resultEsatusPedido[0].id_pedido,
                            estado = resultEsatusPedido[0].estado,
                            activo = resultEsatusPedido[0].activo,
                            fecha_creacion = resultEsatusPedido[0].fecha_creacion
                        };
                    }

                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}