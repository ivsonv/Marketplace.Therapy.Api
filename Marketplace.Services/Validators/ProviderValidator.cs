﻿using FluentValidation;
using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.provider;

namespace Marketplace.Services.Validators
{
    public class ProviderValidator : BaseValidator<BaseRq<providerRq>>
    {
        public ProviderValidator()
        {
            RuleFor(x => x.data).NotNull().NotEmpty().WithMessage("object data not null.");
            RuleFor(x => x.data.email).EmailAddress().WithMessage("E-MAIL informado e Inválido.");
            RuleFor(x => x.data.fantasy_name)
                  .NotNull().NotEmpty().WithMessage("PRIMEIRO NOME informado e inválido.")
                  .MinimumLength(3).WithMessage("NOME deve ter minimo de 3 caracteres.");

            RuleForEach(x => x.data.address).SetValidator(new AddressValidator());
        }
    }
}
