using Marketplace.Domain.Helpers;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class EmailService
    {
        private readonly Domain.Interface.Integrations.Email.IEmail _IEmail;
        private readonly IConfiguration _configuration;
        public EmailService(Domain.Interface.Integrations.Email.IEmail IEmail,
                            IConfiguration configuration)
        {
            _configuration = configuration;
            _IEmail = IEmail;
        }

        public void sendWelcome(Domain.Models.dto.customer.customerDto _customer)
        {
            var dto = new Domain.Models.dto.email.emailDto()
            {
                title = "Bem-vindo ao 99Motos",
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

        public void sendWelcome(Domain.Models.dto.company.providerDto _company)
        {
            var dto = new Domain.Models.dto.email.emailDto()
            {
                title = "Bem-vindo ao 99Motos",
                body = this.GetTemplate(Enumerados.EmailType.welcome),
                email = _company.email
            };

            if (!dto.body.IsEmpty())
            {
                dto.body = dto.body.Replace("%TITLE%", ":: Bem Vindo ::");
                dto.body = dto.body.Replace("%NAME%", _company.fantasy_name);
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
                dto.body = dto.body.Replace("{{LINKREDEFINICAO}}", $"{_configuration["environments:front"]}?recover={token}");
                Task.Run(() => _IEmail.send(dto));
            }
        }

        private string GetTemplate(Enumerados.EmailType _enumtp)
        {
            string filePath = Path.Combine($"wwwroot/templates/{_enumtp}.html");
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