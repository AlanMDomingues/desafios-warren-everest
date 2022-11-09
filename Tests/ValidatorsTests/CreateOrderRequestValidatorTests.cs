using API.Tests.Fixtures;
using Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace API.Tests.ValidatorsTests
{
    public class CreateOrderRequestValidatorTests
    {
        private readonly CreateOrderRequestValidator _createOrderRequestValidator = new();

        #region Quotes Property Tests

        [Fact]
        public void Should_Pass_When_Trying_To_Add_A_CreateOrderRequest_Quotes()
        {
            // Arrange
            var createOrderRequest = OrderFactory.FakeCreateOrderRequest();

            // Act
            var actionTest = _createOrderRequestValidator.TestValidate(createOrderRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(0);
            actionTest.ShouldNotHaveValidationErrorFor(x => x.Quotes);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Add_A_Null_CreateOrderRequest_Quotes()
        {
            // Arrange
            var createOrderRequest = OrderFactory.FakeCreateOrderRequest();
            createOrderRequest.Quotes = 0;

            // Act
            var actionTest = _createOrderRequestValidator.TestValidate(createOrderRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(2);
            actionTest.Errors[0].ErrorMessage.Should().Be("'Quotes' deve ser informado.");
            actionTest.Errors[1].ErrorMessage.Should().Be("'Quotes' deve ser superior a '0'.");
            actionTest.ShouldHaveValidationErrorFor(x => x.Quotes);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Add_A_CreateOrderRequest_Quotes_Less_Than_Zero()
        {
            // Arrange
            var createOrderRequest = OrderFactory.FakeCreateOrderRequest();
            createOrderRequest.Quotes = -1;

            // Act
            var actionTest = _createOrderRequestValidator.TestValidate(createOrderRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.Quotes).WithErrorMessage("'Quotes' deve ser superior a '0'.");
        }

        #endregion Quotes Property Tests

        #region PortfolioId Property Tests

        [Fact]
        public void Should_Pass_When_Trying_To_Add_A_CreateOrderRequest_PortfolioId()
        {
            // Arrange
            var createOrderRequest = OrderFactory.FakeCreateOrderRequest();

            // Act
            var actionTest = _createOrderRequestValidator.TestValidate(createOrderRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(0);
            actionTest.ShouldNotHaveValidationErrorFor(x => x.PortfolioId);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Add_A_Null_CreateOrderRequest_PortfolioId()
        {
            // Arrange
            var createOrderRequest = OrderFactory.FakeCreateOrderRequest();
            createOrderRequest.PortfolioId = 0;

            // Act
            var actionTest = _createOrderRequestValidator.TestValidate(createOrderRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(2);
            actionTest.Errors[0].ErrorMessage.Should().Be("'Portfolio Id' deve ser informado.");
            actionTest.Errors[1].ErrorMessage.Should().Be("'Portfolio Id' deve ser superior a '0'.");
            actionTest.ShouldHaveValidationErrorFor(x => x.PortfolioId);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Add_A_CreateOrderRequest_PortfolioId_Less_Than_Zero()
        {
            // Arrange
            var createOrderRequest = OrderFactory.FakeCreateOrderRequest();
            createOrderRequest.PortfolioId = -1;

            // Act
            var actionTest = _createOrderRequestValidator.TestValidate(createOrderRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.PortfolioId).WithErrorMessage("'Portfolio Id' deve ser superior a '0'.");
        }

        #endregion PortfolioId Property Tests

        #region ProductId Property Tests

        [Fact]
        public void Should_Pass_When_Trying_To_Add_A_CreateOrderRequest_ProductId()
        {
            // Arrange
            var createOrderRequest = OrderFactory.FakeCreateOrderRequest();

            // Act
            var actionTest = _createOrderRequestValidator.TestValidate(createOrderRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(0);
            actionTest.ShouldNotHaveValidationErrorFor(x => x.ProductId);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Add_A_Null_CreateOrderRequest_ProductId()
        {
            // Arrange
            var createOrderRequest = OrderFactory.FakeCreateOrderRequest();
            createOrderRequest.ProductId = 0;

            // Act
            var actionTest = _createOrderRequestValidator.TestValidate(createOrderRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(2);
            actionTest.Errors[0].ErrorMessage.Should().Be("'Product Id' deve ser informado.");
            actionTest.Errors[1].ErrorMessage.Should().Be("'Product Id' deve ser superior a '0'.");
            actionTest.ShouldHaveValidationErrorFor(x => x.ProductId);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Add_A_CreateOrderRequest_ProductId_Less_Than_Zero()
        {
            // Arrange
            var createOrderRequest = OrderFactory.FakeCreateOrderRequest();
            createOrderRequest.ProductId = -1;

            // Act
            var actionTest = _createOrderRequestValidator.TestValidate(createOrderRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.ProductId).WithErrorMessage("'Product Id' deve ser superior a '0'.");
        }

        #endregion ProductId Property Tests

        #region CustomerBankInfoId Property Tests

        [Fact]
        public void Should_Pass_When_Trying_To_Add_A_CreateOrderRequest_CustomerBankInfoId()
        {
            // Arrange
            var createOrderRequest = OrderFactory.FakeCreateOrderRequest();

            // Act
            var actionTest = _createOrderRequestValidator.TestValidate(createOrderRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(0);
            actionTest.ShouldNotHaveValidationErrorFor(x => x.CustomerBankInfoId);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Add_A_Null_CreateOrderRequest_CustomerBankInfoId()
        {
            // Arrange
            var createOrderRequest = OrderFactory.FakeCreateOrderRequest();
            createOrderRequest.CustomerBankInfoId = 0;

            // Act
            var actionTest = _createOrderRequestValidator.TestValidate(createOrderRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(2);
            actionTest.Errors[0].ErrorMessage.Should().Be("'Customer Bank Info Id' deve ser informado.");
            actionTest.Errors[1].ErrorMessage.Should().Be("'Customer Bank Info Id' deve ser superior a '0'.");
            actionTest.ShouldHaveValidationErrorFor(x => x.CustomerBankInfoId);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Add_A_CreateOrderRequest_CustomerBankInfoId_Less_Than_Zero()
        {
            // Arrange
            var createOrderRequest = OrderFactory.FakeCreateOrderRequest();
            createOrderRequest.CustomerBankInfoId = -1;

            // Act
            var actionTest = _createOrderRequestValidator.TestValidate(createOrderRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.CustomerBankInfoId).WithErrorMessage("'Customer Bank Info Id' deve ser superior a '0'.");
        }

        #endregion CustomerBankInfoId Property Tests
    }
}
