using FluentAssertions;
using Infrastructure.Extensions;
using Xunit;

namespace Tests.InfrastructureTests
{
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Should_Pass_When_Trying_To_Convert_String_To_Int(int index)
        {
            // Arrange
            var number = "123";
            var numberConverted = (int)char.GetNumericValue(number, index);

            // Act
            var actionTest = number.ToIntAt(index);

            // Assert
            actionTest.Should().Be(numberConverted);
        }
    }
}
