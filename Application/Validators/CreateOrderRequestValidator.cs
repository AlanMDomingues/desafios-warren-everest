using Application.Models.Requests;
using FluentValidation;

namespace Application.Validators
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.Quotes)
                .NotEmpty()
                .GreaterThan(0)
                .LessThanOrEqualTo(100);
        }
    }
}
