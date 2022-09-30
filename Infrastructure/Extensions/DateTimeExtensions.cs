using System;

namespace Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool CheckIfCustomerIsHigherThanEighteenYearsOld(this DateTime birthdate)
        {
            var age = DateTime.Today.Year - birthdate.Year;
            if ((birthdate.Month > DateTime.Now.Month) || (birthdate.Month == DateTime.Now.Month && birthdate.Day > DateTime.Now.Day))
                age--;
            return age >= 18;
        }
    }
}
