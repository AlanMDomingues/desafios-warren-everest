using API.Tests.Fixtures;
using Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace API.Tests.ValidatorsTests
{
    public class UpdateCustomerRequestValidatorTests
    {
        private readonly UpdateCustomerRequestValidator _updateCustomerRequestValidator;

        public UpdateCustomerRequestValidatorTests() => _updateCustomerRequestValidator = new();

        #region FullName Property Tests

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Customer_FullName_Null()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.FullName = null;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);
            // Assert
            actionTestValidate.Errors.Should().HaveCount(2);
            actionTestValidate.Errors[0].ErrorMessage.Should().Be("'Full Name' deve ser informado.");
            actionTestValidate.Errors[1].ErrorMessage.Should().Be("'Full Name' não atende a condição definida.");
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.FullName);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_An_Empty_Customer_FullName()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.FullName = "";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(3);
            actionTestValidate.Errors[0].ErrorMessage.Should().Be("'Full Name' deve ser informado.");
            actionTestValidate.Errors[1].ErrorMessage.Should().Be("'Full Name' deve ser maior ou igual a 2 caracteres. Você digitou 0 caracteres.");
            actionTestValidate.Errors[2].ErrorMessage.Should().Be("'Full Name' não atende a condição definida.");
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.FullName);
        }

        [Theory]
        [InlineData("carlos Eduardo")]
        [InlineData("Carlos")]
        [InlineData("Carlos Eduardo 2022")]
        [InlineData("Carlos  Eduardo")]
        [InlineData(" Carlos Eduardo")]
        [InlineData("Carlos Eduardo ")]
        [InlineData("João")]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_An_Invalid_Customer_FullName(string fullName)
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.FullName = fullName;

            if (fullName.Equals("João"))
            {
                for (int i = 0; i < 30; i++)
                {
                    updateCustomerRequest.FullName += " João";
                }
            }

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.FullName).WithErrorMessage("'Full Name' não atende a condição definida.");
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Customer_FullName_Have_Less_Than_Two_Letters()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.FullName = "C";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(2);
            actionTestValidate.Errors[0].ErrorMessage.Should().Be("'Full Name' deve ser maior ou igual a 2 caracteres. Você digitou 1 caracteres.");
            actionTestValidate.Errors[1].ErrorMessage.Should().Be("'Full Name' não atende a condição definida.");
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.FullName);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Customer_FullName_Have_Greater_Than_Three_Hundred_Characters()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.FullName = "";

            while (updateCustomerRequest.FullName.Length < 300)
            {
                updateCustomerRequest.FullName += "Ab";
            }
            updateCustomerRequest.FullName += " Batata Doce";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.FullName).WithErrorMessage("'Full Name' deve ser menor ou igual a 300 caracteres. Você digitou 312 caracteres.");
        }

        [Theory]
        [InlineData("Carlos Eduardo")]
        [InlineData("João Pedro Da Silva")]
        [InlineData("Álan Martin Domingues")]
        [InlineData("Carlos Aurelio Casanova de la Torre Ugarte")]
        public void Should_Pass_When_Trying_To_Update_A_Customer_FullName(string fullName)
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.FullName = fullName;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(0);
            actionTestValidate.ShouldNotHaveValidationErrorFor(x => x.FullName);
        }

        #endregion FullName Property Tests

        #region Email Property Tests

        [Theory]
        [InlineData("alanqualquercoisa@gmail.com")]
        [InlineData("Testeteste@gmail.com")]
        [InlineData("Will_Johnston79@gmail.com")]
        [InlineData("Eleazar_Purdy15@hotmail.com")]
        public void Should_Pass_When_Trying_To_Update_A_Customer_Email(string email)
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Email = email;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(0);
            actionTestValidate.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Update_A_Customer_Email_Less_Than_Ten_Characters()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Email = "a@a.com";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("'Email' deve ser maior ou igual a 10 caracteres. Você digitou 7 caracteres.");
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Update_A_Customer_Email_Greater_Than_TwoHundredFiftySix_Characters_Before_At()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();

            updateCustomerRequest.Email = "";
            while (updateCustomerRequest.Email.Length < 256)
            {
                updateCustomerRequest.Email += "Nijinskyoff_VonAmbiguous_";
            }
            updateCustomerRequest.Email += "@gmail.com";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("'Email' deve ser menor ou igual a 256 caracteres. Você digitou 285 caracteres.");
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Update_A_Customer_Email_Greater_Than_TwoHundredFiftySix_Characters_After_At()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();

            updateCustomerRequest.Email = "Nijinskyoff_VonAmbiguous@";
            while (updateCustomerRequest.Email.Length < 256)
            {
                updateCustomerRequest.Email += "gmail";
            }
            updateCustomerRequest.Email += ".com";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("'Email' deve ser menor ou igual a 256 caracteres. Você digitou 264 caracteres.");
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Update_A_Null_Customer_Email()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Email = null;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("'Email' deve ser informado.");
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Update_An_Empty_Customer_Email()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Email = "";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(3);
            actionTestValidate.Errors[0].ErrorMessage.Should().Be("'Email' deve ser informado.");
            actionTestValidate.Errors[1].ErrorMessage.Should().Be("'Email' é um endereço de email inválido.");
            actionTestValidate.Errors[2].ErrorMessage.Should().Be("'Email' deve ser maior ou igual a 10 caracteres. Você digitou 0 caracteres.");
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Email);
        }

        #endregion Email Property Tests

        #region Whatsapp and EmailSms Properties Tests

        [Fact]
        public void Should_Pass_When_Trying_To_Update_A_Customer_Whatsapp_And_EmailSms()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(0);
            actionTestValidate.ShouldNotHaveValidationErrorFor(x => x);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Customer_Whatsapp_And_EmailSms_Are_False()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Whatsapp = false;
            updateCustomerRequest.EmailSms = false;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x).WithErrorMessage("At least one or both 'EmailSms' or/and 'Whatsapp' must be true");
        }

        #endregion Whatsapp and EmailSms Properties Tests

        #region Cpf Property Tests

        [Fact]
        public void Should_Pass_When_Trying_To_Update_A_Customer_Cpf()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Cpf = "72286873020";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(0);
            actionTestValidate.ShouldNotHaveValidationErrorFor(x => x.Cpf);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Invalid_Customer_Cpf()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Cpf = "72286873021";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Cpf).WithErrorMessage("'Cpf' não atende a condição definida.");
        }

        #endregion Cpf Property Tests

        #region Cellphone Property Tests

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Null_Customer_Cellphone()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Cellphone = null;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(2);
            actionTestValidate.Errors[0].ErrorMessage.Should().Be("'Cellphone' deve ser informado.");
            actionTestValidate.Errors[1].ErrorMessage.Should().Be("'Cellphone' não atende a condição definida.");
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Cellphone);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_An_Empty_Customer_Cellphone()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Cellphone = "";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(3);
            actionTestValidate.Errors[0].ErrorMessage.Should().Be("'Cellphone' deve ser informado.");
            actionTestValidate.Errors[1].ErrorMessage.Should().Be("'Cellphone' não atende a condição definida.");
            actionTestValidate.Errors[2].ErrorMessage.Should().Be("'Cellphone' deve ter no máximo 11 caracteres. Você digitou 0 caracteres.");
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Cellphone);
        }

        [Theory]
        [InlineData("#3999501492")]
        [InlineData("-3999501492")]
        [InlineData("!3999501492")]
        [InlineData(")3999501492")]
        [InlineData(" 3999501492")]
        [InlineData("@3999501492")]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_An_Invalid_Customer_Cellphone_With_Special_Characters(string cellphone)
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Cellphone = cellphone;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Cellphone).WithErrorMessage("'Cellphone' não atende a condição definida.");
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Customer_Cellphone_Greater_Than_Eleven_Characters()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Cellphone = "539995014920";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Cellphone).WithErrorMessage("'Cellphone' deve ter no máximo 11 caracteres. Você digitou 12 caracteres.");
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Update_A_Customer_Cellphone()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(0);
            actionTestValidate.ShouldNotHaveValidationErrorFor(x => x.Cellphone);
        }

        #endregion Cellphone Property Tests

        #region Birthdate Property Tests

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Null_Or_Empty_Customer_Birthdate()
        {
            // Arrange
            var customerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            customerRequest.Birthdate = default;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(customerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Birthdate).WithErrorMessage("'Birthdate' deve ser informado.");
        }

        [Theory]
        [InlineData(17, 0, 0)]
        [InlineData(18, 1, 0)]
        [InlineData(18, 0, 1)]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Customer_Birthdate_Under_Eighteen_Years_Old(int reducedYears, int addedMonths, int addedDays)
        {
            // Arrange
            var yearToday = DateTime.Today.Year;
            var monthToday = DateTime.Today.Month;
            var dayToday = DateTime.Today.Day;

            var daysInMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);

            if (dayToday + addedDays > daysInMonth)
            {
                monthToday += 1;
                dayToday = 1;
                addedDays--;
            }

            if (monthToday + addedMonths == 13)
            {
                yearToday += 1;
                monthToday = 1;
                dayToday = 1;
                addedMonths--;
            }

            try
            {
                var test = new DateTime(yearToday, monthToday + addedMonths, dayToday);
            }
            catch (ArgumentOutOfRangeException)
            {
                if (monthToday == 2)
                {
                    dayToday -= 3;
                }
                else
                {
                    dayToday -= 1;
                }
            }

            var customerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            customerRequest.Birthdate = new DateTime(yearToday - reducedYears, monthToday + addedMonths, dayToday + addedDays);

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(customerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Birthdate).WithErrorMessage("Customer must be at least eighteen years old");
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Update_A_Customer_Birthdate_Higher_Than_Eighteen_Years_Old()
        {
            // Arrange
            var customerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            customerRequest.Birthdate = new DateTime(DateTime.Now.Year - 18, DateTime.Today.Month, DateTime.Today.Day);

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(customerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(0);
            actionTestValidate.ShouldNotHaveValidationErrorFor(x => x.Birthdate);
        }

        #endregion Birthdate Property Tests

        #region Country Property Tests

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Null_Customer_Country()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Country = null;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);
            // Assert
            actionTestValidate.Errors.Should().HaveCount(2);
            actionTestValidate.Errors[0].ErrorMessage.Should().Be("'Country' deve ser informado.");
            actionTestValidate.Errors[1].ErrorMessage.Should().Be("'Country' não atende a condição definida.");
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Country);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_An_Empty_Customer_Country()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Country = "";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(3);
            actionTestValidate.Errors[0].ErrorMessage.Should().Be("'Country' deve ser informado.");
            actionTestValidate.Errors[1].ErrorMessage.Should().Be("'Country' não atende a condição definida.");
            actionTestValidate.Errors[2].ErrorMessage.Should().Be("'Country' deve ser maior ou igual a 2 caracteres. Você digitou 0 caracteres.");
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Country);
        }

        [Theory]
        [InlineData("United  States")]
        [InlineData(" United States")]
        [InlineData("United States ")]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_An_Invalid_Customer_Country(string country)
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Country = country;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Country).WithErrorMessage("'Country' não atende a condição definida.");
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Customer_Country_Less_Than_Two_Letters()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Country = "U";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(2);
            actionTestValidate.Errors[0].ErrorMessage.Should().Be("'Country' não atende a condição definida.");
            actionTestValidate.Errors[1].ErrorMessage.Should().Be("'Country' deve ser maior ou igual a 2 caracteres. Você digitou 1 caracteres.");
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Country);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Customer_Country_Have_Greater_Than_Fifty_Eight_Characters()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Country = "";

            while (updateCustomerRequest.Country.Length < 58)
            {
                updateCustomerRequest.Country += "A";
            }
            updateCustomerRequest.Country += " Batata Doce";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Country).WithErrorMessage("'Country' deve ser menor ou igual a 58 caracteres. Você digitou 70 caracteres.");
        }

        [Theory]
        [InlineData("Guyana")]
        [InlineData("Armenia")]
        [InlineData("Saudi Arabia")]
        [InlineData("British Indian Ocean Territory (Chagos Archipelago)")]
        public void Should_Pass_When_Trying_To_Update_A_Customer_Country(string country)
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Country = country;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(0);
            actionTestValidate.ShouldNotHaveValidationErrorFor(x => x.Country);
        }

        #endregion Country Property Tests

        #region City Property Tests

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Null_Customer_City()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.City = null;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);
            // Assert
            actionTestValidate.Errors.Should().HaveCount(2);
            actionTestValidate.Errors[0].ErrorMessage.Should().Be("'City' deve ser informado.");
            actionTestValidate.Errors[1].ErrorMessage.Should().Be("'City' não atende a condição definida.");
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.City);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_An_Empty_Customer_City()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.City = "";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(3);
            actionTestValidate.Errors[0].ErrorMessage.Should().Be("'City' deve ser informado.");
            actionTestValidate.Errors[1].ErrorMessage.Should().Be("'City' não atende a condição definida.");
            actionTestValidate.Errors[2].ErrorMessage.Should().Be("'City' deve ser maior ou igual a 2 caracteres. Você digitou 0 caracteres.");
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.City);
        }

        [Theory]
        [InlineData("Little  Rock")]
        [InlineData(" Little Rock")]
        [InlineData("Little Rock ")]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_An_Invalid_Customer_City(string city)
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.City = city;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.City).WithErrorMessage("'City' não atende a condição definida.");
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Customer_City_Less_Than_Two_Letters()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.City = "U";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(2);
            actionTestValidate.Errors[0].ErrorMessage.Should().Be("'City' não atende a condição definida.");
            actionTestValidate.Errors[1].ErrorMessage.Should().Be("'City' deve ser maior ou igual a 2 caracteres. Você digitou 1 caracteres.");
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.City);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Customer_City_Have_Greater_Than_Fifty_Eight_Characters()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.City = "";

            while (updateCustomerRequest.City.Length < 58)
            {
                updateCustomerRequest.City += "A";
            }
            updateCustomerRequest.City += " Batata Doce";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.City).WithErrorMessage("'City' deve ser menor ou igual a 58 caracteres. Você digitou 70 caracteres.");
        }

        [Theory]
        [InlineData("Guyana")]
        [InlineData("Armenia")]
        [InlineData("Saudi Arabia")]
        [InlineData("British Indian Ocean Territory (Chagos Archipelago)")]
        public void Should_Pass_When_Trying_To_Update_A_Customer_City(string city)
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.City = city;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(0);
            actionTestValidate.ShouldNotHaveValidationErrorFor(x => x.City);
        }

        #endregion City Property Tests

        #region PostalCode Property Tests

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Null_Customer_PostalCode()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.PostalCode = null;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(2);
            actionTestValidate.Errors[0].ErrorMessage.Should().Be("'Postal Code' deve ser informado.");
            actionTestValidate.Errors[1].ErrorMessage.Should().Be("'Postal Code' não atende a condição definida.");
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.PostalCode);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_An_Empty_Customer_PostalCode()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.PostalCode = "";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(3);
            actionTestValidate.Errors[0].ErrorMessage.Should().Be("'Postal Code' deve ser informado.");
            actionTestValidate.Errors[1].ErrorMessage.Should().Be("'Postal Code' não atende a condição definida.");
            actionTestValidate.Errors[2].ErrorMessage.Should().Be("'Postal Code' deve ter no máximo 8 caracteres. Você digitou 0 caracteres.");
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.PostalCode);
        }

        [Theory]
        [InlineData("#3999501")]
        [InlineData("-3999501")]
        [InlineData(" 3999501")]
        [InlineData("@3999501")]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_An_Invalid_Customer_PostalCode_With_Special_Characters(string postalCode)
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.PostalCode = postalCode;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.PostalCode).WithErrorMessage("'Postal Code' não atende a condição definida.");
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Customer_PostalCode_Greater_Than_Eleven_Characters()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.PostalCode = "539995014920";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.PostalCode).WithErrorMessage("'Postal Code' deve ter no máximo 8 caracteres. Você digitou 12 caracteres.");
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Update_A_Customer_PostalCode()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(0);
            actionTestValidate.ShouldNotHaveValidationErrorFor(x => x.PostalCode);
        }

        #endregion PostalCode Property Tests

        #region Address Property Tests

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Null_Customer_Address()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Address = null;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);
            // Assert
            actionTestValidate.Errors.Should().HaveCount(2);
            actionTestValidate.Errors[0].ErrorMessage.Should().Be("'Address' deve ser informado.");
            actionTestValidate.Errors[1].ErrorMessage.Should().Be("'Address' não atende a condição definida.");
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Address);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_An_Empty_Customer_Address()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Address = "";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(3);
            actionTestValidate.Errors[0].ErrorMessage.Should().Be("'Address' deve ser informado.");
            actionTestValidate.Errors[1].ErrorMessage.Should().Be("'Address' não atende a condição definida.");
            actionTestValidate.Errors[2].ErrorMessage.Should().Be("'Address' deve ser maior ou igual a 2 caracteres. Você digitou 0 caracteres.");
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Address);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Customer_Address_With_Two_Spaces_Or_More_Between_Words()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Address = "Little  Rock";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Address).WithErrorMessage("'Address' não atende a condição definida.");
        }

        [Theory]
        [InlineData(" Little Rock")]
        [InlineData("Little Rock ")]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Customer_Address_With_Space_First_Or_After_Words(string address)
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Address = address;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Address).WithErrorMessage("'Address' não atende a condição definida.");
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Customer_Address_Less_Than_Two_Letters()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Address = "U";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(2);
            actionTestValidate.Errors[0].ErrorMessage.Should().Be("'Address' não atende a condição definida.");
            actionTestValidate.Errors[1].ErrorMessage.Should().Be("'Address' deve ser maior ou igual a 2 caracteres. Você digitou 1 caracteres.");
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Address);
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Customer_Address_Have_Greater_Than_One_Hundred_Characters()
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Address = "";

            while (updateCustomerRequest.Address.Length < 100)
            {
                updateCustomerRequest.Address += "A";
            }
            updateCustomerRequest.Address += " Batata Doce";

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(1);
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Address).WithErrorMessage("'Address' deve ser menor ou igual a 100 caracteres. Você digitou 112 caracteres.");
        }

        [Theory]
        [InlineData("Guyana")]
        [InlineData("Armenia")]
        [InlineData("Saudi Arabia")]
        [InlineData("British Indian Ocean Territory (Chagos Archipelago)")]
        public void Should_Pass_When_Trying_To_Update_A_Customer_Address(string address)
        {
            // Arrange
            var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            updateCustomerRequest.Address = address;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(updateCustomerRequest);

            // Assert
            actionTestValidate.Errors.Should().HaveCount(0);
            actionTestValidate.ShouldNotHaveValidationErrorFor(x => x.Address);
        }

        #endregion Updateress Property Tests

        #region Number Property Tests

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Customer_Number_Less_Than_Zero()
        {
            // Arrange
            var customerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            customerRequest.Number = -1;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(customerRequest);

            // Assert
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Number).WithErrorMessage("'Number' deve ser superior a '0'.");
        }

        [Fact]
        public void Should_Fail_And_Return_Error_With_Message_When_Trying_To_Update_A_Null_Or_Empty_Customer()
        {
            // Arrange
            var customerRequest = CustomerFactory.FakeUpdateCustomerRequest();
            customerRequest.Number = 0;

            // Act
            var actionTestValidate = _updateCustomerRequestValidator.TestValidate(customerRequest);

            // Assert
            actionTestValidate.Errors[0].ErrorMessage.Should().Be("'Number' deve ser informado.");
            actionTestValidate.Errors[1].ErrorMessage.Should().Be("'Number' deve ser superior a '0'.");
            actionTestValidate.ShouldHaveValidationErrorFor(x => x.Number);
        }

        #endregion Number Property Tests
    }
}
