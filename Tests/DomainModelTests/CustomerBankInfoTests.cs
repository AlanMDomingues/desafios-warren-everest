using FluentAssertions;
using Tests.Factories;
using Xunit;

namespace Tests.DomainModelTests
{
    public class CustomerBankInfoTests
    {
        [Fact]
        public void Should_Pass_When_Trying_To_Validate_A_Transaction()
        {
            // Arrange
            var customerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();
            customerBankInfo.AccountBalance = 200.00m;

            // Act
            var actionTest = customerBankInfo.ValidateTransaction(100.00m);

            // Assert
            actionTest.Should().BeTrue();
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Validate_A_Transaction_For_An_Amount_Equal_To_The_Account_Balance()
        {
            // Arrange
            var customerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();
            customerBankInfo.AccountBalance = 200.00m;

            // Act
            var actionTest = customerBankInfo.ValidateTransaction(200.00m);

            // Assert
            actionTest.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Validate_A_Transaction_For_An_Amount_Higher_Than_AccountBalance()
        {
            // Arrange
            var customerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();
            customerBankInfo.AccountBalance = 200.00m;

            // Act
            var actionTest = customerBankInfo.ValidateTransaction(300.00m);

            // Assert
            actionTest.Should().BeFalse();
        }
    }
}
