using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Reconocimientos.Models;
using Microsoft.Extensions.Configuration;
using Reconocimientos.Interfaces;

namespace Reconocimientos.Services
{
    public class UsuarioRolService : IUsuarioRolService
    {
        private readonly IConfiguration _config;

        public UsuarioRolService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IEnumerable<UsuariosRoles> ObtenerUsuarioRol(bool activo)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var result = (List<UsuariosRoles>)con.Query<UsuariosRoles>(sql: _config["QuerysUsuariosRoles:SelectUsuarioRol"]);

                    foreach (UsuariosRoles item in result)
                    {
                        var resultRol = (List<Roles>)con.Query<Roles>(sql: _config["QuerysRoles:SelectRolId"],
                            new { Id = item.id_rol, Activo = Convert.ToInt32(activo) });

                        item.roles = new Roles()
                        {
                            id = resultRol[0].id,
                            nombre = resultRol[0].nombre,
                            descripcion = resultRol[0].descripcion
                        };

                        Colaboradores resultNombre = con.Query<Colaboradores>(sql: _config["QuerysColaboradores:SelectColaboradoresIdUnico"],
                            new { Id_MGA_PlazasMH = item.id_empleado }).FirstOrDefault();

                        if (resultNombre != null)
                            item.nombre = resultNombre.Nombre;
                        else
                            item.nombre = "";
                    }

                    return result;

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<UsuariosRoles> ObtenerUsuarioRolIdEmpleado(string id_empleado)
        {
            bool activo = true;
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysUsuariosRoles:SelectUsuarioRolId"];
                    return con.Query<UsuariosRoles>(query, new { IdEmpleado = id_empleado, Activo = Convert.ToInt32(activo) });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<UsuariosRoles> ObtenerUsuarioRolIdById(int id)
        {
            bool activo = true;
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysUsuariosRoles:SelectUsuarioRolById"];
                    return con.Query<UsuariosRoles>(query, new { Id = id, Activo = Convert.ToInt32(activo) });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int ActualizarUsuarioRol(UsuariosRoles usuarioRol)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysUsuariosRoles:UpdateUsuarioRol"];
                    using (con)
                    {
                        affectedRows = con.Execute(query, new
                        {
                            Id = usuarioRol.id,
                            IdEmpleado = usuarioRol.id_empleado,
                            IdRol = usuarioRol.id_rol,
                            Activo = Convert.ToInt32(usuarioRol.activo)
                        });
                    }
                }

                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int EliminarUsuarioRol(int id)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    affectedRows = con.Execute(_config["QuerysUsuariosRoles:DeleteUsuarioRol"], new { Id = id });
                }
                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int InsertarUsuarioRol(UsuariosRoles usuarioRol)
        {
            try
            {
                var affectedRows = 0;
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysUsuariosRoles:InsertUsuarioRol"];

                    using (con)
                    {
                        con.Open();

                        affectedRows = con.Execute(query,
                            new
                            {
                                IdEmpleado = usuarioRol.id_empleado,
                                IdRol = usuarioRol.id_rol
                            });
                    }
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