using API.Tests.Fixtures;
using Domain.Services.Services;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using FluentAssertions;
using Infrastructure.Data.Context;
using System;
using Xunit;

namespace API.Tests.DomainServicesTests
{
    [Collection(nameof(DatabaseFixtureCollection))]
    public class CustomerBankInfoServiceTests
    {
        private readonly CustomerBankInfoService _customerBankInfoService;
        private readonly IUnitOfWork<DataContext> _unitOfWork;

        public CustomerBankInfoServiceTests(DatabaseFixture fixture)
        {
            var unitOfWork = fixture.CreateDbContext();
            _unitOfWork = unitOfWork;
            var repositoryFactory = (IRepositoryFactory<DataContext>)unitOfWork;
            _customerBankInfoService = new(_unitOfWork, repositoryFactory);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Add_New_CustomerBankInfo()
        {
            // Arrange
            var customerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();

            // Act
            var actionTest = () => _customerBankInfoService.Add(customerBankInfo);

            // Assert
            actionTest.Should().NotThrow();
            var result = _customerBankInfoService.Get(customerBankInfo.Id);
            result.Id.Should().Be(1);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Get_A_CustomerBankInfo()
        {
            // Arrange
            var customerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();
            _customerBankInfoService.Add(customerBankInfo);

            // Act
            var result = _customerBankInfoService.Get(1);

            // Assert
            result.Id.Should().Be(1);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Withdraw_Money_From_AccountBalance()
        {
            // Arrange
            var customerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();
            customerBankInfo.AccountBalance = 200.00m;
            _customerBankInfoService.Add(customerBankInfo);
            var amount = 100.00m;

            // Act
            _customerBankInfoService.Withdraw(customerBankInfo.Id, amount);

            // Assert
            var result = _customerBankInfoService.Get(customerBankInfo.Id);
            result.AccountBalance.Should().Be(100.00m);
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Withdraw_Money_From_AccountBalance_That_Doesnt_Exist()
        {
            // Arrange
            var customerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();
            _customerBankInfoService.Add(customerBankInfo);
            var amount = 100.00m;

            // Act
            var actionTest = () => _customerBankInfoService.Withdraw(1321, amount);

            // Assert
            actionTest.Should().ThrowExactly<ArgumentException>()
                               .WithMessage($"'CustomerBankInfo' não encontrado para o ID: {1321}");
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Withdraw_Money_From_AccountBalance_Greater_Than_Balance()
        {
            // Arrange
            var customerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();
            customerBankInfo.AccountBalance = 200.00m;
            _customerBankInfoService.Add(customerBankInfo);
            var amount = 10000.00m;

            // Act
            var actionTest = () => _customerBankInfoService.Withdraw(customerBankInfo.Id, amount);

            // Assert
            actionTest.Should().ThrowExactly<ArgumentException>()
                               .WithMessage("Saldo insuficiente");
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Withdraw_Money_From_AccountBalance_For_A_Customer_That_Doesnt_Exist()
        {
            // Arrange
            var customerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();
            customerBankInfo.AccountBalance = 200.00m;
            _customerBankInfoService.Add(customerBankInfo);
            var amount = 100.00m;

            // Act
            _customerBankInfoService.Withdraw(customerBankInfo.Id, amount);

            // Assert
            var result = _customerBankInfoService.Get(customerBankInfo.Id);
            result.AccountBalance.Should().Be(100.00m);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Deposit_Money_For_AccountBalance()
        {
            // Arrange
            var customerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();
            customerBankInfo.AccountBalance = 200.00m;
            _customerBankInfoService.Add(customerBankInfo);
            var amount = 100.00m;

            // Act
            _customerBankInfoService.Deposit(customerBankInfo.Id, amount);

            // Assert
            var result = _customerBankInfoService.Get(customerBankInfo.Id);
            result.AccountBalance.Should().Be(300.00m);
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Deposit_Money_For_AccountBalance_That_Doesnt_Exist()
        {
            // Arrange
            var customerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();
            _customerBankInfoService.Add(customerBankInfo);
            var amount = 100.00m;
            var id = 3213;

            // Act
            var actionTest = () => _customerBankInfoService.Deposit(id, amount);

            // Assert
            actionTest.Should().Throw<ArgumentException>()
                               .WithMessage($"'CustomerBankInfo' não encontrado para o ID: {id}");
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Find_Any_AccountBalance_That_Isnt_Zero_For_CustomerId()
        {
            // Arrange
            var customerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();
            customerBankInfo.AccountBalance = 0.00m;
            _customerBankInfoService.Add(customerBankInfo);

            // Act
            var result = _customerBankInfoService.IsAccountBalanceThatIsntZeroForCustomerId(customerBankInfo.Id);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Find_Any_AccountBalance_That_Isnt_Zero_For_CustomerId_Doesnt_Exists()
        {
            // Arrange
            var customerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();
            customerBankInfo.AccountBalance = 00.00m;
            _customerBankInfoService.Add(customerBankInfo);

            // Act
            var result = _customerBankInfoService.IsAccountBalanceThatIsntZeroForCustomerId(1);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Find_Any_CustomerBankInfo_For_Id()
        {
            // Arrange
            var customerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();
            _customerBankInfoService.Add(customerBankInfo);

            // Act
            var result = _customerBankInfoService.AnyCustomerBankInfoForId(customerBankInfo.Id);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Find_Any_CustomerBankInfo_For_Id_That_Doesnt_Exist()
        {
            // Arrange
            var customerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();
            _customerBankInfoService.Add(customerBankInfo);
            var id = 4321;

            // Act
            var result = _customerBankInfoService.AnyCustomerBankInfoForId(id);

            // Assert
            result.Should().BeFalse();
        }
    }
}
