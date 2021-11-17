using AutoMapper;
using Marketplace.Domain.Helpers;
using Marketplace.Domain.Interface.Integrations.Merchant;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.provider;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.provider;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class MerchantService
    {
        private readonly ProviderService _providerService;
        private readonly EmailService _emailService;
        private readonly IMerchant _merchant;
        private readonly IMapper _mapper;


        public MerchantService(
            ProviderService providerService,
            EmailService emailService,
            IMerchant merchant,
            IMapper mapper)
        {
            _providerService = providerService;
            _emailService = emailService;
            _merchant = merchant;
            _mapper = mapper;
        }

        public async Task<BaseRs<providerRs>> Store(BaseRq<providerRq> _request)
        {
            var _res = new BaseRs<providerRs>();
            Domain.Models.dto.provider.providerDto dto = null;
            try
            {
                // buscar comerciante
                var entities = await _providerService.FindById(_request.data.id.Value);
                var provider = entities.content.provider[0];

                // validar se existe merchant criado.
                if (provider.splitAccounts.Any(a => a.payment_provider == Domain.Helpers.Enumerados.PaymentProvider.nexxera))
                {
                    _res.setError("Já existe um estabelecimento criado para esse provedor.");
                    return _res;
                }

                // criar estabelecimento
                provider = await _merchant.Create(provider, Domain.Helpers.Enumerados.PaymentProvider.nexxera);
                dto = provider;

                // notificar nexxera
                string msg = $"Por favor, peço ativar transação para Razão social: {provider.fantasy_name} {provider.company_name}" +
                    $"<br><br>CNPJ: {provider.cnpj}" +
                    $"<br><br>CPF: {provider.cpf}";
                _emailService.sendDefault("atendimento@meunix.com.br", "Clique Terapia - Criar MERCHANT", "Implantação", msg);

                // mapper
                var mapper = _mapper.Map<providerRq>(provider);

                //atualizar
                await _providerService.Update(new BaseRq<providerRq>() { data = mapper });

                // notificar psico
                msg = $"Iniciamos o processo de sincronizar sua conta para receber seus pagamentos. <br>" +
                    $"Informaremos quando tudo tiver Pronto.";
                _emailService.sendDefault(provider.email, "Clique Terapia - sincronizar conta pendente", $"{provider.fantasy_name} {provider.company_name}", msg);
            }
            catch (System.Exception ex) { _res.setError(ex); }
            finally
            {
                if (dto != null && !dto.splitAccounts.IsEmpty()) 
                {
                    string msg = $"MERCHANT: {dto.email} <br>LOG: {dto.splitAccounts[0].json}";
                    _emailService.sendDefault("ivsonv@gmail.com", "Clique Ter.-MERCHANT", $"IVSON LOG", msg);
                }
            }
            return _res;
        }
    }
}