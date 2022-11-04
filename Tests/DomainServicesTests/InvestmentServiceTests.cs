using API.Tests.Fixtures;
using Domain.Services.Services;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using FluentAssertions;
using Infrastructure.Data.Context;
using Xunit;

namespace API.Tests.DomainServicesTests
{
    [Collection(nameof(DatabaseFixtureCollection))]
    public class InvestmentServiceTests
    {
        private readonly PortfolioService _portfolioService;
        private readonly CustomerBankInfoService _customerBankInfoService;
        private readonly InvestmentService _investmentService;
        private readonly IRepositoryFactory<DataContext> _repositoryFactory;
        private readonly IUnitOfWork<DataContext> _unitOfWork;

        public InvestmentServiceTests(DatabaseFixture fixture)
        {
            var unitOfWork = fixture.CreateDbContext();
            var repositoryFactory = (IRepositoryFactory<DataContext>)unitOfWork;
            _repositoryFactory = repositoryFactory;
            _unitOfWork = unitOfWork;
            _investmentService = new(_repositoryFactory);
            _portfolioService = new(_repositoryFactory, _unitOfWork);
            _customerBankInfoService = new(_unitOfWork, _repositoryFactory);
        }

        //[Fact]
        //public void Should_Pass_When_Trying_To_Deposit_Money_In_Portfolio()
        //{
        //    // Arrange
        //    var portfolio = PortfolioFactory.FakePortfolio();
        //    portfolio.TotalBalance = 0.00m;
        //    _portfolioService.Add(portfolio);
        //    var amount = 100.00m;

        //    // Act
        //    _investmentService.DepositMoneyInPortfolio(portfolio.Id, amount);

        //    // Assert
        //    var result = _portfolioService.Get(portfolio.Id);
        //    result.TotalBalance.Should().Be(100.00m);
        //}
    }
}
