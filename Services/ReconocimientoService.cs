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
    public class ReconocimientoService : IReconocimientoService
    {
        readonly IConfiguration _config;
        readonly IDbConnection con;
        public ReconocimientoService(IConfiguration configuration)
        {
            _config = configuration;
            con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        }

        public int ObtenerPuntosAcumulados(string idempleadorecibe, bool activo)
        {
            try
            {
                string query = _config["QuerysUsuariosPuntos:ObtenerPuntos"];
                return con.Query<int>(sql: query, new { IdEmpleado = idempleadorecibe}).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int InsertarReconocimiento(Models.Reconocimientos reconocimientos)
        {
            try
            {
                var affectedRows = 0;
                string query = _config["QuerysReconocimientos:InsertReconocimiento"];

                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    con.Open();
                    var result = con.ExecuteScalar(query + " SELECT SCOPE_IDENTITY()",
                        new
                        {
                            IdEmpleadoEnvia = reconocimientos.id_empleado_envia,
                            IdEmpleadoRecibe = reconocimientos.id_empleado_recibe,
                            IdCompetencia = reconocimientos.id_competencia,
                            Motivo = reconocimientos.motivo,
                            Logro = reconocimientos.logro,                       
                            IdEmpleadoAutorizador = reconocimientos.id_empleado_autorizador,
                            IdPuntos = reconocimientos.id_puntos
                        });
                    con.Close();
                    affectedRows = Convert.ToInt32(result);
                }

                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int AprobarRechazarReconocimiento(Models.Reconocimientos reconocimientos)
        {
            var affectedRows = 0;
            try
            {
                string query = _config["QuerysReconocimientos:AprobarRechazarReconocimiento"];

                using (con)
                {
                    affectedRows = con.Execute(query, new
                    {
                        Aprobado = reconocimientos.aprobado,
                        ComentarioResolucion = reconocimientos.comentario_resolucion,
                        FechaResolucion = reconocimientos.fecha_resolucion,
                        Id = reconocimientos.id
                    });
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return affectedRows;
        }

        public int EliminarReconocimiento(int id)
        {
            try
            {
                var affectedRows = 0;
                string query = _config["QuerysReconocimientos:DeleteReconocimiento"];
                using (con)
                {
                    affectedRows = con.Execute(query, new { Id = id });
                }
                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int ActualizarReconocimiento(Models.Reconocimientos reconocimientos)
        {
            var affectedRows = 0;
            try
            {
                string query = _config["QuerysReconocimientos:UpdateReconocimiento"];

                using (con)
                {
                    affectedRows = con.Execute(query, new
                    {
                        Id = reconocimientos.id,
                        IdEmpledoEnvia = reconocimientos.id_empleado_envia,
                        IdEmpledoRecibe = reconocimientos.id_empleado_recibe,
                        IdCompetencia = reconocimientos.id_competencia,
                        Motivo = reconocimientos.motivo,
                        Logro = reconocimientos.logro,
                        IdEmpleadoAutorizador = reconocimientos.id_empleado_autorizador,
                        Aprobado = reconocimientos.aprobado,
                        ComentarioResolucion = reconocimientos.comentario_resolucion,
                        FechaResolucion = reconocimientos.fecha_resolucion,
                        Activo = reconocimientos.activo,
                        FechaRegistro = reconocimientos.fecha_registro,
                        IdPuntos = reconocimientos.id_puntos
                    });
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return affectedRows;
        }

        public IEnumerable<Models.Reconocimientos> ObtenerReconocimientos()
        {
            try
            {
                string query = _config["QuerysReconocimientos:SelectReconocimientos"];
                return con.Query<Models.Reconocimientos>(sql: query);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<ReconocimientosPorAutorizar> ObtenerReconocimientosPorAutorizar(string id_empleado_autorizador, bool activo)
        {
            try
            {
                string query = _config["QuerysReconocimientos:SelectReconocimientosPorAutorizar"];
                return con.Query<ReconocimientosPorAutorizar>(sql: query, new { IdEmpleadoAutorizador = id_empleado_autorizador, Activo = activo });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<ReconocimientosPorAutorizar> ObtenerReconocimientosPorAutorizarAdmin( bool activo)
        {
            try
            {
                string query = _config["QuerysReconocimientos:SelectReconocimientosPorAutorizarAdmin"];
                return con.Query<ReconocimientosPorAutorizar>(sql: query, new { Activo = activo });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Models.Reconocimientos> ObtenerReconocimientosId(int id, bool activo)
        {
            try
            {
                string query = _config["QuerysReconocimientos:SelectReconocimientosId"];
                return con.Query<Models.Reconocimientos>(sql: query, new { Id = id, Activo = activo });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<MisReconocimientos> MisReconocimientos(string id_empleado_recibe, bool activo)
        {
            try
            {
                string query = _config["QuerysReconocimientos:SelectMisReconocimientos"];
                return con.Query<MisReconocimientos>(sql: query, new { IdEmpleadoRecibe = id_empleado_recibe, Activo = activo });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<MisReconocimientosDetalle> MisReconocimientosPorCompetencia(string id_empleado_recibe, string nombreCompetencia, bool activo)
        {
            try
            {
                string query = _config["QuerysReconocimientos:SelectMisReconocimientosPorCompetencia"];
                return con.Query<MisReconocimientosDetalle>(sql: query, new { IdEmpleadoRecibe = id_empleado_recibe, NombreCompetencia = nombreCompetencia, Activo = activo });

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<ReconocimientosEntregados> ReconocerAOtros(string id_empleado_envia, bool activo)
        {
            
            
            try
            {
                string query = _config["QuerysReconocimientos:SelectReconocerAOtros"];
                return con.Query<ReconocimientosEntregados>(sql: query, new { IdEmpleadoEnvia = id_empleado_envia, Activo = activo });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<ReconocimientosEntregadosDetalles> ReconocerAOtrosPorCompetencia(string id_empleado_envia, string nombreCompetencia, bool activo)
        {
            try
            {
                string query = _config["QuerysReconocimientos:SelectReconocerAOtrosPorCompetencia"];
                return con.Query<ReconocimientosEntregadosDetalles>(sql: query, new { IdEmpleadoEnvia = id_empleado_envia, NombreCompetencia = nombreCompetencia, Activo = activo });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int ObtenerAutorizador(string area, string sistema, string regional)
        {
            try
            {
                string query = _config["QuerysReconocimientos:SelectAutorizador"];
                return con.ExecuteScalar<int>(sql: query, new { Area = area, Sistema = sistema, Regional = regional });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int ValidarReconociminetoEntregado(string id_empleado_recibe, string id_empleado_envia, bool activo)
        {
            try
            {
                string query = _config["QuerysReconocimientos:SelectValidarReconocimiento"];
                return con.ExecuteScalar<int>(sql: query, new { IdEmpleadoRecibe = id_empleado_recibe, IdEmpleadoEnvia = id_empleado_envia, Activo = activo });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<TopReconocimiento> ObtenerTopReconocidos()
        {
            try
            {
                string query = _config["QuerysReconocimientos:SelectTopReconocidos"];
                return con.Query<TopReconocimiento>(sql: query);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int AprobarPuntosConcepto(Models.Reconocimientos reconocimiento)
        {
            try
            {
                var affectedRows = 0;
                string query = _config["QuerysUsuariosPuntos:ActivarPuntosConceptos"];

                using (con)
                {
                    con.Open();

                    affectedRows = con.Execute(query,
                        new
                        {
                            Id = reconocimiento.id,
                            Activo = reconocimiento.activo
                        });
                    con.Close();
                }

                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int AprobarPuntosPorReconocimientoId(Models.Reconocimientos reconocimiento)
        {
            try
            {
                var affectedRows = 0;
                string query = _config["QuerysUsuariosPuntos:ActivarPuntosPorReconocimientoId"];

                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    con.Open();

                    affectedRows = con.Execute(query,
                        new
                        {
                            Id = reconocimiento.id,
                            Activo = reconocimiento.activo
                        });
                    con.Close();
                }

                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int RechazarPuntosConcepto(Models.Reconocimientos reconocimiento)
        {
            try
            {
                var affectedRows = 0;
                string query = _config["QuerysUsuariosPuntos:RechzarPuntosConceptos"];

                using (con)
                {
                    con.Open();

                    affectedRows = con.Execute(query,
                        new
                        {
                            Id = reconocimiento.id,
                            ConceptoRechazo = reconocimiento.comentario_resolucion
                        });
                    con.Close();
                }

                return affectedRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<MisConceptosPuntos> ObtenerConceptosPuntos(string id_empleado_recibe)
        {
            try
            {
                string query = _config["QuerysReconocimientos:ObtenerConceptosPuntos"];
                return con.Query<MisConceptosPuntos>(sql: query, new { IdEmpleado = id_empleado_recibe });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
