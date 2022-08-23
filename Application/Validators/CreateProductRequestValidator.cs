using Application.Models.Requests;
using FluentValidation;

namespace Application.Validators
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Symbol)
                .NotEmpty();

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0);
        }
    }
}
