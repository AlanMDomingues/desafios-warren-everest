using API.Tests.Fixtures;
using Application.Interfaces;
using Application.Models.Response;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Domain.Services.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace API.Tests.ApplicationTests
{
    public class PortfolioAppServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IPortfolioService> _portfolioServiceMock;
        private readonly Mock<ICustomerBankInfoAppService> _customerBankInfoAppServiceMock;
        private readonly Mock<IProductAppService> _productAppServiceMock;
        private readonly Mock<IOrderAppService> _orderAppServiceMock;
        private readonly Mock<IPortfolioProductService> _portfolioProductServiceMock;
        private readonly Mock<IInvestmentService> _investmentServiceMock;
        private readonly PortfolioAppService _portfolioAppService;
        public PortfolioAppServiceTests(IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _portfolioServiceMock = new();
            _customerBankInfoAppServiceMock = new();
            _productAppServiceMock = new();
            _orderAppServiceMock = new();
            _portfolioProductServiceMock = new();
            _investmentServiceMock = new();
            _portfolioAppService = new(
                _mapper,
                _portfolioServiceMock.Object,
                _customerBankInfoAppServiceMock.Object,
                _productAppServiceMock.Object,
                _orderAppServiceMock.Object,
                _portfolioProductServiceMock.Object,
                _investmentServiceMock.Object);
        }

        [Fact]
        public void Should_Pass_And_Return_All_Portfolios_List_When_Trying_To_Get_All_Portfolios_By_A_Customer()
        {
            // Arrange
            var fakePortfolios = PortfolioFactory.FakePortfolios();

            var fakePortfoliosResultExpected = _mapper.Map<IEnumerable<PortfolioResult>>(fakePortfolios);

            _portfolioServiceMock.Setup(x => x.GetAll(It.IsAny<int>())).Returns(fakePortfolios);

            // Act
            var actionResult = _portfolioAppService.GetAll(It.IsAny<int>());

            // Assert
            actionResult.Should().BeEquivalentTo(fakePortfoliosResultExpected);
            _portfolioServiceMock.Verify(x => x.GetAll(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Should_Pass_And_Return_Portfolio_When_Trying_To_Get_A_Portfolio()
        {
            // Arrange
            var fakePortfolio = PortfolioFactory.FakePortfolio();

            var fakePortfolioResultExpected = _mapper.Map<PortfolioResult>(fakePortfolio);

            _portfolioServiceMock.Setup(x => x.Get(It.IsAny<int>())).Returns(fakePortfolio);

            // Act
            var actionResult = _portfolioAppService.Get(It.IsAny<int>());

            // Assert
            actionResult.Should().BeEquivalentTo(fakePortfolioResultExpected);
            _portfolioServiceMock.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Add_A_Portfolio_Doesnt_Exists()
        {
            // Arrange
            var portfolio = PortfolioFactory.FakeCreatePortfolioRequest();
            _customerBankInfoAppServiceMock.Setup(x => x.AnyCustomerBankInfoForId(It.IsAny<int>())).Returns(false);

            // Act
            var actionResult = () => _portfolioAppService.Add(portfolio);

            // Assert
            actionResult.Should().ThrowExactly<ArgumentException>().WithMessage($"'Customer' não encontrado para o ID: {portfolio.CustomerId}");
            _customerBankInfoAppServiceMock.Verify(x => x.AnyCustomerBankInfoForId(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Add_A_Portfolio()
        {
            // Arrange
            var portfolio = PortfolioFactory.FakeCreatePortfolioRequest();
            _customerBankInfoAppServiceMock.Setup(x => x.AnyCustomerBankInfoForId(It.IsAny<int>())).Returns(true);

            // Act
            var actionResult = () => _portfolioAppService.Add(portfolio);

            // Assert
            actionResult.Should().NotThrow();
            _customerBankInfoAppServiceMock.Verify(x => x.AnyCustomerBankInfoForId(It.IsAny<int>()), Times.Once);
            _portfolioServiceMock.Verify(x => x.Add(It.IsAny<Portfolio>()), Times.Once);
        }

        [Fact]
        public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Update_A_Portfolio_Doesnt_Exists()
        {
            // Arrange
            var id = 1;
            var updatePortfolio = PortfolioFactory.FakeUpdatePortfolioRequest();

            _portfolioServiceMock.Setup(x => x.AnyPortfolioForId(It.IsAny<int>())).Returns(false);

            // Act
            var actionResult = () => _portfolioAppService.Update(id, updatePortfolio);

            // Assert
            actionResult.Should().ThrowExactly<ArgumentException>().WithMessage($"'Portfolio' não encontrado para o ID: {id}");
            _portfolioServiceMock.Verify(x => x.AnyPortfolioForId(It.IsAny<int>()), Times.Once);
            _portfolioServiceMock.Verify(x => x.Update(It.IsAny<Portfolio>()), Times.Never);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Update_A_Portfolio()
        {
            // Arrange
            var updatePortfolio = PortfolioFactory.FakeUpdatePortfolioRequest();

            _portfolioServiceMock.Setup(x => x.AnyPortfolioForId(It.IsAny<int>())).Returns(true);

            // Act
            var actionResult = () => _portfolioAppService.Update(It.IsAny<int>(), updatePortfolio);

            // Assert
            actionResult.Should().NotThrow();
            _portfolioServiceMock.Verify(x => x.AnyPortfolioForId(It.IsAny<int>()), Times.Once);
            _portfolioServiceMock.Verify(x => x.Update(It.IsAny<Portfolio>()), Times.Once);
        }

        [Fact]
        public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Delete_A_Portfolio_Doesnt_Exists()
        {
            // Arrange
            var id = 1;
            _portfolioServiceMock.Setup(x => x.AnyPortfolioForId(It.IsAny<int>())).Returns(false);

            // Act
            var actionResult = () => _portfolioAppService.Delete(id);

            // Assert
            actionResult.Should().ThrowExactly<ArgumentException>().WithMessage($"'Portfolio' não encontrado para o ID: {id}");
            _portfolioServiceMock.Verify(x => x.AnyPortfolioForId(It.IsAny<int>()), Times.Once);
            _portfolioServiceMock.Verify(x => x.AnyPortfolioFromACustomerArentEmpty(It.IsAny<int>()), Times.Never);
            _portfolioServiceMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Delete_A_Portfolio_Doesnt_Exists()
        {
            // Arrange
            _portfolioServiceMock.Setup(x => x.AnyPortfolioForId(It.IsAny<int>())).Returns(true);

            // Act
            var actionResult = () => _portfolioAppService.Delete(It.IsAny<int>());

            // Assert
            actionResult.Should().NotThrow();
            _portfolioServiceMock.Verify(x => x.AnyPortfolioForId(It.IsAny<int>()), Times.Once);
            _portfolioServiceMock.Verify(x => x.AnyPortfolioFromACustomerArentEmpty(It.IsAny<int>()), Times.Once);
            _portfolioServiceMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Delete_A_Portfolio_Arent_Empty()
        {
            // Arrange
            _portfolioServiceMock.Setup(x => x.AnyPortfolioForId(It.IsAny<int>())).Returns(true);
            _portfolioServiceMock.Setup(x => x.AnyPortfolioFromACustomerArentEmpty(It.IsAny<int>())).Returns(true);

            // Act
            var actionResult = () => _portfolioAppService.Delete(It.IsAny<int>());

            // Assert
            actionResult.Should().ThrowExactly<ArgumentException>().WithMessage("Você precisa sacar o saldo das sua carteira antes de deletá-la");
            _portfolioServiceMock.Verify(x => x.AnyPortfolioForId(It.IsAny<int>()), Times.Once);
            _portfolioServiceMock.Verify(x => x.AnyPortfolioFromACustomerArentEmpty(It.IsAny<int>()), Times.Once);
            _portfolioServiceMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Invest_For_CustomerBankInfo_Doesnt_Exists()
        {
            // Arrange
            var order = OrderFactory.FakeCreateOrderRequest();

            _customerBankInfoAppServiceMock.Setup(x => x.AnyCustomerBankInfoForId(It.IsAny<int>())).Returns(false);

            // Act
            var actionResult = () => _portfolioAppService.Invest(order);

            // Assert
            actionResult.Should().ThrowExactly<ArgumentException>().WithMessage($"'Customer' não encontrado para o ID: {order.CustomerBankInfoId}");
            _customerBankInfoAppServiceMock.Verify(x => x.AnyCustomerBankInfoForId(It.IsAny<int>()), Times.Once);
            _portfolioServiceMock.Verify(x => x.AnyPortfolioForId(It.IsAny<int>()), Times.Never);
            _productAppServiceMock.Verify(x => x.Get(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Invest_For_Portfolio_Doesnt_Exists()
        {
            // Arrange
            var order = OrderFactory.FakeCreateOrderRequest();

            _customerBankInfoAppServiceMock.Setup(x => x.AnyCustomerBankInfoForId(It.IsAny<int>())).Returns(true);
            _portfolioServiceMock.Setup(x => x.AnyPortfolioForId(It.IsAny<int>())).Returns(false);

            // Act
            var actionResult = () => _portfolioAppService.Invest(order);

            // Assert
            actionResult.Should().ThrowExactly<ArgumentException>().WithMessage($"'Portfolio' não encontrado para o ID: {order.PortfolioId}");
            _customerBankInfoAppServiceMock.Verify(x => x.AnyCustomerBankInfoForId(It.IsAny<int>()), Times.Once);
            _portfolioServiceMock.Verify(x => x.AnyPortfolioForId(It.IsAny<int>()), Times.Once);
            _productAppServiceMock.Verify(x => x.Get(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Invest_For_Product_Doesnt_Exists()
        {
            // Arrange
            var order = OrderFactory.FakeCreateOrderRequest();

            _customerBankInfoAppServiceMock.Setup(x => x.AnyCustomerBankInfoForId(It.IsAny<int>())).Returns(true);
            _portfolioServiceMock.Setup(x => x.AnyPortfolioForId(It.IsAny<int>())).Returns(true);

            // Act
            var actionResult = () => _portfolioAppService.Invest(order);

            // Assert
            actionResult.Should().ThrowExactly<ArgumentException>().WithMessage($"'Product' não encontrado para o ID: {order.ProductId}");
            _customerBankInfoAppServiceMock.Verify(x => x.AnyCustomerBankInfoForId(It.IsAny<int>()), Times.Once);
            _portfolioServiceMock.Verify(x => x.AnyPortfolioForId(It.IsAny<int>()), Times.Once);
            _productAppServiceMock.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Should_Pass_And_Return_ArgumentException_When_Trying_To_Invest_For_Product()
        {
            // Arrange
            var createOrderRequest = OrderFactory.FakeCreateOrderRequest();
            var productResult = ProductFactory.FakeProductResult();

            _customerBankInfoAppServiceMock.Setup(x => x.AnyCustomerBankInfoForId(It.IsAny<int>())).Returns(true);
            _portfolioServiceMock.Setup(x => x.AnyPortfolioForId(It.IsAny<int>())).Returns(true);
            _productAppServiceMock.Setup(x => x.Get(It.IsAny<int>())).Returns(productResult);

            // Act
            var actionResult = () => _portfolioAppService.Invest(createOrderRequest);

            // Assert
            actionResult.Should().NotThrow();
            _customerBankInfoAppServiceMock.Verify(x => x.AnyCustomerBankInfoForId(It.IsAny<int>()), Times.Once);
            _portfolioServiceMock.Verify(x => x.AnyPortfolioForId(It.IsAny<int>()), Times.Once);
            _productAppServiceMock.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Pass_When_Trying_To_Call_AnyPortfolioFromACustomerArentEmpty(bool trueOrFalse)
        {
            // Arrange
            _portfolioServiceMock.Setup(x => x.AnyPortfolioFromACustomerArentEmpty(It.IsAny<int>())).Returns(trueOrFalse);

            // Act
            var actionTest = _portfolioAppService.AnyPortfolioFromACustomerArentEmpty(It.IsAny<int>());

            // Assert
            actionTest.Should().Be(trueOrFalse);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Transfer_Money_To_Account_Balance()
        {
            // Act
            _portfolioAppService.TransferMoneyToAccountBalance(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>());

            // Assert
            _investmentServiceMock.Verify(x => x.DepositMoneyInCustomerBankInfo(It.IsAny<int>(), It.IsAny<decimal>()), Times.Once);
            _portfolioServiceMock.Verify(x => x.Withdraw(It.IsAny<int>(), It.IsAny<decimal>()), Times.Once);
        }
    }
}
