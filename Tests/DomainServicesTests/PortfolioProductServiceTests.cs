using API.Tests.Fixtures;
using Domain.Services.Services;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using FluentAssertions;
using Infrastructure.Data.Context;
using System.Linq;
using Xunit;

namespace API.Tests.DomainServicesTests
{
    [Collection(nameof(DatabaseFixtureCollection))]
    public class PortfolioProductServiceTests
    {
        private readonly PortfolioProductService _portfolioProductService;
        private readonly IUnitOfWork<DataContext> _unitOfWork;

        public PortfolioProductServiceTests(DatabaseFixture fixture)
        {
            var unitOfWork = fixture.CreateDbContext();
            _unitOfWork = unitOfWork;
            var repositoryFactory = (IRepositoryFactory<DataContext>)unitOfWork;
            _portfolioProductService = new(repositoryFactory, _unitOfWork);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Add_PortfolioProduct()
        {
            // Arrange
            var portfolioProduct = PortfolioProductFactory.FakePortfolioProduct();

            // Act
            var actionTest = () => _portfolioProductService.Add(portfolioProduct.First().PortfolioId, portfolioProduct.First().ProductId);

            // Assert
            actionTest.Should().NotThrow();
        }
    }
}
