using FluentValidation;
using Marketplace.Domain.Models.Response;
using System.Linq;

namespace Marketplace.Services.Validators
{
    public class BaseValidator<T> : AbstractValidator<T>
    {
        public BaseError Check(T value)
        {
            var _validade = base.Validate(value);
            return !_validade.IsValid
                   ? new BaseError(_validade.Errors.Select(s => s.ErrorMessage).ToList())
                   : null;
        }
    }
}
