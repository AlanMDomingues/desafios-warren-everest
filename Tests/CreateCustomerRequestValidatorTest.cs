using Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Tests.Factories;
using Xunit;

namespace Tests;

public class CreateCustomerRequestValidatorTest
{
    private readonly CreateCustomerRequestValidator _createCustomerRequestValidator;

    public CreateCustomerRequestValidatorTest() => _createCustomerRequestValidator = new();

    [Fact]
    public void Should_Fail_And_Return_ErrorMessage_When_Trying_To_Add_A_Customer_FullName_Null()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.FullName = null;

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);
        // Assert
        actionTestValidate.Errors.Should().HaveCount(2);
        actionTestValidate.Errors[0].ErrorMessage.Should().Be("'Full Name' deve ser informado.");
        actionTestValidate.Errors[1].ErrorMessage.Should().Be("'Full Name' não atende a condição definida.");
        actionTestValidate.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Fail_And_Return_StatusCode400_With_Message_When_Trying_To_Add_A_Customer_FullName_Empty()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.FullName = "";

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(3);
        actionTestValidate.Errors[0].ErrorMessage.Should().Be("'Full Name' deve ser informado.");
        actionTestValidate.Errors[1].ErrorMessage.Should().Be("'Full Name' não atende a condição definida.");
        actionTestValidate.Errors[2].ErrorMessage.Should().Be("'Full Name' deve ser maior ou igual a 2 caracteres. Você digitou 0 caracteres.");
        actionTestValidate.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Fail_And_Return_StatusCode400_With_Message_When_Trying_To_Add_A_Customer_FullName_Without_First_Letter_In_UpperCase()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.FullName = "carlos Eduardo";

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(1);
        actionTestValidate.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Fail_And_Return_StatusCode400_With_Message_When_Trying_To_Add_A_Customer_FullName_With_Only_FirstName()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.FullName = "Carlos";

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(1);
        actionTestValidate.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Fail_And_Return_StatusCode400_With_Message_When_Trying_To_Add_A_Customer_FullName_With_Numbers()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.FullName = "Carlos Eduardo 2022";

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(1);
        actionTestValidate.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Fail_And_Return_StatusCode400_With_Message_When_Trying_To_Add_A_Customer_FullName_With_Two_Spaces_Or_More_Between_Words()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.FullName = "Carlos  Eduardo";

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(1);
        actionTestValidate.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Theory]
    [InlineData(" Carlos Eduardo")]
    [InlineData("Carlos Eduardo ")]
    public void Should_Fail_And_Return_StatusCode400_With_Message_When_Trying_To_Add_A_Customer_FullName_With_Space_First_Or_After_Words(string fullName)
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.FullName = fullName;

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(1);
        actionTestValidate.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Fail_And_Return_StatusCode400_With_Message_When_Trying_To_Add_A_Customer_FullName_Have_Less_Than_Two_Words()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.FullName = "Carlos";

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(1);
        actionTestValidate.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Fail_And_Return_StatusCode400_With_Message_When_Trying_To_Add_A_Customer_FullName_Have_Greater_Than_Thirty_Words()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.FullName = "";
        for (int i = 0; i < 30; i++)
        {
            createCustomerRequest.FullName += "Carlos ";
        }
        createCustomerRequest.FullName += "Augusto";

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(1);
        actionTestValidate.ShouldHaveValidationErrorFor(x => x.FullName).WithErrorMessage("'Full Name' não atende a condição definida.");
    }

    [Fact]
    public void Should_Fail_And_Return_StatusCode400_With_Message_When_Trying_To_Add_A_Customer_FullName_Have_Less_Than_Two_Letters()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.FullName = "C";

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(2);
        actionTestValidate.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Fail_And_Return_StatusCode400_With_Message_When_Trying_To_Add_A_Customer_FullName_Have_Greater_Than_Three_Hundred_Characters()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.FullName = "";

        while (createCustomerRequest.FullName.Length < 300)
        {
            createCustomerRequest.FullName += "Ab";
        }
        createCustomerRequest.FullName += " Batata Doce";

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(1);
        actionTestValidate.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Theory]
    [InlineData("Carlos Eduardo")]
    [InlineData("João Pedro Da Silva")]
    [InlineData("Álan Martin Domingues")]
    [InlineData("Carlos Aurelio Casanova de la Torre Ugarte")]
    public void Should_Pass__When_Trying_To_Add_A_Customer_FullName(string fullName)
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.FullName = fullName;

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(0);
        actionTestValidate.ShouldNotHaveValidationErrorFor(x => x.FullName);
    }

    [Theory]
    [InlineData("alanqualquercoisa@gmail.com")]
    [InlineData("Testeteste@gmail.com")]
    [InlineData("Will_Johnston79@gmail.com")]
    [InlineData("Eleazar_Purdy15@hotmail.com")]
    public void Should_Pass_When_Trying_To_Add_A_Customer_Email(string email)
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.Email = email;

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(1);
        actionTestValidate.ShouldNotHaveValidationErrorFor(x => x.Email);
        actionTestValidate.ShouldHaveValidationErrorFor(x => x).WithErrorMessage("'Email' and 'EmailConfirmation' should be equals");
    }

    [Fact]
    public void Should_Fail_When_Trying_To_Add_A_Customer_Email_Less_Than_Ten_Characters()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.Email = "a@a.com";

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(2);
        actionTestValidate.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("'Email' deve ser maior ou igual a 10 caracteres. Você digitou 7 caracteres.");
        actionTestValidate.ShouldHaveValidationErrorFor(x => x).WithErrorMessage("'Email' and 'EmailConfirmation' should be equals");
    }

    [Fact]
    public void Should_Fail_When_Trying_To_Add_A_Customer_Email_Greater_Than_TwoHundredFiftySix_Characters_Before_At()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();

        createCustomerRequest.Email = "";
        while (createCustomerRequest.Email.Length < 256)
        {
            createCustomerRequest.Email += "Nijinskyoff_VonAmbiguous_";
        }
        createCustomerRequest.Email += "@gmail.com";

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(2);
        actionTestValidate.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("'Email' deve ser menor ou igual a 256 caracteres. Você digitou 285 caracteres.");
        actionTestValidate.ShouldHaveValidationErrorFor(x => x).WithErrorMessage("'Email' and 'EmailConfirmation' should be equals");
    }

    [Fact]
    public void Should_Fail_When_Trying_To_Add_A_Customer_Email_Greater_Than_TwoHundredFiftySix_Characters_After_At()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();

        createCustomerRequest.Email = "Nijinskyoff_VonAmbiguous@";
        while (createCustomerRequest.Email.Length < 256)
        {
            createCustomerRequest.Email += "gmail";
        }
        createCustomerRequest.Email += ".com";

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(2);
        actionTestValidate.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("'Email' deve ser menor ou igual a 256 caracteres. Você digitou 264 caracteres.");
        actionTestValidate.ShouldHaveValidationErrorFor(x => x).WithErrorMessage("'Email' and 'EmailConfirmation' should be equals");
    }

    [Fact]
    public void Should_Fail_When_Trying_To_Add_A_Null_Customer_Email()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.Email = null;

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(2);
        actionTestValidate.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("'Email' deve ser informado.");
        actionTestValidate.ShouldHaveValidationErrorFor(x => x).WithErrorMessage("'Email' and 'EmailConfirmation' should be equals");
    }

    [Fact]
    public void Should_Fail_When_Trying_To_Add_An_Empty_Customer_Email()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.Email = "";

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(4);
        actionTestValidate.Errors[0].ErrorMessage.Should().Be("'Email' deve ser informado.");
        actionTestValidate.Errors[1].ErrorMessage.Should().Be("'Email' é um endereço de email inválido.");
        actionTestValidate.Errors[2].ErrorMessage.Should().Be("'Email' deve ser maior ou igual a 10 caracteres. Você digitou 0 caracteres.");
        actionTestValidate.Errors[3].ErrorMessage.Should().Be("'Email' and 'EmailConfirmation' should be equals");
        actionTestValidate.ShouldHaveValidationErrorFor(x => x.Email);
        actionTestValidate.ShouldHaveValidationErrorFor(x => x);
    }

    [Fact]
    public void Should_Fail_When_Trying_To_Add_A_Customer_EmailConfirmation_And_Email_Doesnt_Equals()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.EmailConfirmation = "teste1teste2@gmail.com";

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(1);
        actionTestValidate.ShouldHaveValidationErrorFor(x => x).WithErrorMessage("'Email' and 'EmailConfirmation' should be equals");
    }

    [Fact]
    public void Should_Pass_When_Trying_To_Add_A_Customer_EmailConfirmation_And_Email_Equals()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.EmailConfirmation = createCustomerRequest.Email;

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(0);
        actionTestValidate.ShouldNotHaveValidationErrorFor(x => x);
    }

    [Fact]
    public void Should_Pass_When_Trying_To_Add_A_Customer_Whatsapp_And_EmailSms()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(0);
        actionTestValidate.ShouldNotHaveValidationErrorFor(x => x);
    }

    [Fact]
    public void Should_Fail_When_Trying_To_Add_A_Customer_Whatsapp_And_EmailSms_Are_False()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.Whatsapp = false;
        createCustomerRequest.EmailSms = false;

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(1);
        actionTestValidate.Errors[0].ErrorMessage.Should().Be("At least one or both 'EmailSms' or/and 'Whatsapp' must be true");
        actionTestValidate.ShouldHaveValidationErrorFor(x => x);
    }

    [Fact]
    public void Should_Pass_When_Trying_To_Add_A_Customer_Cpf()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.Cpf = "72286873020";

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(0);
        actionTestValidate.ShouldNotHaveValidationErrorFor(x => x.Cpf);
    }

    [Fact]
    public void Should_Fail_When_Trying_To_Add_A_Invalid_Customer_Cpf()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        createCustomerRequest.Cpf = "72286873021";

        // Act
        var actionTestValidate = _createCustomerRequestValidator.TestValidate(createCustomerRequest);

        // Assert
        actionTestValidate.Errors.Should().HaveCount(1);
        actionTestValidate.ShouldHaveValidationErrorFor(x => x.Cpf).WithErrorMessage("'Cpf' não atende a condição definida.");
    }
}
