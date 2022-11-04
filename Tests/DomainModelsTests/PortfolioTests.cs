using API.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace API.Tests.DomainModelsTests
{
    public class PortfolioTests
    {
        [Theory]
        [InlineData(100.00)]
        [InlineData(200.00)]
        public void Should_Pass_When_Trying_To_Validate_A_Transaction(decimal amount)
        {
            // Arrange
            var portfolio = PortfolioFactory.FakePortfolio();
            portfolio.TotalBalance = 200.00m;

            // Act
            var actionTest = portfolio.ValidateTransaction(amount);

            // Assert
            actionTest.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Validate_A_Transaction_For_An_Amount_Higher_Than_AccountBalance()
        {
            // Arrange
            var portfolio = PortfolioFactory.FakePortfolio();
            portfolio.TotalBalance = 200.00m;

            // Act
            var actionTest = portfolio.ValidateTransaction(300.00m);

            // Assert
            actionTest.Should().BeFalse();
        }
    }
}
