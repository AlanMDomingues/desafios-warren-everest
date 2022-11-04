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
    public class ProductServiceTests
    {
        private readonly ProductService _productService;
        private readonly IUnitOfWork<DataContext> _unitOfWork;

        public ProductServiceTests(DatabaseFixture fixture)
        {
            var unitOfWork = fixture.CreateDbContext();
            _unitOfWork = unitOfWork;
            var repositoryFactory = (IRepositoryFactory<DataContext>)unitOfWork;
            _productService = new(repositoryFactory, _unitOfWork);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Get_All_Products()
        {
            // Arrange
            var products = ProductFactory.FakeProducts();
            products[0].Symbol = "VIIA3";
            products[1].Symbol = "CEDO4F";
            products[2].Symbol = "NFLX34F";
            products[3].Symbol = "NIKE34F";
            products[4].Symbol = "MCDC34F";

            foreach (var item in products)
            {
                _productService.Add(item);
            }

            // Act
            var result = _productService.GetAll();

            // Assert
            result.Should().HaveCount(5);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Get_A_Product()
        {
            // Arrange
            var product = ProductFactory.FakeProduct();
            _productService.Add(product);

            // Act
            var result = _productService.Get(product.Id);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Find_Any_Product_For_Id()
        {
            // Arrange
            var product = ProductFactory.FakeProduct();
            _productService.Add(product);

            // Act
            var result = _productService.AnyProductForId(product.Id);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Find_Any_Product_For_Id_That_Doesnt_Exist()
        {
            // Arrange
            var product = ProductFactory.FakeProduct();
            _productService.Add(product);

            // Act
            var result = _productService.AnyProductForId(12321321);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Add_A_Product()
        {
            // Arrange
            var product = ProductFactory.FakeProduct();

            // Act
            _productService.Add(product);

            // Assert
            var result = _productService.AnyProductForId(product.Id);
            result.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Add_A_Product_That_Already_Exists()
        {
            // Arrange
            var product = ProductFactory.FakeProduct();
            _productService.Add(product);

            // Act
            var result = () => _productService.Add(product);

            // Assert
            result.Should().ThrowExactly<ArgumentException>()
                           .WithMessage($"Produto já existente para esse 'Symbol': {product.Symbol}");
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Update_A_Product()
        {
            // Arrange
            var product = ProductFactory.FakeProduct();
            _productService.Add(product);
            product.Symbol = "ABCDE";

            // Act
            _productService.Update(product);

            // Assert
            var result = _productService.Get(product.Id);
            result.Symbol.Should().Be("ABCDE");
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Update_A_Product_That_Doesnt_Exist()
        {
            // Arrange
            var product = ProductFactory.FakeProduct();
            _productService.Add(product);
            product.Symbol = "ABCDE";
            product.Id = 100;

            // Act
            var result = () => _productService.Update(product);

            // Assert
            result.Should().ThrowExactly<ArgumentException>()
                           .WithMessage($"Produto não existente para o ID: {product.Id}");
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Delete_A_Product()
        {
            // Arrange
            var product = ProductFactory.FakeProduct();
            _productService.Add(product);

            // Act
            _productService.Delete(product.Id);

            // Assert
            var result = _productService.AnyProductForId(product.Id);
            result.Should().BeFalse();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Delete_A_Product_That_Doesnt_Exist()
        {
            // Arrange
            var product = ProductFactory.FakeProduct();
            _productService.Add(product);

            // Act
            var result = () => _productService.Delete(0);

            // Assert
            result.Should().ThrowExactly<ArgumentException>()
                           .WithMessage($"Produto não existente para o ID: {0}");
        }
    }
}
