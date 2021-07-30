using FluentValidation;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.company;

namespace Marketplace.Services.Validators
{
    public class CompanyValidator : BaseValidator<BaseRq<providerRq>>
    {
        public CompanyValidator()
        {
            RuleFor(x => x.data).NotNull().NotEmpty().WithMessage("object data not null.");
            RuleFor(x => x.data.email).EmailAddress().WithMessage("{PropertyName} informado e Inválido.");
            RuleFor(x => x.data.fantasy_name)
                  .NotNull().NotEmpty().WithMessage("{PropertyName} informado e inválido.")
                  .MinimumLength(3).WithMessage("{PropertyName} minimo 3 caracteres.");
            
            RuleFor(x => x.data.company_name)
                  .NotNull().NotEmpty().WithMessage("{PropertyName} informado e inválido.")
                  .MinimumLength(3).WithMessage("{PropertyName} minimo 3 caracteres.");

            RuleFor(x => x.data.cnpj)
                  .NotNull().NotEmpty().WithMessage("{PropertyName} informado e inválido.")
                  .MinimumLength(14)
                  .WithMessage("{PropertyName} minimo 14 caracteres.")
                  .MaximumLength(14)
                  .WithMessage("{PropertyName} máximo 14 caracteres.");

            RuleForEach(x => x.data.address).SetValidator(new AddressValidator());
        }
    }
}
