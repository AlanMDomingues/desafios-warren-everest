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
                .MinimumLength(2)
                .MaximumLength(20);

            RuleFor(x => x.UnitPrice)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
