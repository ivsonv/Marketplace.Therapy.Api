using FluentValidation;

namespace Marketplace.Services.Validators
{
    public class AddressValidator : BaseValidator<Domain.Models.dto.location.Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.address).NotNull().NotEmpty().WithMessage("Logradouro é um campo obrigatório.");
            RuleFor(x => x.complement).NotNull().NotEmpty().WithMessage("Complemento é um campo obrigatório.");
            RuleFor(x => x.city).NotNull().NotEmpty().WithMessage("Cidade é campo obrigatório.");
            RuleFor(x => x.neighborhood).NotNull().NotEmpty().WithMessage("Bairro é campo obrigatório.");
            RuleFor(x => x.number).NotNull().NotEmpty().WithMessage("Numero é campo obrigatório.");

            RuleFor(x => x.uf)
                .NotNull().NotEmpty().WithMessage("UF é campo obrigatrio.")
                .MaximumLength(2).WithMessage("UF permitido apenas 2 caracteres.");
        }
    }
}
