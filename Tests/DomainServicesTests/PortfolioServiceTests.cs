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
    public class PortfolioServiceTests
    {
        private readonly PortfolioService _portfolioService;
        private readonly IUnitOfWork<DataContext> _unitOfWork;

        public PortfolioServiceTests(DatabaseFixture fixture)
        {
            var unitOfWork = fixture.CreateDbContext();
            _unitOfWork = unitOfWork;
            var repositoryFactory = (IRepositoryFactory<DataContext>)unitOfWork;
            _portfolioService = new(repositoryFactory, _unitOfWork);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Get_All_Portfolios_By_A_Customer()
        {
            // Arrange
            var portfolios = PortfolioFactory.FakePortfolios();

            portfolios[0].CustomerId = 1;
            portfolios[1].CustomerId = 1;
            portfolios[2].CustomerId = 1;
            portfolios[3].CustomerId = 2;
            portfolios[4].CustomerId = 2;

            foreach (var item in portfolios)
            {
                _portfolioService.Add(item);
            }

            // Act
            var result = _portfolioService.GetAll(1);

            // Assert
            result.Should().HaveCount(3);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Find_Any_Portfolio_From_A_Customer_Arent_Empty()
        {
            // Arrange
            var portfolio = PortfolioFactory.FakePortfolio();
            portfolio.CustomerId = 1;
            portfolio.TotalBalance = 100.00m;
            _portfolioService.Add(portfolio);

            // Act
            var result = _portfolioService.AnyPortfolioFromACustomerArentEmpty(portfolio.CustomerId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Find_Any_Portfolio_From_A_Customer_Arent_Empty()
        {
            // Arrange
            var portfolio = PortfolioFactory.FakePortfolio();
            portfolio.CustomerId = 1;
            portfolio.TotalBalance = 00.00m;
            _portfolioService.Add(portfolio);

            // Act
            var result = _portfolioService.AnyPortfolioFromACustomerArentEmpty(portfolio.CustomerId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Find_Any_Portfolio_For_Id()
        {
            // Arrange
            var portfolio = PortfolioFactory.FakePortfolio();
            _portfolioService.Add(portfolio);

            // Act
            var result = _portfolioService.AnyPortfolioForId(portfolio.Id);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Find_Any_Portfolio_For_Id()
        {
            // Arrange
            var portfolio = PortfolioFactory.FakePortfolio();
            portfolio.Id = 2;
            _portfolioService.Add(portfolio);

            // Act
            var result = _portfolioService.AnyPortfolioForId(1);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Add_Portfolio()
        {
            // Arrange
            var portfolio = PortfolioFactory.FakePortfolio();

            // Act
            _portfolioService.Add(portfolio);

            // Assert
            var result = _portfolioService.Get(portfolio.Id);
            result.Should().NotBeNull();
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Update_Portfolio()
        {
            // Arrange
            var portfolio = PortfolioFactory.FakePortfolio();
            _portfolioService.Add(portfolio);
            portfolio.Name = "Teste";

            // Act
            _portfolioService.Update(portfolio);

            // Assert
            var result = _portfolioService.Get(portfolio.Id);
            result.Name.Should().Be("Teste");
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Delete_Portfolio()
        {
            // Arrange
            var portfolio = PortfolioFactory.FakePortfolio();
            _portfolioService.Add(portfolio);

            // Act
            _portfolioService.Delete(portfolio.Id);

            // Assert
            var result = _portfolioService.Get(portfolio.Id);
            result.Should().BeNull();
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Withdraw_Money_From_Portfolio()
        {
            // Arrange
            var portfolio = PortfolioFactory.FakePortfolio();
            portfolio.TotalBalance = 100.00m;
            _portfolioService.Add(portfolio);
            var amount = 50.00m;

            // Act
            _portfolioService.Withdraw(portfolio.Id, amount);

            // Assert
            var result = _portfolioService.Get(portfolio.Id);
            result.TotalBalance.Should().Be(50.00m);
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Withdraw_Money_From_Portfolio_That_Doesnt_Exist()
        {
            // Arrange
            var portfolio = PortfolioFactory.FakePortfolio();
            portfolio.TotalBalance = 100.00m;
            _portfolioService.Add(portfolio);
            var amount = 50.00m;
            var id = 110;

            // Act
            var result = () => _portfolioService.Withdraw(id, amount);

            // Assert
            result.Should().ThrowExactly<ArgumentException>()
                           .WithMessage($"'Portfolio' não encontrado para o ID: {id}");
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Withdraw_Money_From_Portfolio_Because_The_Balance_Is_Insufficient()
        {
            // Arrange
            var portfolio = PortfolioFactory.FakePortfolio();
            portfolio.TotalBalance = 100.00m;
            _portfolioService.Add(portfolio);
            var amount = 200.00m;

            // Act
            var result = () => _portfolioService.Withdraw(portfolio.Id, amount);

            // Assert
            result.Should().ThrowExactly<ArgumentException>()
                           .WithMessage("Saldo insuficiente");
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Deposit_Money_For_Portfolio()
        {
            // Arrange
            var portfolio = PortfolioFactory.FakePortfolio();
            portfolio.TotalBalance = 100.00m;
            _portfolioService.Add(portfolio);
            var amount = 50.00m;

            // Act
            _portfolioService.Deposit(portfolio.Id, amount);

            // Assert
            var result = _portfolioService.Get(portfolio.Id);
            result.TotalBalance.Should().Be(150.00m);
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Deposit_Money_For_Portfolio_That_Doesnt_Exist()
        {
            // Arrange
            var portfolio = PortfolioFactory.FakePortfolio();
            _portfolioService.Add(portfolio);
            var amount = 50.00m;
            var id = 110;

            // Act
            var result = () => _portfolioService.Deposit(id, amount);

            // Assert
            result.Should().ThrowExactly<ArgumentException>()
                           .WithMessage($"'Portfolio' não encontrado para o ID: {id}");
        }
    }
}
