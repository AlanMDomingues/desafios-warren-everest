using API.Tests.Fixtures;
using Domain.Services.Services;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using FluentAssertions;
using Infrastructure.Data.Context;
using Xunit;

namespace API.Tests.DomainServicesTests
{
    [Collection(nameof(DatabaseFixtureCollection))]
    public class CustomerServiceTests
    {
        private readonly CustomerService _customerService;
        private readonly IUnitOfWork<DataContext> _unitOfWork;

        public CustomerServiceTests(DatabaseFixture fixture)
        {
            var unitOfWork = fixture.CreateDbContext();
            _unitOfWork = unitOfWork;
            var repositoryFactory = (IRepositoryFactory<DataContext>)unitOfWork;
            _customerService = new(_unitOfWork, repositoryFactory);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Add_A_Customer()
        {
            // Arrange
            var customer = CustomerFactory.FakeCustomer();

            // Act
            var actionTest = () => _customerService.Add(customer);

            // Assert
            actionTest.Should().NotThrow();
            var result = _customerService.Get(x => x.Id.Equals(customer.Id));
            result.Id.Should().Be(1);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Get_A_Customer()
        {
            // Arrange
            var customer = CustomerFactory.FakeCustomer();
            _customerService.Add(customer);

            // Act
            var result = _customerService.Get(x => x.Cpf.Equals(customer.Cpf));

            // Assert
            result.Id.Should().Be(1);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Get_All_Customers()
        {
            // Arrange
            var customers = CustomerFactory.FakeCustomers();
            foreach (var item in customers)
            {
                _customerService.Add(item);
            }

            // Act
            var result = _customerService.GetAll();

            // Assert
            result.Should().HaveCount(5);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Get_All_Customers_With_Filters()
        {
            // Arrange
            var customers = CustomerFactory.FakeCustomers();
            customers[3].FullName = "João Carlos Eduardo";
            customers[4].FullName = "João Carlos Eduardo";

            foreach (var item in customers)
            {
                _customerService.Add(item);
            }

            // Act
            var result = _customerService.GetAll(x => x.FullName.Equals("João Carlos Eduardo"));

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Update_Customer()
        {
            // Arrange
            var customer = CustomerFactory.FakeCustomer();
            _customerService.Add(customer);

            customer.FullName = "Álan Domingues";

            // Act
            _customerService.Update(customer);

            // Assert
            var result = _customerService.Get(x => x.FullName.Equals("Álan Domingues"));
            result.FullName.Should().Be("Álan Domingues");
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Delete_Customer()
        {
            // Arrange
            var customer = CustomerFactory.FakeCustomer();
            _customerService.Add(customer);

            // Act
            _customerService.Delete(customer.Id);

            // Assert
            var result = _customerService.Get(x => x.Id.Equals(customer.Id));
            result.Should().BeNull();
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Validate_Customer_Already_Exists_Is_True()
        {
            // Arrange
            var customer = CustomerFactory.FakeCustomer();
            _customerService.Add(customer);

            // Act
            var result = _customerService.ValidateAlreadyExists(customer);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Validate_Customer_Already_Exists_Is_False()
        {
            // Arrange
            var customer = CustomerFactory.FakeCustomer();

            // Act
            var result = _customerService.ValidateAlreadyExists(customer);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Should_Pass_When_Trying_To_Find_Any_For_Id_Return_True()
        {
            // Arrange
            var customer = CustomerFactory.FakeCustomer();
            _customerService.Add(customer);

            // Act
            var result = _customerService.AnyForId(customer.Id);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Trying_To_Find_Any_For_Id_Return_False()
        {
            // Arrange
            var customer = CustomerFactory.FakeCustomer();
            customer.Id = 100;
            _customerService.Add(customer);

            // Act
            var result = _customerService.AnyForId(1);

            // Assert
            result.Should().BeFalse();
        }
    }
}
