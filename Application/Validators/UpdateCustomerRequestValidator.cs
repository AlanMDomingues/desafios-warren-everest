using Application.Models.Requests;
using FluentValidation;
using FluentValidation.Validators;
using Infrastructure.Extensions;
using System;

namespace Application.Validators
{
    public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
    {
        public UpdateCustomerRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(300)
                .Must(x => x.IsValidFullName());

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress(EmailValidationMode.Net4xRegex)
                .MinimumLength(10)
                .MaximumLength(256);

            RuleFor(x => x)
                .Must(x => x.EmailSms && !x.Whatsapp || !x.EmailSms && x.Whatsapp || x.EmailSms && x.Whatsapp)
                .WithMessage("At least one or both 'EmailSms' or/and 'Whatsapp' must be true");

            RuleFor(x => x.Cpf)
                .NotEmpty()
                .Must(x => x.IsValidCPF())
                .MinimumLength(11)
                .MaximumLength(14);

            RuleFor(x => x.Cellphone)
                .NotEmpty()
                .Must(x => x.IsValidNumber())
                .Length(11);

            RuleFor(x => x.Birthdate)
                .NotEmpty()
                .Must(x => x.CheckIfCustomerIsHigherThanEighteenYearsOld())
                .WithMessage("Customer must be at least eighteen years old");

            RuleFor(x => x.Country)
                .NotEmpty()
                .Must(x => x.IsValidPlace())
                .MinimumLength(2)
                .MaximumLength(58);

            RuleFor(x => x.City)
                .NotEmpty()
                .Must(x => x.IsValidPlace())
                .MinimumLength(2)
                .MaximumLength(58);

            RuleFor(x => x.PostalCode)
                .NotEmpty()
                .Must(x => x.IsValidNumber())
                .Length(8);

            RuleFor(x => x.Address)
                .NotEmpty()
                .Must(x => x.IsValidPlace())
                .MinimumLength(2)
                .MaximumLength(100);

            RuleFor(x => x.Number)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}