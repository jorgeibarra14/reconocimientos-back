using MimeKit;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Reconocimientos.Models
{
    public class Notificaciones
    {
        public int id { get; set; }
        public int id_reconocimiento { get; set; }
        public string id_empleado { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public bool leido { get; set; }
        public bool activo { get; set; }
        public DateTime fecha_registro { get; set; }
    }

    public class CorreoNotificacion
    {

        public IList<string> ToMail { get; set; }
        public IList<string>? Cco { get; set; }
        public IList<string>? Cc { get; set; }
        public ReconocimientoAprobado? reconocimientoaprobado { get; set; }
        public ReconocimientoRechazado? reconocimientorechazado { get; set; }
        public string TipoEvento { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }


        public IList<Attachment>? Attachments { get; set; }
        public string FromDisplay { get; set; }
        public string FromMail { get; set; }
        public dynamic Item { get; set; }
        public int UserId { get; set; }

    }

    public class ReconocimientoAprobado
    {
        public string Url { get; set; }
        public string competencia_id { get; set; }
        public string competencia_nombre { get; set; }
        public string competencia_descripcion { get; set; }
        public string nombre_quien_envia { get; set; }
        public string recibe { get; set; }
        public string comentario { get; set; }
    }

    public class ReconocimientoRechazado
    {
        public string Url { get; set; }
        public string nombre { get; set; }
        public string body { get; set; }
    }

    public class EmailMessage
    {
        public MailboxAddress Sender { get; set; }
        public IList<string> Reciever { get; set; }
        public IList<string> Bcc { get; set; }
        public IList<string> CC { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }

    public class EmailConfiguration
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}