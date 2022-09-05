using Application.Models.Requests;
using FluentValidation;

namespace Application.Validators
{
    public class UpdatePortfolioRequestValidator : AbstractValidator<UpdatePortfolioRequest>
    {
        public UpdatePortfolioRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(30);

            RuleFor(x => x.Description)
                .MaximumLength(100);
        }
    }
}
