using FluentValidation;

namespace Marketplace.Services.Validators
{
    public class AddressValidator : BaseValidator<Domain.Models.dto.location.Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.address).NotNull().NotEmpty().WithMessage("Endereço é campo obrigatrio.");
            RuleFor(x => x.city).NotNull().NotEmpty().WithMessage("Cidade é campo obrigatrio.");
            RuleFor(x => x.neighborhood).NotNull().NotEmpty().WithMessage("Bairro é campo obrigatrio.");
            RuleFor(x => x.number).NotNull().NotEmpty().WithMessage("{PropertyName} é campo obrigatrio.");

            RuleFor(x => x.uf)
                .NotNull().NotEmpty().WithMessage("UF é campo obrigatrio.")
                .MaximumLength(2).WithMessage("UF permitido apenas 2 caracteres.");
        }
    }
}
