using FluentValidation;
using Marketplace.Domain.Models.Request.auth.customer;

namespace Marketplace.Services.Validators
{
    public class CustomerAuthValidator : BaseValidator<customerAuthRq>
    {
        public CustomerAuthValidator()
        {
            RuleFor(x => x.login).EmailAddress().WithMessage("{PropertyName} informado e Inválido.");
            RuleFor(x => x.password)
                  .NotNull().NotEmpty().WithMessage("Senha informado e inválido.")
                  .MinimumLength(5).WithMessage("Senha Senha informada e incompativel.");
        }
    }
}
