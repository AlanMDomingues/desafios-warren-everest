using FluentAssertions;
using Infrastructure.Extensions;
using System;
using Xunit;

namespace Tests.InfrastructureTests
{
    public class StringExtensionsTests
    {
        #region ToIntAt method tests

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Should_Pass_When_Trying_To_Convert_String_To_Int(Index index)
        {
            // Arrange
            var number = "123";
            var numberConverted = (int)char.GetNumericValue(number, index.Value);

            // Act
            var actionTest = number.ToIntAt(index);

            // Assert
            actionTest.Should().Be(numberConverted);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Convert_String_To_Int_From_The_End()
        {
            // Arrange
            var number = "12345678";
            var index = ^2;
            var numberConverted = (int)char.GetNumericValue(number, (number.Length - index.Value));

            // Act
            var actionTest = number.ToIntAt(index);

            // Assert
            actionTest.Should().Be(numberConverted);
        }

        #endregion ToIntAt method tests

        #region IsValidNumber method tests

        [Fact]
        public void Should_Pass_When_Trying_To_Validate_Number()
        {
            // Arrange
            var number = "987654321";

            // Act
            var actionTest = number.IsValidNumber();

            // Assert
            actionTest.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Validate_Null_Number()
        {
            // Arrange
            string number = null;

            // Act
            var actionTest = number.IsValidNumber();

            // Assert
            actionTest.Should().BeFalse();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Validate_Empty_Number()
        {
            // Arrange
            string number = "";

            // Act
            var actionTest = number.IsValidNumber();

            // Assert
            actionTest.Should().BeTrue();
        }

        [Theory]
        [InlineData("123@")]
        [InlineData("999!")]
        [InlineData("123.2")]
        [InlineData("123*")]
        [InlineData("123()")]
        [InlineData("123&")]
        [InlineData("123%")]
        [InlineData("123-")]
        [InlineData("123+")]
        [InlineData("123=")]
        public void Should_Fail_When_Trying_To_Validate_Number_With_Special_Characters(string number)
        {
            // Act
            var actionTest = number.IsValidNumber();

            // Assert
            actionTest.Should().BeFalse();
        }

        #endregion IsValidNumber method tests

        #region IsValidPlace method tests

        [Fact]
        public void Should_Pass_When_Trying_To_Validate_A_Place()
        {
            // Arrange
            var place = "Argentina";

            // Act
            var actionTest = place.IsValidPlace();

            // Assert
            actionTest.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Validate_A_Null_Place()
        {
            // Arrange
            string place = null;

            // Act
            var actionTest = place.IsValidPlace();

            // Assert
            actionTest.Should().BeFalse();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Validate_An_Empty_Place()
        {
            // Arrange
            string place = "";

            // Act
            var actionTest = place.IsValidPlace();

            // Assert
            actionTest.Should().BeFalse();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Validate_An_Place_With_Two_Spaces_Or_More_Between_Words()
        {
            // Arrange
            string place = "teste  teste";

            // Act
            var actionTest = place.IsValidPlace();

            // Assert
            actionTest.Should().BeFalse();
        }

        [Theory]
        [InlineData(" teste teste")]
        [InlineData("teste teste ")]
        public void Should_Fail_When_Trying_To_Validate_An_Place_With_Space_First_Or_After_Words(string place)
        {
            // Act
            var actionTest = place.IsValidPlace();

            // Assert
            actionTest.Should().BeFalse();
        }

        [Theory]
        [InlineData("aaaaaaaaaaaaaaaaa")]
        [InlineData("aaaaaaaa aaaaaaaaa")]
        public void Should_Fail_When_Trying_To_Validate_An_Place_With_All_Characters_Equals_To_The_First_Character(string place)
        {
            // Act
            var actionTest = place.IsValidPlace();

            // Assert
            actionTest.Should().BeFalse();
        }

        #endregion IsValidPlace method tests

        #region CpfFormatter method tests

        [Fact]
        public void Should_Pass_When_Trying_To_Format_Cpf()
        {
            // Arrange
            var cpf = "312.233.090-37";

            // Act
            var actionTest = cpf.CpfFormatter();

            // Assert
            actionTest.Should().Be("31223309037");
        }

        #endregion CpfFormatter method tests

        #region IsValidFullName method tests

        [Theory]
        [InlineData("Jô Soares")]
        [InlineData("Gabriela Amaro")]
        [InlineData("Lucas Albino")]
        [InlineData("Carlos Aurelio Casanova de la Torre Ugarte")]
        public void Should_Pass_When_Trying_To_Validate_A_FullName(string fullName)
        {
            // Act
            var actionTest = fullName.IsValidFullName();

            // Assert
            actionTest.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Validate_A_Null_FullName()
        {
            // Arrange
            string fullName = null;

            // Act
            var actionTest = fullName.IsValidFullName();

            // Assert
            actionTest.Should().BeFalse();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Validate_An_Empty_FullName()
        {
            // Arrange
            string fullName = "";

            // Act
            var actionTest = fullName.IsValidFullName();

            // Assert
            actionTest.Should().BeFalse();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Validate_A_FullName_With_Two_Spaces_Or_More_Between_Words()
        {
            // Arrange
            string fullName = "Teste  Teste";

            // Act
            var actionTest = fullName.IsValidFullName();

            // Assert
            actionTest.Should().BeFalse();
        }

        [Theory]
        [InlineData(" Teste teste")]
        [InlineData("Teste teste ")]
        public void Should_Fail_When_Trying_To_Validate_An_FullName_With_Space_First_Or_After_Words(string fullName)
        {
            // Act
            var actionTest = fullName.IsValidFullName();

            // Assert
            actionTest.Should().BeFalse();
        }

        [Theory]
        [InlineData("Aaaaaaaaaaaaaaaaa")]
        [InlineData("Aaaaaaaa aaaaaaaaa")]
        public void Should_Fail_When_Trying_To_Validate_An_FullName_With_All_Characters_Equals_To_The_First_Character(string fullName)
        {
            // Act
            var actionTest = fullName.IsValidFullName();

            // Assert
            actionTest.Should().BeFalse();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Validate_A_FullName_Without_First_Letter_In_UpperCase()
        {
            // Arrange
            string fullName = "alan Domingues";

            // Act
            var actionTest = fullName.IsValidFullName();

            // Assert
            actionTest.Should().BeFalse();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Validate_A_FullName_Have_Less_Than_Two_Words()
        {
            // Arrange
            string fullName = "Álan";

            // Act
            var actionTest = fullName.IsValidFullName();

            // Assert
            actionTest.Should().BeFalse();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Validate_A_FullName_Have_Greater_Than_Thirty_Words()
        {
            // Arrange
            var fullName = "";
            for (int i = 0; i < 30; i++)
            {
                fullName += "Carlos ";
            }
            fullName += "Augusto";

            // Act
            var actionTest = fullName.IsValidFullName();

            // Assert
            actionTest.Should().BeFalse();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Validate_A_FullName_With_Numbers()
        {
            // Arrange
            string fullName = "Álan Domingues 2022";

            // Act
            var actionTest = fullName.IsValidFullName();

            // Assert
            actionTest.Should().BeFalse();
        }

        #endregion IsValidFullName method tests

        #region AllCharacteresAreEqualsToTheFirstCharacter method tests

        [Theory]
        [InlineData("Aaaaaaaaaaaaaaaaa")]
        [InlineData("aaaaaaaaaaaaaaaaa")]
        [InlineData("AAAAAAAAAAAAAAAAA")]
        [InlineData("AAAAAAAA AAAAAAAAA")]
        [InlineData("11111111111")]
        public void Should_Pass_When_Trying_To_Validate_A_Field_With_All_Characteres_Equals_To_The_First_Character(string fullName)
        {
            // Act
            var actionTest = fullName.AllCharacteresAreEqualsToTheFirstCharacter();

            // Assert
            actionTest.Should().BeTrue();
        }

        [Theory]
        [InlineData("Aaaaaaaaaaaaaaaab")]
        [InlineData("aaaaaaaaaaaaaaaab")]
        [InlineData("AAAAAAAAAAAAAAAAB")]
        [InlineData("AAAAAAAA AAAAAAAAB")]
        [InlineData("11111111112")]
        public void Should_Fail_When_Trying_To_Validate_A_Field_Without_All_Characteres_Equals_To_The_First_Character(string fullName)
        {
            // Act
            var actionTest = fullName.AllCharacteresAreEqualsToTheFirstCharacter();

            // Assert
            actionTest.Should().BeFalse();
        }

        #endregion AllCharacteresAreEqualsToTheFirstCharacter method tests

        #region IsValidCPF method tests

        [Theory]
        [InlineData("122.956.670-89")]
        [InlineData("151.501.400-23")]
        [InlineData("088.466.970-06")]
        [InlineData("22622433018")]
        [InlineData("38205484082")]
        [InlineData("38851575002")]
        public void Should_Pass_When_Trying_To_Validate_CPF(string cpf)
        {
            // Act
            var actionTest = cpf.IsValidCPF();

            // Assert
            actionTest.Should().BeTrue();
        }

        [Theory]
        [InlineData("120.956.670-89")]
        [InlineData("153.501.400-23")]
        [InlineData("089.466.970-06")]
        [InlineData("22622433019")]
        [InlineData("38205484086")]
        [InlineData("38851575004")]
        public void Should_Fail_When_Trying_To_Validate_CPF(string cpf)
        {
            // Act
            var actionTest = cpf.IsValidCPF();

            // Assert
            actionTest.Should().BeFalse();
        }

        [Theory]
        [InlineData("11111111111")]
        [InlineData("111.111.111-11")]
        public void Should_Fail_When_Trying_To_Validate_CPF_With_All_Numbers_Equals_To_The_First_Number(string cpf)
        {
            // Act
            var actionTest = cpf.IsValidCPF();

            // Assert
            actionTest.Should().BeFalse();
        }

        [Theory]
        [InlineData("122.956.670-8@")]
        [InlineData("!22.956.670-89")]
        public void Should_Fail_When_Trying_To_Validate_CPF_With_Special_Characters(string cpf)
        {
            // Act
            var actionTest = cpf.IsValidCPF();

            // Assert
            actionTest.Should().BeFalse();
        }

        #endregion IsValidCPF method tests
    }
}
