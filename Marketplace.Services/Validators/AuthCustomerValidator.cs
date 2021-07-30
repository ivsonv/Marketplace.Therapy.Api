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
                  .NotNull().NotEmpty().WithMessage("{PropertyName} informado e inválido.")
                  .MinimumLength(5).WithMessage("{PropertyName} Senha informada e incompativel.");
        }
    }
}
