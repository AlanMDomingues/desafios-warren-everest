using Application.Models.Response;
using Application.Profiles;
using Application.Services;
using AutoMapper;
using Domain.Services.Interfaces;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Tests.Factories;
using Xunit;

namespace Tests
{
    public class ProductAppServiceTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductAppService _productAppService;

        public ProductAppServiceTest(IMapper mapper)
        {
            _mapper = mapper;
            _productServiceMock = new Mock<IProductService>();
            _productAppService = new(_mapper, _productServiceMock.Object);
        }

        [Fact]
        public void Should_Pass_And_Return_All_Products_When_Trying_To_Get_All_Products()
        {
            // Arrange
            var products = ProductFactory.CreateProduct();

            var productExpected = _mapper.Map<IEnumerable<ProductResult>>(products);
            _productServiceMock.Setup(x => x.GetAll()).Returns(products);

            // Act
            var actionResult = _productAppService.GetAll();

            // Assert
            actionResult.Should().BeEquivalentTo(productExpected);
        }
    }
}
