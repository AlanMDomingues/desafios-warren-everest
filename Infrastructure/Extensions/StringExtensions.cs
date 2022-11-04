using System;
using System.Linq;

namespace Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static int ToIntAt(this string value, Index index)
        {
            var indexValue = index.IsFromEnd
                ? value.Length - index.Value
                : index.Value;

            return (int)char.GetNumericValue(value, indexValue);
        }

        public static bool IsValidNumber(this string number)
        {
            if (string.IsNullOrEmpty(number)) return false;

            return number.All(x => char.IsDigit(x));
        }

        public static bool IsValidPlace(this string text)
        {
            if (text is null
                || text.Trim() != text
                || text.Split(' ').Contains("")) return false;

            return !AllCharacteresAreEqualsToTheFirstCharacter(text);
        }

        public static string CpfFormatter(this string cpf)
        {
            var cpfFormated = cpf.Replace(".", string.Empty).Replace("-", string.Empty);
            return cpfFormated;
        }

        public static bool IsValidFullName(this string fullName)
        {
            if (fullName is null
                || fullName.Trim() != fullName
                || fullName.Split(' ').Contains("")
                || !char.IsUpper(fullName.First())
                || fullName.Any(x => char.IsNumber(x))
                || AllCharacteresAreEqualsToTheFirstCharacter(fullName)) return false;

            var nameAndLastName = fullName.Split(' ');
            return nameAndLastName.Length > 1 && nameAndLastName.Length <= 30;
        }

        public static bool AllCharacteresAreEqualsToTheFirstCharacter(this string field)
        {
            field = field.Replace(" ", string.Empty).ToLower();

            return field.All(c => c.Equals(field.First()));
        }

        public static bool IsValidCPF(this string cpf)
        {
            cpf = cpf.CpfFormatter();

            if (!cpf.IsValidNumber() || cpf.AllCharacteresAreEqualsToTheFirstCharacter()) return false;

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
