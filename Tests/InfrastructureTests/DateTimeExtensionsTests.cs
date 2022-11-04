using FluentAssertions;
using Infrastructure.Extensions;
using System;
using Xunit;

namespace API.Tests.InfrastructureTests
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void Should_Pass_When_Trying_To_Add_Birthdate_Higher_Than_Eighteen_Years_Old()
        {
            // Arrange
            var birthdate = new DateTime(DateTime.Today.Year - 18, DateTime.Today.Month, DateTime.Today.Day);

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
                monthToday += 1;
                dayToday = 1;
                addedDays--;
            }

            if (monthToday + addedMonths == 13)
            {
                yearToday += 1;
                monthToday = 1;
                dayToday = 1;
                addedMonths--;
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
