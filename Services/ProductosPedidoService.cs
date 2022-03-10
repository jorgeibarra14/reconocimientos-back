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
    public class ProductosPedidoService : IProductosPedidoService
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection con;

        public ProductosPedidoService(IConfiguration configuration)
        {
            _config = configuration;
        }
        public IEnumerable<ProductosPedido> ObtenerAllProductosPedido()
        {
            bool activo = true;
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysProductosPedido:SelectAllProductosPedido"];
                    return con.Query<ProductosPedido>(query, new { Activo = activo });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<ProductosPedido> ObtenerProductosPedidoId(int id)
        {
            bool activo = true;
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysProductosPedido:SelectProductosPedidoId"];
                    return con.Query<ProductosPedido>(query, new { IdPedido = id, Activo = activo });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int InsertarProductosPedido(ProductosPedido productosPedido)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysProductosPedido:InsertProductosPedido"];

                        affectedRows = con.Execute(query,
                            new
                            {
                                IdPedido =productosPedido.id_pedido,
                                ProductoId = productosPedido.producto_id,
                                ProductoNombre = productosPedido.producto_nombre,
                                ProductoCosto = productosPedido.producto_costo,
                                ProductoImagen = productosPedido.producto_imagen,
                                Cantidad = productosPedido.cantidad
                            });
                   
                    return affectedRows;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int ActulizarProductosPedido(ProductosPedido productosPedido)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    affectedRows = con.Execute(_config["QuerysProductosPedido:UpdateProductosPedido"],
                        new
                        {
                            Id = productosPedido.id,
                            IdPedido = productosPedido.id_pedido,
                            ProductoId = productosPedido.producto_id,
                            ProductoNombre = productosPedido.producto_nombre,
                            ProductoCosto = productosPedido.producto_costo,
                            ProductoImagen = productosPedido.producto_imagen,
                            Cantidad = productosPedido.cantidad
                           
                        });
                }
                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int EliminarProductosPedido(int id)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                 affectedRows = con.Execute(_config["QuerysProductosPedido:DeleteProductosPedido"], new { IdPedido = id });
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