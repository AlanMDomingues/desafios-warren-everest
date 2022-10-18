using Application.Models.Requests;
using FluentValidation;

namespace Application.Validators
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.Symbol)
                .NotEmpty();

            RuleFor(x => x.UnitPrice)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
