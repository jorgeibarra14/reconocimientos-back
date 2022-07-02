using Dapper;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Utils;
using Reconocimientos.Interfaces;
using Reconocimientos.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace Reconocimientos.Services
{
    public class NotificacionesService : INotificacionesService
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection con;
        private readonly EmailConfiguration _emailConfig;

        private readonly string FullFormatPath;
        private readonly string[] ImgPaths;
        private readonly string[] templates;

        public NotificacionesService(IConfiguration configuration, EmailConfiguration emailConfig)
        {
            _config = configuration;
            con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
            _emailConfig = emailConfig;

            FullFormatPath = Path.Combine(Environment.CurrentDirectory, "wwwroot");      
            ImgPaths = Directory.GetFiles(Path.Combine(FullFormatPath, "img"));
            templates = Directory.GetFiles(Path.Combine(FullFormatPath, "Templates"));      
    }

        public IEnumerable<Notificaciones> ObtenerNotificaciones()
        {
            bool activo = true;
            try
            {
                var query = _config["QuerysNotificaciones:SelectNotificaciones"];
                return con.Query<Notificaciones>(query, new { Activo = activo });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Notificaciones> ObtenerNotificacionesIdEmpleado(string id_empleado)
        {
            bool activo = true;
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var query = _config["QuerysNotificaciones:SelectNotificacionesIdEmpleado"];
                    return con.Query<Notificaciones>(query, new { IdEmpleado = id_empleado, Activo = activo });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public int MarcarComoLeidoIdEmpleado(string id_empleado)
        {
            try
            {
                var notificaciones = this.ObtenerNotificacionesIdEmpleado(id_empleado);
                foreach (Notificaciones n in notificaciones)
                {
                    n.leido = true;
                    this.ActualizarNotificacion(n);
                }
                return 1;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public int ActualizarNotificacion(Notificaciones notificacion)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]))
                {
                    var affectedRows = 0;
                    var query = _config["QuerysNotificaciones:UpdateNotificacion"];
                    using (con)
                    {
                        affectedRows = con.Execute(query, new
                        {
                            Id = notificacion.id,
                            IdReconocimiento = notificacion.id_reconocimiento,
                            IdEmpleado = notificacion.id_empleado,
                            titulo = notificacion.titulo,
                            Descripcion = notificacion.descripcion,
                            Leido = notificacion.leido,
                            Activo = Convert.ToInt32(notificacion.activo)
                        });
                    }

                    return affectedRows;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int EliminarNotificacion(int id)
        {
            try
            {
                var affectedRows = 0;
                var query = _config["QuerysNotificaciones:DeleteNotificacion"];
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

        public int InsertarNotificacion(Notificaciones notificacion)
        {
                try
                {
                    var affectedRows = 0;
                    var query = _config["QuerysNotificaciones:InsertNotificacion"];

                    using (con)
                    {
                        con.Open();

                        affectedRows = con.Execute(query,
                            new
                            {
                                IdReconocimiento = notificacion.id_reconocimiento,
                                IdEmpleado = notificacion.id_empleado,
                                Titulo = notificacion.titulo,
                                Descripcion = notificacion.descripcion
                            });
                    }

                    return affectedRows;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

        public async Task EnviarNotificacion(CorreoNotificacion notificacion)
        {
            try
            {
                var mimeMessage = new MimeMessage();
                EmailMessage message = new EmailMessage();
                List<string> receiver = new List<string>();
                //receiver.Add("jorge.ibarra.ms@gmail.com");
                receiver.Add("jorge.ibarra14@outlook.com");
                receiver.Add(notificacion.ToMail[0]);
                message.Sender = new MailboxAddress("Talent Suite Urrea", _emailConfig.From);
                //message.Reciever = notificacion.ToMail;
                message.Reciever = receiver;

                //message.Bcc = notificacion.Cco;
                //message.CC = notificacion.Cc;
                message.Subject = notificacion.Subject;

                switch (notificacion.TipoEvento)
                {  
                    case "ReconociminetoAprobado":
                        mimeMessage = await CreateEmailReconocimientoAprobado(message, notificacion.reconocimientoaprobado);
                        break;
                    case "ReconociminetoRechazado":
                        mimeMessage = await CreateEmailReconocimientoRechazado(message, notificacion.reconocimientorechazado);
                        break;
                }

                if (mimeMessage.To.Count > 0)
                {
                    using (SmtpClient smtpClient = new SmtpClient())
                    {
                        smtpClient.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                        smtpClient.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                        //smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                        smtpClient.Send(mimeMessage);
                        smtpClient.Disconnect(true);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error en el servicio al enviar email de tipo: " + notificacion.TipoEvento + " " +
                                    e.Message);
            }
        }

        private async Task<MimeMessage> CreateEmailReconocimientoAprobado(EmailMessage message, ReconocimientoAprobado reconocimiento)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(message.Sender);
            InternetAddressList listTo = new InternetAddressList();
            InternetAddressList listCC = new InternetAddressList();
            InternetAddressList listCCO = new InternetAddressList();
            string msg;
            var builder = new BodyBuilder();

            string templeteUrl = string.Empty;

            for (var i = 0; i < message.Reciever.Count; i++)
            {
                listTo.Add(new MailboxAddress(message.Reciever[i]));
            }

            if (message.CC != null)
            {
                for (var i = 0; i < message.CC.Count; i++)
                {
                    listCCO.Add(new MailboxAddress(message.CC[i]));
                }
            }

            if (message.Bcc != null)
            {
                for (var i = 0; i < message.Bcc.Count; i++)
                {
                    listCCO.Add(new MailboxAddress(message.Bcc[i]));
                }
            }

            if (listTo.Count > 0)
            {
                mimeMessage.To.AddRange(listTo);
            }

            if (listCC.Count > 0)
            {
                mimeMessage.Cc.AddRange(listCC);
            }

            if (listCCO.Count > 0)
            {
                mimeMessage.Bcc.AddRange(listCCO);
            }

            foreach (var t in templates)
            {             
                    if (t.Contains("nuevo"))
                    {
                        templeteUrl = t;
                    }                         
            }

            mimeMessage.Subject = message.Subject;

            using (System.IO.StreamReader SourceReader = System.IO.File.OpenText(templeteUrl))
            {
                msg = SourceReader.ReadToEnd();
                //msg = msg.Replace("{{competencia_id}}", $"{reconocimiento.competencia_id}");
                msg = msg.Replace("{{competencia}}", $"{reconocimiento.competencia_nombre}");
                //msg = msg.Replace("{{competencia}}", $"{reconocimiento.}");

                //msg = msg.Replace("{{comentario}}", $"{reconocimiento.competencia_descripcion}");
                msg = msg.Replace("{{nombre_envia}}", $"{reconocimiento.nombre_quien_envia}");
                msg = msg.Replace("{{comentario}}", $"{reconocimiento.comentario}");
                msg = msg.Replace("{{recibe}}", $"{reconocimiento.recibe}");


                //var ITGovUrl = _config.GetSection("UrlApps").GetValue<string>("ITGovApp");
                //var url = $"{ITGovUrl}/Login/Index?applicationName=Reconocimientos";

                //msg = msg.Replace("{{Url}}", $"{url}");
                //foreach (string imgpath in ImgPaths)
                //{
                //    if (imgpath.Contains("team.png"))
                //    {
                //        var image = builder.LinkedResources.Add(imgpath);
                //        image.ContentId = MimeUtils.GenerateMessageId();
                //        msg = msg.Replace("{{competencia_imagen}}", image.ContentId);
                //    }

                //    if (imgpath.Contains("principal.jpg"))
                //    {
                //        var image = builder.LinkedResources.Add(imgpath);
                //        image.ContentId = MimeUtils.GenerateMessageId();
                //        msg = msg.Replace("{{logo}}", image.ContentId);
                //    }
                //}

                builder.HtmlBody = msg;
            }

            mimeMessage.Body = builder.ToMessageBody();

            return mimeMessage;
        }

        private async Task<MimeMessage> CreateEmailReconocimientoRechazado(EmailMessage message, ReconocimientoRechazado reconocimiento)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(message.Sender);
            InternetAddressList listTo = new InternetAddressList();
            InternetAddressList listCC = new InternetAddressList();
            InternetAddressList listCCO = new InternetAddressList();
            string msg;
            var builder = new BodyBuilder();

            string templeteUrl = string.Empty;

            for (var i = 0; i < message.Reciever.Count; i++)
            {
                listTo.Add(new MailboxAddress(message.Reciever[i]));
            }

            if (message.CC != null)
            {
                for (var i = 0; i < message.CC.Count; i++)
                {
                    listCCO.Add(new MailboxAddress(message.CC[i]));
                }
            }

            if (message.Bcc != null)
            {
                for (var i = 0; i < message.Bcc.Count; i++)
                {
                    listCCO.Add(new MailboxAddress(message.Bcc[i]));
                }
            }

            if (listTo.Count > 0)
            {
                mimeMessage.To.AddRange(listTo);
            }

            if (listCC.Count > 0)
            {
                mimeMessage.Cc.AddRange(listCC);
            }

            if (listCCO.Count > 0)
            {
                mimeMessage.Bcc.AddRange(listCCO);
            }

            foreach (var t in templates)
            {            
                    if (t.Contains("Notificacion-Rechazado"))
                    {
                        templeteUrl = t;
                    }           
            }

            mimeMessage.Subject = message.Subject;

            using (System.IO.StreamReader SourceReader = System.IO.File.OpenText(templeteUrl))
            {
                msg = SourceReader.ReadToEnd();
                msg = msg.Replace("{{Nombre}}", $"{reconocimiento.nombre}");
                msg = msg.Replace("{{Body}}", $"{reconocimiento.body}");
                msg = msg.Replace("{{Fecha}}", DateTime.Now.ToString("dd/MM/yyyy"));

                var ITGovUrl = _config.GetSection("UrlApps").GetValue<string>("ITGovApp");
                var url = $"{ITGovUrl}/Login/Index?applicationName=Reconocimientos";

                msg = msg.Replace("{{Url}}", $"{url}");
                foreach (string imgpath in ImgPaths)
                {
                    
                    if (imgpath.Contains("logo_banner.png"))
                    {
                        var image = builder.LinkedResources.Add(imgpath);
                        image.ContentId = MimeUtils.GenerateMessageId();
                        msg = msg.Replace("{{logo}}", image.ContentId);
                    }
                }

                builder.HtmlBody = msg;
            }

            mimeMessage.Body = builder.ToMessageBody();

            return mimeMessage;
        }
    }  
}
