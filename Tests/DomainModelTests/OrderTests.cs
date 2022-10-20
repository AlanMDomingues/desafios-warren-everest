using FluentAssertions;
using System;
using Tests.Factories;
using Xunit;

namespace Tests.DomainModelTests
{
    public class OrderTests
    {
        [Fact]
        public void Should_Pass_When_Trying_To_Set_Net_Value()
        {
            // Arrange
            var order = OrderFactory.FakeOrder();
            order.UnitPrice = 25;
            order.Quotes = 2;

            // Act
            var actionTest = () => order.SetNetValue();

            // Assert
            actionTest.Should().NotThrow();
            order.NetValue.Should().Be(50);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Fail_And_Return_ArgumentException_With_Message_When_Trying_To_Set_NetValue_With_Less_Than_Or_Equal_To_Zero_UnitPrice(decimal unitPrice)
        {
            // Arrange
            var order = OrderFactory.FakeOrder();
            order.UnitPrice = unitPrice;
            order.Quotes = 2;

            // Act
            var actionTest = () => order.SetNetValue();

            // Assert
            actionTest.Should().ThrowExactly<ArgumentException>()
                               .WithMessage("UnitPrice and Quotes must be higher than 0");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Fail_And_Return_ArgumentException_With_Message_When_Trying_To_Set_NetValue_With_With_Less_Than_Or_Equal_To_Zero_Quotes(int quotes)
        {
            // Arrange
            var order = OrderFactory.FakeOrder();
            order.UnitPrice = 2;
            order.Quotes = quotes;

            // Act
            var actionTest = () => order.SetNetValue();

            // Assert
            actionTest.Should().ThrowExactly<ArgumentException>()
                               .WithMessage("UnitPrice and Quotes must be higher than 0");
        }
    }
}
