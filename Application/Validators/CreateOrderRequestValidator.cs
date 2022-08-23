using Application.Models.Requests;
using FluentValidation;

namespace Application.Validators
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.Quotes)
                .GreaterThan(0);
        }
    }
}
