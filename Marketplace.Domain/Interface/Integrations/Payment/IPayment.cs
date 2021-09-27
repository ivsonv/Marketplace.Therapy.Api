using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Integrations.Payment
{
    public interface IPayment
    {
        Task Buy(Models.dto.payment.PaymentDto _payment);
        Task Search(Models.dto.payment.PaymentDto _payment);
        Task Cancel();
        Task Refund();
    }
}