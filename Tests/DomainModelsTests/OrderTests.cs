using API.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace API.Tests.DomainModelsTests
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
    }
}
