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
    public class ProductosService : IProductosService
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection con;

        public ProductosService(IConfiguration configuration)
        {
            _config = configuration;
        }
        public IEnumerable<Productos> getAllProducts()
        {
            bool activo = true;
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var result = (List<Productos>)con.Query<Productos>(_config["QuerysProductos:SelectAllProductos"], new { Activo = activo });
                    foreach (Productos item in result)
                    {
                        var resultCategoria = (List<Categorias>)con.Query<Categorias>(sql: _config["QuerysCategorias:SelectCategoriasId"],
                            new { Id = item.categoria_id,Activo=1 });

                        item.categoria = new Categorias()
                        {
                            id = resultCategoria[0].id,
                            nombre = resultCategoria[0].nombre,
                            descripcion = resultCategoria[0].descripcion,
                            img= resultCategoria[0].img
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

        public IEnumerable<Productos> getProductsById(int id)
        {
            bool activo = true;
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    return con.Query<Productos>(_config["QuerysProductos:SelectProductosId"], new { Id = id, Activo = activo });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int InsertProducts(Productos productos)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    affectedRows = con.Execute(_config["QuerysProductos:InsertProductos"],
                        new
                        {
                            Nombre = productos.nombre,
                            Descripcion = productos.descripcion,
                            Costo=productos.costo,
                            Stock=productos.stock,
                            Imagen = productos.imagen,
                            Categoria_id= productos.categoria_id,
                            Notas= productos.notas
                        });
                }

                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int UpdateProducts(Productos productos)
        {
            try
            {
                Console.WriteLine(productos);
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    affectedRows = con.Execute(_config["QuerysProductos:UpdateProductos"],
                        new
                        {
                            Id = productos.id,
                            Nombre = productos.nombre,
                            Descripcion = productos.descripcion,
                            Costo = productos.costo,
                            Stock = productos.stock,
                            Imagen = productos.imagen,
                            Categoria_id = productos.categoria_id,
                            Activo = Convert.ToInt32(productos.activo),
                            Notas = productos.notas
                        });
                }

                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int DeleteProducts(int id)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    affectedRows = con.Execute(_config["QuerysProductos:DeleteProductos"], new { Id = id });
                }

                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Productos> getProductsByCategoryId(int categoriaId)
        {
            bool activo = true;
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    return con.Query<Productos>(_config["QuerysProductos:SelectProductosByCategoriaId"], new { Categoria_id = categoriaId, Activo = activo });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int ActualizarStockProducto(Productos productos)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    affectedRows = con.Execute(_config["QuerysProductos:UpdateStockProductos"],
                        new
                        {
                            Id = productos.id,
                            Stock = productos.stock
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