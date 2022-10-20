using Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Tests.Factories;
using Xunit;

namespace Tests.ValidationTests
{
    public class UpdatePortfolioRequestValidatorTests
    {
        private readonly UpdatePortfolioRequestValidator _updatePortfolioRequestValidator = new();

        #region Name Property Tests

        [Fact]
        public void Should_Pass_When_Trying_To_Update_Portfolio_Name()
        {
            // Arrange
            var updatePortfolioRequest = PortfolioFactory.FakeUpdatePortfolioRequest();

            // Act
            var actionTest = _updatePortfolioRequestValidator.TestValidate(updatePortfolioRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(0);
            actionTest.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Update_Null_Portfolio_Name()
        {
            // Arrange
            var updatePortfolioRequest = PortfolioFactory.FakeUpdatePortfolioRequest();
            updatePortfolioRequest.Name = null;

            // Act
            var actionTest = _updatePortfolioRequestValidator.TestValidate(updatePortfolioRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("'Name' deve ser informado.");
        }

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Update_Empty_Portfolio_Name()
        {
            // Arrange
            var updatePortfolioRequest = PortfolioFactory.FakeUpdatePortfolioRequest();
            updatePortfolioRequest.Name = "";

            // Act
            var actionTest = _updatePortfolioRequestValidator.TestValidate(updatePortfolioRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(2);
            actionTest.Errors[0].ErrorMessage.Should().Be("'Name' deve ser informado.");
            actionTest.Errors[1].ErrorMessage.Should().Be("'Name' deve ser maior ou igual a 2 caracteres. Você digitou 0 caracteres.");
            actionTest.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Update_Portfolio_Name_Less_Than_Two_Characteres()
        {
            // Arrange
            var updatePortfolioRequest = PortfolioFactory.FakeUpdatePortfolioRequest();
            updatePortfolioRequest.Name = "A";

            // Act
            var actionTest = _updatePortfolioRequestValidator.TestValidate(updatePortfolioRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("'Name' deve ser maior ou igual a 2 caracteres. Você digitou 1 caracteres.");
        }

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Update_Portfolio_Name_Higher_Than_Thirty_Characteres()
        {
            // Arrange
            var updatePortfolioRequest = PortfolioFactory.FakeUpdatePortfolioRequest();
            updatePortfolioRequest.Name = "";
            while (updatePortfolioRequest.Name.Length <= 30)
            {
                updatePortfolioRequest.Name += "Teste";
            }

            // Act
            var actionTest = _updatePortfolioRequestValidator.TestValidate(updatePortfolioRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("'Name' deve ser menor ou igual a 30 caracteres. Você digitou 35 caracteres.");
        }

        #endregion Name Property Tests

        #region Description Property Tests

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Update_Portfolio_Description_Higher_Than_One_Hundred_Characteres()
        {
            // Arrange
            var updatePortfolioRequest = PortfolioFactory.FakeUpdatePortfolioRequest();
            updatePortfolioRequest.Description = "";
            while (updatePortfolioRequest.Description.Length <= 100)
            {
                updatePortfolioRequest.Description += "Teste";
            }

            // Act
            var actionTest = _updatePortfolioRequestValidator.TestValidate(updatePortfolioRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.Description).WithErrorMessage("'Description' deve ser menor ou igual a 100 caracteres. Você digitou 105 caracteres.");
        }

        #endregion Description Property Tests

        #region CustomerId Property Tests

        [Fact]
        public void Should_Pass_When_Trying_To_Update_A_CreateOrderRequest_CustomerBankInfoId()
        {
            // Arrange
            var updatePortfolioRequest = PortfolioFactory.FakeUpdatePortfolioRequest();

            // Act
            var actionTest = _updatePortfolioRequestValidator.TestValidate(updatePortfolioRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(0);
            actionTest.ShouldNotHaveValidationErrorFor(x => x.CustomerId);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Update_A_Null_CreateOrderRequest_CustomerBankInfoId()
        {
            // Arrange
            var updatePortfolioRequest = PortfolioFactory.FakeUpdatePortfolioRequest();
            updatePortfolioRequest.CustomerId = 0;

            // Act
            var actionTest = _updatePortfolioRequestValidator.TestValidate(updatePortfolioRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(2);
            actionTest.Errors[0].ErrorMessage.Should().Be("'Customer Id' deve ser informado.");
            actionTest.Errors[1].ErrorMessage.Should().Be("'Customer Id' deve ser superior a '0'.");
            actionTest.ShouldHaveValidationErrorFor(x => x.CustomerId);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Update_A_CreateOrderRequest_CustomerBankInfoId_Less_Than_Zero()
        {
            // Arrange
            var updatePortfolioRequest = PortfolioFactory.FakeUpdatePortfolioRequest();
            updatePortfolioRequest.CustomerId = -1;

            // Act
            var actionTest = _updatePortfolioRequestValidator.TestValidate(updatePortfolioRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.CustomerId).WithErrorMessage("'Customer Id' deve ser superior a '0'.");
        }

        #endregion CustomerId Property Tests
    }
}
