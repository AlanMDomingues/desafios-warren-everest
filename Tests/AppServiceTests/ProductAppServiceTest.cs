using Application.Models.Requests;
using Application.Models.Response;
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

namespace Tests.AppServiceTests
{
    public class ProductAppServiceTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductAppService _productAppService;

        public ProductAppServiceTest(IMapper mapper)
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
            var actionTest = _productAppService.AnyProductForId(It.IsAny<int>());

            // Assert
            actionTest.Should().As<bool>();
            _productServiceMock.Verify(x => x.AnyProductForId(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Add()
        {
            // Arrange
            var createProductRequest = ProductFactory.FakeCreateProductRequest();

            // Act
            _productAppService.Add(createProductRequest);

            // Assert
            _productServiceMock.Verify(x => x.Add(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Update()
        {
            // Arrange
            var updateProductRequest = ProductFactory.FakeUpdateProductRequest();

            _productServiceMock.Setup(x => x.AnyProductForId(It.IsAny<int>())).Returns(true);

            // Act
            _productAppService.Update(It.IsAny<int>(), updateProductRequest);

            // Assert
            _productServiceMock.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Delete()
        {
            // Act
            _productAppService.Delete(It.IsAny<int>());

            // Assert
            _productServiceMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
        }
    }
}
