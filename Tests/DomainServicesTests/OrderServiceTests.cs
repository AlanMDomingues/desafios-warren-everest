using API.Tests.Fixtures;
using Domain.Services.Services;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using FluentAssertions;
using Infrastructure.Data.Context;
using Xunit;

namespace API.Tests.DomainServicesTests
{
    [Collection(nameof(DatabaseFixtureCollection))]
    public class OrderServiceTests
    {
        private readonly OrderService _orderService;
        private readonly IUnitOfWork<DataContext> _unitOfWork;

        public OrderServiceTests(DatabaseFixture fixture)
        {
            var unitOfWork = fixture.CreateDbContext();
            _unitOfWork = unitOfWork;
            var repositoryFactory = (IRepositoryFactory<DataContext>)unitOfWork;
            _orderService = new(repositoryFactory, _unitOfWork);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Get_All_Orders_For_PortfolioId()
        {
            // Arrange
            var orders = OrderFactory.FakeOrders();
            orders[0].PortfolioId = 9;
            orders[1].PortfolioId = 10;
            orders[2].PortfolioId = 1;
            orders[3].PortfolioId = 1;
            orders[4].PortfolioId = 1;

            foreach (var item in orders)
            {
                _orderService.Add(item);
            }

            // Act
            var result = _orderService.GetAll(1);

            // Assert
            result.Should().HaveCount(3);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Get_An_Order()
        {
            // Arrange
            var order = OrderFactory.FakeOrder();
            _orderService.Add(order);

            // Act
            var result = _orderService.Get(1);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Add_An_Order()
        {
            // Arrange
            var order = OrderFactory.FakeOrder();

            // Act
            _orderService.Add(order);

            // Assert
            var result = _orderService.Get(1);
            result.Should().NotBeNull();
        }
    }
}
