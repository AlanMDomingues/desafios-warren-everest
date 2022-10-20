using Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Tests.Factories;
using Xunit;

namespace Tests.ValidationTests
{
    public class CreateProductRequestValidatorTests
    {
        private readonly CreateProductRequestValidator _createProductRequestValidator = new();

        #region Symbol Property Tests

        [Fact]
        public void Should_Pass_When_Trying_To_Add_Symbol()
        {
            // Arrange
            var createProductRequest = ProductFactory.FakeCreateProductRequest();

            // Act
            var actionTest = _createProductRequestValidator.TestValidate(createProductRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(0);
            actionTest.ShouldNotHaveValidationErrorFor(x => x.Symbol);
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Add_Null_Symbol()
        {
            // Arrange
            var createProductRequest = ProductFactory.FakeCreateProductRequest();
            createProductRequest.Symbol = null;

            // Act
            var actionTest = _createProductRequestValidator.TestValidate(createProductRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.Symbol).WithErrorMessage("'Symbol' deve ser informado.");
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Add_Empty_Symbol()
        {
            // Arrange
            var createProductRequest = ProductFactory.FakeCreateProductRequest();
            createProductRequest.Symbol = "";

            // Act
            var actionTest = _createProductRequestValidator.TestValidate(createProductRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(2);
            actionTest.Errors[0].ErrorMessage.Should().Be("'Symbol' deve ser informado.");
            actionTest.Errors[1].ErrorMessage.Should().Be("'Symbol' deve ser maior ou igual a 2 caracteres. Você digitou 0 caracteres.");
            actionTest.ShouldHaveValidationErrorFor(x => x.Symbol);
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Add_Symbol_Less_Than_Two_Characters()
        {
            // Arrange
            var createProductRequest = ProductFactory.FakeCreateProductRequest();
            createProductRequest.Symbol = "B";

            // Act
            var actionTest = _createProductRequestValidator.TestValidate(createProductRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.Symbol).WithErrorMessage("'Symbol' deve ser maior ou igual a 2 caracteres. Você digitou 1 caracteres.");
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Add_Symbol_Higher_Than_Twenty_Characters()
        {
            // Arrange
            var createProductRequest = ProductFactory.FakeCreateProductRequest();
            createProductRequest.Symbol = "";
            while (createProductRequest.Symbol.Length <= 20)
            {
                createProductRequest.Symbol += "B";
            }

            // Act
            var actionTest = _createProductRequestValidator.TestValidate(createProductRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.Symbol).WithErrorMessage("'Symbol' deve ser menor ou igual a 20 caracteres. Você digitou 21 caracteres.");
        }

        #endregion Symbol Property Tests

        #region UnitPrice Property Tests

        [Fact]
        public void Should_Pass_When_Trying_To_Add_UnitPrice()
        {
            // Arrange
            var createProductRequest = ProductFactory.FakeCreateProductRequest();

            // Act
            var actionTest = _createProductRequestValidator.TestValidate(createProductRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(0);
            actionTest.ShouldNotHaveValidationErrorFor(x => x.UnitPrice);
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Add_Null_Or_Empty_UnitPrice()
        {
            // Arrange
            var createProductRequest = ProductFactory.FakeCreateProductRequest();
            createProductRequest.UnitPrice = 0;

            // Act
            var actionTest = _createProductRequestValidator.TestValidate(createProductRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(2);
            actionTest.Errors[0].ErrorMessage.Should().Be("'Unit Price' deve ser informado.");
            actionTest.Errors[1].ErrorMessage.Should().Be("'Unit Price' deve ser superior a '0'.");
            actionTest.ShouldHaveValidationErrorFor(x => x.UnitPrice);
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Add_UnitPrice_Less_Than_Zero()
        {
            // Arrange
            var createProductRequest = ProductFactory.FakeCreateProductRequest();
            createProductRequest.UnitPrice = -1;

            // Act
            var actionTest = _createProductRequestValidator.TestValidate(createProductRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.UnitPrice).WithErrorMessage("'Unit Price' deve ser superior a '0'.");
        }

        #endregion UnitPrice Property Tests
    }
}
