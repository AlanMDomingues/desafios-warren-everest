using Application.Interfaces;
using Application.Models.Requests;
using Application.Models.Response;
using Application.Profiles;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Domain.Services.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Tests.Factories;
using Xunit;

namespace Tests
{
    public class PortfolioAppServiceTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IPortfolioService> _portfolioServiceMock;
        private readonly Mock<ICustomerBankInfoAppService> _customerBankInfoAppServiceMock;
        private readonly Mock<IProductAppService> _productAppServiceMock;
        private readonly Mock<IOrderAppService> _orderAppServiceMock;
        private readonly Mock<IPortfolioProductService> _portfolioProductServiceMock;
        private readonly Mock<IInvestmentService> _investmentServiceMock;
        private readonly PortfolioAppService _portfolioAppService;
        public PortfolioAppServiceTest(IMapper mapper)
        {
            _mapper = mapper;
            _portfolioServiceMock = new Mock<IPortfolioService>();
            _customerBankInfoAppServiceMock = new Mock<ICustomerBankInfoAppService>();
            _productAppServiceMock = new Mock<IProductAppService>();
            _orderAppServiceMock = new Mock<IOrderAppService>();
            _portfolioProductServiceMock = new Mock<IPortfolioProductService>();
            _investmentServiceMock = new Mock<IInvestmentService>();
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
        public void Should_Pass_And_Return_All_Portfolios_List_When_Trying_To_GetAll_Portfolios_By_A_Customer()
        {
            // Arrange
            var fakePortfolios = PortfolioFactory.CreatePortfolios();

            var finalResult = _mapper.Map<IEnumerable<PortfolioResult>>(fakePortfolios);
            foreach (var portfoliosResult in finalResult)
            {
                foreach (var portfolio in fakePortfolios)
                {
                    portfoliosResult.Products = _mapper.Map<IEnumerable<PortfolioProductResult>>(portfolio.PortfoliosProducts);
                }
            }

            _portfolioServiceMock.Setup(x => x.GetAll(It.IsAny<int>())).Returns(fakePortfolios);

            // Act
            var actionResult = _portfolioAppService.GetAll(It.IsAny<int>());

            // Assert
            actionResult.Should().BeEquivalentTo(finalResult);
        }

        [Fact]
        public void Should_Pass_And_Return_Portfolio_When_Trying_To_Get_A_Portfolio()
        {
            // Arrange
            var fakePortfolio = PortfolioFactory.CreatePortfolio();

            var finalResult = _mapper.Map<PortfolioResult>(fakePortfolio);
            finalResult.Products = _mapper.Map<IEnumerable<PortfolioProductResult>>(fakePortfolio.PortfoliosProducts);

            _portfolioServiceMock.Setup(x => x.Get(It.IsAny<int>())).Returns(fakePortfolio);

            // Act
            var actionResult = _portfolioAppService.Get(It.IsAny<int>());

            // Assert
            actionResult.Should().BeEquivalentTo(finalResult);
        }

        [Fact]
        public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Add_A_Portfolio_Doesnt_Exists()
        {
            // Arrange

            var portfolio = PortfolioFactory.CreatePortfolioRequest();
            _customerBankInfoAppServiceMock.Setup(x => x.AnyCustomerBankInfoForId(portfolio.CustomerId)).Returns(false);

            // Act
            var actionResult = () => _portfolioAppService.Add(portfolio);

            // Assert
            actionResult.Should().ThrowExactly<ArgumentException>().WithMessage($"'Customer' not found for ID: {portfolio.CustomerId}");
        }

        [Fact]
        public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Update_A_Portfolio_Doesnt_Exists()
        {
            // Arrange
            var id = 1;
            var updatePortfolio = PortfolioFactory.UpdatePortfolioRequest();

            _portfolioServiceMock.Setup(x => x.AnyPortfolioForId(id)).Returns(false);

            // Act
            var actionResult = () => _portfolioAppService.Update(id, updatePortfolio);

            // Assert
            actionResult.Should().ThrowExactly<ArgumentException>().WithMessage($"'Portfolio' not found for ID: {id}");
        }

        [Fact]
        public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Delete_A_Portfolio_Doesnt_Exists()
        {
            // Arrange
            var id = 1;

            _portfolioServiceMock.Setup(x => x.AnyPortfolioForId(id)).Returns(false);

            // Act
            var actionResult = () => _portfolioAppService.Delete(id);

            // Assert
            actionResult.Should().ThrowExactly<ArgumentException>().WithMessage($"'Portfolio' not found for ID: {id}");
        }

        [Fact]
        public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Delete_A_Portfolio()
        {
            // Arrange
            var id = 1;
            _portfolioServiceMock.Setup(x => x.AnyPortfolioForId(id)).Returns(true);
            _portfolioServiceMock.Setup(x => x.AnyPortfolioFromACustomerArentEmpty(id)).Returns(true);

            // Act
            var actionResult = () => _portfolioAppService.Delete(id);

            // Assert
            actionResult.Should().ThrowExactly<ArgumentException>().WithMessage("You must withdraw money from the portfolio before deleting it");
        }

        [Fact]
        public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Invest_For_CustomerBankInfo_Doesnt_Exists()
        {
            // Arrange
            var customerBankInfoId = 1;
            var order = OrderFactory.CreateOrderRequest();

            _customerBankInfoAppServiceMock.Setup(x => x.AnyCustomerBankInfoForId(customerBankInfoId)).Returns(false);

            // Act
            var actionResult = () => _portfolioAppService.Invest(customerBankInfoId, order);

            // Assert
            actionResult.Should().ThrowExactly<ArgumentException>().WithMessage($"'Customer' not found for ID: {customerBankInfoId}");
        }

        [Fact]
        public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Invest_For_Portfolio_Doesnt_Exists()
        {
            // Arrange
            var customerBankInfoId = 1;
            var order = OrderFactory.CreateOrderRequest();

            _customerBankInfoAppServiceMock.Setup(x => x.AnyCustomerBankInfoForId(customerBankInfoId)).Returns(true);
            _portfolioServiceMock.Setup(x => x.AnyPortfolioForId(order.PortfolioId)).Returns(false);

            // Act
            var actionResult = () => _portfolioAppService.Invest(customerBankInfoId, order);

            // Assert
            actionResult.Should().ThrowExactly<ArgumentException>().WithMessage($"'Portfolio' not found for ID: {order.PortfolioId}");
        }

        [Fact]
        public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Invest_For_Product_Doesnt_Exists()
        {
            // Arrange
            var customerBankInfoId = 1;
            var order = OrderFactory.CreateOrderRequest();

            _customerBankInfoAppServiceMock.Setup(x => x.AnyCustomerBankInfoForId(customerBankInfoId)).Returns(true);
            _portfolioServiceMock.Setup(x => x.AnyPortfolioForId(order.PortfolioId)).Returns(true);
            // NÃO FIZ SETUP DO GET PRODUCT

            // Act
            var actionResult = () => _portfolioAppService.Invest(customerBankInfoId, order);

            // Assert
            actionResult.Should().ThrowExactly<ArgumentException>().WithMessage($"'Product' not found for ID: {order.ProductId}");
        }
    }
}
