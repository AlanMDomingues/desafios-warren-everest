using Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Tests.Factories;
using Xunit;

namespace Tests.ValidationTests
{
    public class CreateOrderRequestValidatorTest
    {
        private readonly CreateOrderRequestValidator _createOrderRequestValidator = new();

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

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Add_A_CreateOrderRequest_Quotes_Greater_Than_One_Hundred()
        {
            // Arrange
            var createOrderRequest = OrderFactory.FakeCreateOrderRequest();
            createOrderRequest.Quotes = 101;

            // Act
            var actionTest = _createOrderRequestValidator.TestValidate(createOrderRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.Quotes).WithErrorMessage("'Quotes' deve ser inferior ou igual a '100'.");
        }
    }
}
