using Marketplace.Domain.Models.dto.payment;
using System.Threading.Tasks;

namespace Marketplace.Integrations.Payment
{
    public class PaymentIntegrations : Domain.Interface.Integrations.Payment.IPayment
    {
        public Task Buy(PaymentDto _payment)
        {
            switch (_payment.PaymentProvider)
            {
                case Domain.Helpers.Enumerados.PaymentProvider.nexxera:
                    Nexxera.NexxeraClient.Buy(_payment); break;

                default:
                    throw new System.NotImplementedException();
            }
            return Task.FromResult(_payment);
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
