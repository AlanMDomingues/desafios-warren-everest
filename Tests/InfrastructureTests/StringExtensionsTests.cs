using FluentAssertions;
using Infrastructure.Extensions;
using Xunit;

namespace API.Tests.InfrastructureTests
{
    public class StringExtensionsTests
    {
        #region ToIntAt method tests

        [Fact]
        public void Should_Pass_When_Trying_To_Convert_String_To_Int()
        {
            // Arrange
            var number = "12345678";

            // Act
            var actionTest = number.ToIntAt(1);

            // Assert
            actionTest.Should().Be(2);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Convert_String_To_Int_From_The_End_Of_String()
        {
            // Arrange
            var number = "12345678";

            // Act
            var actionTest = number.ToIntAt(^1);

            // Assert
            actionTest.Should().Be(8);
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("123@")]
        [InlineData("999!")]
        [InlineData("123.2")]
        public void Should_Fail_When_Trying_To_Validate_A_Number(string number)
        {
            // Act
            var actionTest = number.IsValidNumber();

            // Assert
            actionTest.Should().BeFalse();
        }

        #endregion IsValidNumber method tests

        #region IsValidPlace method tests

        [Theory]
        [InlineData("Argentina")]
        [InlineData("Cameroon")]
        [InlineData("Churchill-laan 266")]
        [InlineData("Netherlands")]
        public void Should_Pass_When_Trying_To_Validate_A_Place(string place)
        {
            // Act
            var actionTest = place.IsValidPlace();

            // Assert
            actionTest.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("teste  teste")]
        [InlineData(" teste teste")]
        [InlineData("teste teste ")]
        [InlineData("aaaaaaaaaaaaaaaaa")]
        [InlineData("aaaaaaaa aaaaaaaaa")]
        public void Should_Fail_When_Trying_To_Validate_A_Place(string place)
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Teste  Teste")]
        [InlineData(" Teste teste")]
        [InlineData("Teste teste ")]
        [InlineData("Aaaaaaaaaaaaaaaaa")]
        [InlineData("Aaaaaaaa aaaaaaaaa")]
        [InlineData("alan Domingues")]
        [InlineData("Álan")]
        [InlineData("Álan Domingues 2022")]
        public void Should_Fail_When_Trying_To_Validate_A_Null_FullName(string fullName)
        {
            // Act
            var actionTest = fullName.IsValidFullName();

            // Assert
            actionTest.Should().BeFalse();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Validate_A_FullName_Have_Greater_Than_Thirty_Words()
        {
            // Arrange
            var fullName =
                "Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan Álan";

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
        [InlineData("11111111111")]
        [InlineData("111.111.111-11")]
        [InlineData("122.956.670-8@")]
        [InlineData("!22.956.670-89")]
        public void Should_Fail_When_Trying_To_Validate_CPF(string cpf)
        {
            // Act
            var actionTest = cpf.IsValidCPF();

            // Assert
            actionTest.Should().BeFalse();
        }

        #endregion IsValidCPF method tests
    }
}
