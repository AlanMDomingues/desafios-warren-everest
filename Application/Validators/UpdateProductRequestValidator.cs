using Application.Models.Requests;
using FluentValidation;

namespace Application.Validators
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
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
