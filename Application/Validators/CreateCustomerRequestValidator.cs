using Application.Models.Requests;
using FluentValidation;
using FluentValidation.Validators;
using Infrastructure.Extensions;
using System;

namespace Application.Validators
{
    public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
    {
        public CreateCustomerRequestValidator()
        {
            RuleFor(x => x.FullName)
                 .NotEmpty()
                 .Must(x => x.IsValidFullName())
                 .MinimumLength(2)
                 .MaximumLength(300);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress(EmailValidationMode.Net4xRegex)
                .MinimumLength(9)
                .MaximumLength(256);

            RuleFor(x => x)
                .Must(x => x.Email == x.EmailConfirmation)
                .WithMessage("'Email' and 'EmailConfirmation' should be equals")
                .Must(x => x.EmailSms && !x.Whatsapp || !x.EmailSms && x.Whatsapp || x.EmailSms && x.Whatsapp)
                .WithMessage("At least one or both 'EmailSms' or/and 'Whatsapp' must be true");

            RuleFor(x => x.Cpf)
                .NotEmpty()
                .Must(IsValidCPF)
                .MinimumLength(11)
                .MaximumLength(14);

            RuleFor(x => x.Cellphone)
                .NotEmpty()
                .Must(x => x.IsValidNumber())
                .Length(11);

            RuleFor(x => x.Birthdate)
                .NotEmpty()
                .Must(CheckIfCustomerIsHigherThanEighteenYearsOld)
                .WithMessage("Customer must be at least eighteen years old");

            RuleFor(x => x.Country)
                .NotEmpty()
                .Must(x => x.IsValidText())
                .MinimumLength(2)
                .MaximumLength(58);

            RuleFor(x => x.City)
                .NotEmpty()
                .Must(x => x.IsValidText())
                .MinimumLength(2)
                .MaximumLength(58);

            RuleFor(x => x.PostalCode)
                .NotEmpty()
                .Must(x => x.IsValidNumber())
                .Length(8);

            RuleFor(x => x.Adress)
                .NotEmpty()
                .Must(x => x.IsValidText())
                .MinimumLength(2)
                .MaximumLength(100);

            RuleFor(x => x.Number)
                .NotEmpty()
                .GreaterThan(0);
        }

        private static bool CheckIfCustomerIsHigherThanEighteenYearsOld(DateTime birthdate)
        {
            var dateTimeNow = DateTime.Now;
            if (dateTimeNow.Year - birthdate.Year > 18)
            {
                return true;
            }

            if (dateTimeNow.Year - birthdate.Year == 18)
            {
                if (birthdate.Month - dateTimeNow.Month < 0)
                {
                    return true;
                }
                else if (birthdate.Month - dateTimeNow.Month == 0)
                {
                    if (birthdate.Day <= dateTimeNow.Day)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsValidCPF(string cpf)
        {
            cpf = cpf.Replace(".", string.Empty).Replace("-", string.Empty);

            if (!cpf.IsValidNumber() || cpf.AllCharacteresArentEqualsToTheFirstCharacter()) return false;

            var firstDigitAfterDash = 0;
            for (int i = 0; i < cpf.Length - 2; i++)
            {
                firstDigitAfterDash += cpf.ToIntAt(i) * (10 - i);
            }

            firstDigitAfterDash = (firstDigitAfterDash * 10) % 11;
            firstDigitAfterDash = firstDigitAfterDash == 10 ? 0 : firstDigitAfterDash;

            var secondDigitAfterDash = 0;
            for (int i = 0; i < cpf.Length - 1; i++)
            {
                secondDigitAfterDash += cpf.ToIntAt(i) * (11 - i);
            }

            secondDigitAfterDash = (secondDigitAfterDash * 10) % 11;
            secondDigitAfterDash = secondDigitAfterDash == 10 ? 0 : secondDigitAfterDash;

            return firstDigitAfterDash == cpf.ToIntAt(^2) && secondDigitAfterDash == cpf.ToIntAt(^1);
        }
    }
}