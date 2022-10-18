using Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Tests.Factories;
using Xunit;

namespace Tests.ValidationTests
{
    public class CreatePortfolioRequestValidatorTest
    {
        private readonly CreatePortfolioRequestValidator _createPortfolioRequestValidator = new();

        [Fact]
        public void Should_Pass_When_Trying_To_Add_Portfolio_Name()
        {
            // Arrange
            var createPortfolioRequest = PortfolioFactory.FakeCreatePortfolioRequest();

            // Act
            var actionTest = _createPortfolioRequestValidator.TestValidate(createPortfolioRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(0);
            actionTest.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Add_Null_Portfolio_Name()
        {
            // Arrange
            var createPortfolioRequest = PortfolioFactory.FakeCreatePortfolioRequest();
            createPortfolioRequest.Name = null;

            // Act
            var actionTest = _createPortfolioRequestValidator.TestValidate(createPortfolioRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("'Name' deve ser informado.");
        }

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Add_Empty_Portfolio_Name()
        {
            // Arrange
            var createPortfolioRequest = PortfolioFactory.FakeCreatePortfolioRequest();
            createPortfolioRequest.Name = "";

            // Act
            var actionTest = _createPortfolioRequestValidator.TestValidate(createPortfolioRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("'Name' deve ser informado.");
        }

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Add_Portfolio_Name_Higher_Than_Thirty_Characteres()
        {
            // Arrange
            var createPortfolioRequest = PortfolioFactory.FakeCreatePortfolioRequest();
            createPortfolioRequest.Name = "";
            while (createPortfolioRequest.Name.Length <= 30)
            {
                createPortfolioRequest.Name += "Teste";
            }

            // Act
            var actionTest = _createPortfolioRequestValidator.TestValidate(createPortfolioRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("'Name' deve ser menor ou igual a 30 caracteres. Você digitou 35 caracteres.");
        }

        [Fact]
        public void Should_Fail_And_Return_Error_Message_When_Trying_To_Add_Portfolio_Description_Higher_Than_One_Hundred_Characteres()
        {
            // Arrange
            var createPortfolioRequest = PortfolioFactory.FakeCreatePortfolioRequest();
            createPortfolioRequest.Description = "";
            while (createPortfolioRequest.Description.Length <= 100)
            {
                createPortfolioRequest.Description += "Teste";
            }

            // Act
            var actionTest = _createPortfolioRequestValidator.TestValidate(createPortfolioRequest);

            // Assert
            actionTest.Errors.Should().HaveCount(1);
            actionTest.ShouldHaveValidationErrorFor(x => x.Description).WithErrorMessage("'Description' deve ser menor ou igual a 100 caracteres. Você digitou 105 caracteres.");
        }
    }
}
