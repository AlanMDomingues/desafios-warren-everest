using FluentAssertions;
using Infrastructure.Extensions;
using System;
using Xunit;

namespace API.Tests.InfrastructureTests
{
    public class DateTimeExtensionsTests
    {
        [Theory]
        [InlineData(2004, 01, 01)]
        [InlineData(2004, 11, 08)]
        [InlineData(2003, 08, 07)]
        public void Should_Pass_When_Trying_To_Add_Birthdate_Higher_Than_Eighteen_Years_Old(int year, int month, int day)
        {
            // Arrange
            var birthdate = new DateTime(year, month, day);

            // Act
            var actionTest = birthdate.CheckIfCustomerIsHigherThanEighteenYearsOld();

            // Assert
            actionTest.Should().BeTrue();
        }

        [Theory]
        [InlineData(17, 0, 0)]
        [InlineData(18, 1, 0)]
        [InlineData(18, 0, 1)]
        public void Should_Fail_When_Trying_To_Add_Birthdate_Under_Eighteen_Years_Old(int reducedYears, int addedMonths, int addedDays)
        {
            // Arrange
            var yearToday = DateTime.Today.Year;
            var monthToday = DateTime.Today.Month;
            var dayToday = DateTime.Today.Day;

            var daysInMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);

            if (dayToday + addedDays > daysInMonth)
            {
                dayToday = 0;
            }

            if (monthToday + addedMonths == 13)
            {
                yearToday++;
                monthToday = 0;
                dayToday = 1;
            }

            try
            {
                var test = new DateTime(yearToday, monthToday + addedMonths, dayToday);
            }
            catch (ArgumentOutOfRangeException)
            {
                if (monthToday == 2)
                {
                    dayToday -= 3;
                }
                else
                {
                    dayToday -= 1;
                }
            }

            var birthdate = new DateTime(yearToday - reducedYears, monthToday + addedMonths, dayToday + addedDays);

            // Act
            var actionTest = birthdate.CheckIfCustomerIsHigherThanEighteenYearsOld();

            // Assert
            actionTest.Should().BeFalse();
        }
    }
}
