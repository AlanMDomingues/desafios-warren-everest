using API.Tests.Fixtures;
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
    public class ProductAppServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductAppService _productAppService;

        public ProductAppServiceTests(IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _productServiceMock = new();
            _productAppService = new(_mapper, _productServiceMock.Object);
        }

        [Fact]
        public void Should_Pass_And_Return_All_Products_When_Trying_To_Get_All_Products()
        {
            // Arrange
            var products = ProductFactory.FakeProducts();

            var productExpected = _mapper.Map<IEnumerable<ProductResult>>(products);
            _productServiceMock.Setup(x => x.GetAll()).Returns(products);

            // Act
            var actionResult = _productAppService.GetAll();

            // Assert
            actionResult.Should().BeEquivalentTo(productExpected);
            _productServiceMock.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public void Should_Pass_And_Return_A_Product_When_Trying_To_Get_Product()
        {
            // Arrange
            var product = ProductFactory.FakeProduct();

            var productExpected = _mapper.Map<ProductResult>(product);
            _productServiceMock.Setup(x => x.Get(It.IsAny<int>())).Returns(product);

            // Act
            var actionResult = _productAppService.Get(It.IsAny<int>());

            // Assert
            actionResult.Should().BeEquivalentTo(productExpected);
            _productServiceMock.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Call_AnyProductForId()
        {
            // Act
            var actionTest = () => _productAppService.AnyProductForId(It.IsAny<int>());

            // Assert
            actionTest.Should().NotThrow();
            _productServiceMock.Verify(x => x.AnyProductForId(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Add()
        {
            // Arrange
            var createProductRequest = ProductFactory.FakeCreateProductRequest();

            // Act
            var action = () => _productAppService.Add(createProductRequest);

            // Assert
            action.Should().NotThrow();
            _productServiceMock.Verify(x => x.Add(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Update()
        {
            // Arrange
            var updateProductRequest = ProductFactory.FakeUpdateProductRequest();

            // Act
            var action = () => _productAppService.Update(It.IsAny<int>(), updateProductRequest);

            // Assert
            action.Should().NotThrow();
            _productServiceMock.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Delete()
        {
            // Act
            var action = () => _productAppService.Delete(It.IsAny<int>());

            // Assert
            action.Should().NotThrow();
            _productServiceMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
        }
    }
}
