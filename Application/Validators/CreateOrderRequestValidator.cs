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
                .GreaterThan(0);

            RuleFor(x => x.PortfolioId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.ProductId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.CustomerBankInfoId)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
