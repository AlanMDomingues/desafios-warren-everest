using FluentAssertions;
using Infrastructure.Extensions;
using System;
using Xunit;

namespace Tests.InfrastructureTests
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

        [Fact]
        public void Should_Fail_When_Trying_To_Add_Birthdate_Under_Eighteen_Years_Old()
        {
            // Arrange
            var birthdate = new DateTime(DateTime.Today.Year - 17, DateTime.Today.Month, DateTime.Today.Day);

            // Act
            var actionTest = birthdate.CheckIfCustomerIsHigherThanEighteenYearsOld();

            // Assert
            actionTest.Should().BeFalse();
        }
    }
}
