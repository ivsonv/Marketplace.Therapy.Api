using Marketplace.Domain.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class EmailService
    {
        private readonly Domain.Interface.Integrations.Email.IEmail _IEmail;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmailService(Domain.Interface.Integrations.Email.IEmail IEmail,
                            IConfiguration configuration,
                            IWebHostEnvironment env)
        {
            _configuration = configuration;
            _IEmail = IEmail;
            _env = env;
        }

        public void sendWelcome(Domain.Models.dto.customer.customerDto _customer)
        {
            var dto = new Domain.Models.dto.email.emailDto()
            {
                title = "Bem-vindo Clique Terapia",
                body = this.GetTemplate(Enumerados.EmailType.welcome),
                email = _customer.email
            };

            if (!dto.body.IsEmpty())
            {
                dto.body = dto.body.Replace("%TITLE%", ":: Bem Vindo ::");
                dto.body = dto.body.Replace("%NAME%", _customer.name);
                Task.Run(() => _IEmail.send(dto));
            }
        }
        public void sendWelcome(Domain.Models.dto.provider.providerDto _provider)
        {
            var dto = new Domain.Models.dto.email.emailDto()
            {
                title = "Bem-vindo ao Clique Terapia",
                body = this.GetTemplate(Enumerados.EmailType.welcome),
                email = _provider.email
            };

            if (!dto.body.IsEmpty())
            {
                dto.body = dto.body.Replace("%TITLE%", ":: Bem Vindo ::");
                dto.body = dto.body.Replace("%NAME%", _provider.fantasy_name);
                Task.Run(() => _IEmail.send(dto));
            }
        }
        public void sendResetPassword(Domain.Models.dto.customer.customerDto _customer, string token)
        {
            var dto = new Domain.Models.dto.email.emailDto()
            {
                title = "Recuperação de Senha",
                body = this.GetTemplate(Enumerados.EmailType.recoverpassword),
                email = _customer.email
            };

            if (!dto.body.IsEmpty())
            {
                dto.body = dto.body.Replace("{{TITLE}", ":: Recuperar Senha ::");
                dto.body = dto.body.Replace("{{LINKREDEFINICAO}}", $"{_configuration["environments:front"]}/sou-paciente/esqueci-minha-senha?token={token}");
                Task.Run(() => _IEmail.send(dto));
            }
        }
        public void sendResetPasswordProvider(Domain.Models.dto.provider.providerDto _provider, string token)
        {
            var dto = new Domain.Models.dto.email.emailDto()
            {
                title = "Recuperação de Senha",
                body = this.GetTemplate(Enumerados.EmailType.recoverpassword),
                email = _provider.email
            };

            if (!dto.body.IsEmpty())
            {
                dto.body = dto.body.Replace("{{TITLE}", ":: Recuperar Senha ::");
                dto.body = dto.body.Replace("{{LINKREDEFINICAO}}", $"{_configuration["environments:front"]}/sou-psicologo/esqueci-minha-senha?token={token}");
                Task.Run(() => _IEmail.send(dto));
            }
        }

        public void sendAppointment(Domain.Models.dto.appointment.Email _email)
        {
            var dto = new Domain.Models.dto.email.emailDto()
            {
                body = this.GetTemplate(Enumerados.EmailType.appointment),
                email = _email.email,
                title = _email.nick
            };

            if (!dto.body.IsEmpty())
            {
                dto.body = dto.body.Replace("{{DESCRIPTION}}", _email.description);
                dto.body = dto.body.Replace("{{TITLE}}", _email.title);
                dto.body = dto.body.Replace("{{NAME}}", _email.name);
                dto.body = dto.body.Replace("{{SUBTITLE}}", "");
                Task.Run(() => _IEmail.send(dto));
            }
        }

        public void sendDefault(string email, string subject, string name, string description)
        {
            var dto = new Domain.Models.dto.email.emailDto()
            {
                body = this.GetTemplate(Enumerados.EmailType.defaultt),
                email = email,
                title = subject
            };

            if (!dto.body.IsEmpty())
            {
                dto.body = dto.body.Replace("{{NAME}}", name);
                dto.body = dto.body.Replace("{{DESCRIPTION}}", description);
                Task.Run(() => _IEmail.send(dto));
            }
        }

        private string GetTemplate(Enumerados.EmailType _enumtp)
        {
            string filePath = $"{_env.ContentRootPath}/templates/{_enumtp}.html";
            lock (filePath)
            {
                if (File.Exists(filePath))
                    using (var fileStream = new FileStream(filePath, FileMode.Open))
                        lock (fileStream)
                            return (new StreamReader(fileStream)).ReadToEnd();
                return "";
            }
        }
    }
}