using API.Tests.Fixtures;
using Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace API.Tests.ValidatorsTests
{
    public class UpdateProductRequestValidatorTests
    {
        private readonly UpdateProductRequestValidator _updateProductRequestValidator = new();

        #region Symbol Property Tests

        [Fact]
        public void Should_Pass_When_Trying_To_Update_Symbol()
        {
            // Arrange
            var updateProductRequest = ProductFactory.FakeUpdateProductRequest();

            // Act
            var actionTest = _updateProductRequestValidator.TestValidate(updateProductRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(0);
            actionTest.ShouldNotHaveValidationErrorFor(x => x.Symbol);
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Update_Null_Symbol()
        {
            // Arrange
            var updateProductRequest = ProductFactory.FakeUpdateProductRequest();
            updateProductRequest.Symbol = null;

            // Act
            var actionTest = _updateProductRequestValidator.TestValidate(updateProductRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.Symbol).WithErrorMessage("'Symbol' deve ser informado.");
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Update_Empty_Symbol()
        {
            // Arrange
            var updateProductRequest = ProductFactory.FakeUpdateProductRequest();
            updateProductRequest.Symbol = "";

            // Act
            var actionTest = _updateProductRequestValidator.TestValidate(updateProductRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(2);
            actionTest.Errors[0].ErrorMessage.Should().Be("'Symbol' deve ser informado.");
            actionTest.Errors[1].ErrorMessage.Should().Be("'Symbol' deve ser maior ou igual a 2 caracteres. Você digitou 0 caracteres.");
            actionTest.ShouldHaveValidationErrorFor(x => x.Symbol);
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Update_Symbol_Less_Than_Two_Characters()
        {
            // Arrange
            var updateProductRequest = ProductFactory.FakeUpdateProductRequest();
            updateProductRequest.Symbol = "B";

            // Act
            var actionTest = _updateProductRequestValidator.TestValidate(updateProductRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.Symbol).WithErrorMessage("'Symbol' deve ser maior ou igual a 2 caracteres. Você digitou 1 caracteres.");
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Update_Symbol_Higher_Than_Twenty_Characters()
        {
            // Arrange
            var updateProductRequest = ProductFactory.FakeUpdateProductRequest();
            updateProductRequest.Symbol = "";
            while (updateProductRequest.Symbol.Length <= 20)
            {
                updateProductRequest.Symbol += "B";
            }

            // Act
            var actionTest = _updateProductRequestValidator.TestValidate(updateProductRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.Symbol).WithErrorMessage("'Symbol' deve ser menor ou igual a 20 caracteres. Você digitou 21 caracteres.");
        }

        #endregion Symbol Property Tests

        #region UnitPrice Property Tests

        [Fact]
        public void Should_Pass_When_Trying_To_Update_UnitPrice()
        {
            // Arrange
            var updateProductRequest = ProductFactory.FakeUpdateProductRequest();

            // Act
            var actionTest = _updateProductRequestValidator.TestValidate(updateProductRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(0);
            actionTest.ShouldNotHaveValidationErrorFor(x => x.UnitPrice);
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Update_Null_Or_Empty_UnitPrice()
        {
            // Arrange
            var updateProductRequest = ProductFactory.FakeUpdateProductRequest();
            updateProductRequest.UnitPrice = 0;

            // Act
            var actionTest = _updateProductRequestValidator.TestValidate(updateProductRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(2);
            actionTest.Errors[0].ErrorMessage.Should().Be("'Unit Price' deve ser informado.");
            actionTest.Errors[1].ErrorMessage.Should().Be("'Unit Price' deve ser superior a '0'.");
            actionTest.ShouldHaveValidationErrorFor(x => x.UnitPrice);
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Update_UnitPrice_Less_Than_Zero()
        {
            // Arrange
            var updateProductRequest = ProductFactory.FakeUpdateProductRequest();
            updateProductRequest.UnitPrice = -1;

            // Act
            var actionTest = _updateProductRequestValidator.TestValidate(updateProductRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.UnitPrice).WithErrorMessage("'Unit Price' deve ser superior a '0'.");
        }

        #endregion UnitPrice Property Tests
    }
}
