namespace Marketplace.Domain.Interface.Integrations.Email
{
    public interface IEmail
    {
        void send(Models.dto.email.emailDto dto);
    }
}
