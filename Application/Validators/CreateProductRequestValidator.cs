using Application.Models.Requests;
using FluentValidation;

namespace Application.Validators
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Symbol)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.UnitPrice)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
