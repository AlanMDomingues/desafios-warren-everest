using Application.Models.Requests;
using FluentValidation;

namespace Application.Validators
{
    public class CreatePortfolioRequestValidator : AbstractValidator<CreatePortfolioRequest>
    {
        public CreatePortfolioRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.Description)
                .MaximumLength(100);
        }
    }
}
