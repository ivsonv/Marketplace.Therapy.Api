using Marketplace.Domain.Models.dto.payment;
using System.Threading.Tasks;

namespace Marketplace.Integrations.Payment
{
    public class PaymentIntegrations : Domain.Interface.Integrations.Payment.IPayment
    {
        public Task Buy(PaymentDto _payment)
        {
            throw new System.NotImplementedException();
        }

        public Task Cancel()
        {
            throw new System.NotImplementedException();
        }

        public Task Refund()
        {
            throw new System.NotImplementedException();
        }

        public Task Search()
        {
            throw new System.NotImplementedException();
        }
    }
}
