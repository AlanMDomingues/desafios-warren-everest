using API.Tests.Fixtures;
using Domain.Services.Interfaces;
using Domain.Services.Services;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using FluentAssertions;
using Infrastructure.Data.Context;
using Moq;
using Xunit;

namespace API.Tests.DomainServicesTests
{
    [Collection(nameof(DatabaseFixtureCollection))]
    public class InvestmentServiceTests
    {
        private readonly PortfolioService _portfolioService;
        private readonly CustomerBankInfoService _customerBankInfoService;
        private readonly InvestmentService _investmentService;
        private readonly Mock<IRepositoryFactory<DataContext>> _repositoryFactory;
        private readonly IUnitOfWork<DataContext> _unitOfWork;

        public InvestmentServiceTests(DatabaseFixture fixture)
        {
            var unitOfWork = fixture.CreateDbContext();
            var repositoryFactory = (IRepositoryFactory<DataContext>)unitOfWork;
            _repositoryFactory = new();
            _unitOfWork = unitOfWork;
            _investmentService = new(_repositoryFactory.Object);
            _portfolioService = new(repositoryFactory, _unitOfWork);
            _customerBankInfoService = new(_unitOfWork, repositoryFactory);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Deposit_Money_In_Portfolio()
        {
            // Arrange
            var portfolio = PortfolioFactory.FakePortfolio();
            portfolio.TotalBalance = 0.00m;
            _portfolioService.Add(portfolio);
            var amount = 100.00m;
            _repositoryFactory.Setup(x => x.CustomRepository<IPortfolioService>()).Returns(_portfolioService);
            _repositoryFactory.Setup(x => x.CustomRepository<ICustomerBankInfoService>()).Returns(_customerBankInfoService);

            // Act
            _investmentService.DepositMoneyInPortfolio(portfolio.Id, amount);

            // Assert
            var result = _portfolioService.Get(portfolio.Id);
            result.TotalBalance.Should().Be(100.00m);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Deposit_Money_In_CustomerBankInfo()
        {
            // Arrange
            var customerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();
            customerBankInfo.AccountBalance = 0.00m;
            _customerBankInfoService.Add(customerBankInfo);
            var amount = 100.00m;
            _repositoryFactory.Setup(x => x.CustomRepository<IPortfolioService>()).Returns(_portfolioService);
            _repositoryFactory.Setup(x => x.CustomRepository<ICustomerBankInfoService>()).Returns(_customerBankInfoService);

            // Act
            _investmentService.DepositMoneyInCustomerBankInfo(customerBankInfo.Id, amount);

            // Assert
            var result = _customerBankInfoService.Get(customerBankInfo.Id);
            result.AccountBalance.Should().Be(100.00m);
        }
    }
}
