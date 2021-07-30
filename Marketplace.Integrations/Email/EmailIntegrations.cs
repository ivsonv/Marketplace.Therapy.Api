using Marketplace.Domain.Helpers;
using Marketplace.Domain.Models.dto.email;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Marketplace.Integrations.Email
{
    public class EmailIntegrations : Domain.Interface.Integrations.Email.IEmail
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        public EmailIntegrations(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void send(emailDto dto)
        {
            var _mail = new MailMessage()
            {
                From = new MailAddress(_configuration["email:user"], dto.display ?? dto.title),
                Priority = MailPriority.High,
                Subject = dto.title,
                IsBodyHtml = true,
                Body = dto.body
            };

            dto.email.Split(';').ToList().ForEach(fe =>
            {
                _mail.To.Add(new MailAddress(fe));
            });

            //
            var _smtp = new SmtpClient()
            {
                Credentials = new NetworkCredential(_configuration["email:user"], _configuration["email:password"]),
                Port = _configuration["email:port"].ToInt(),
                Host = _configuration["email:host"],
                EnableSsl = true
            };
            _smtp.Send(_mail);
        }
    }
}
