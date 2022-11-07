using API.Tests.Fixtures;
using Application.Interfaces;
using Application.Models.Response;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Domain.Services.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace API.Tests.ApplicationTests;

public class CustomerAppServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ICustomerService> _customerServiceMock;
    private readonly Mock<ICustomerBankInfoAppService> _customerBankInfoAppServiceMock;
    private readonly Mock<IPortfolioAppService> _portfolioAppServiceMock;
    private readonly CustomerAppService _customerAppService;

    public CustomerAppServiceTests(IMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _customerServiceMock = new();
        _customerBankInfoAppServiceMock = new();
        _portfolioAppServiceMock = new();
        _customerAppService = new(
            _mapper,
            _customerServiceMock.Object,
            _customerBankInfoAppServiceMock.Object,
            _portfolioAppServiceMock.Object);
    }

    [Fact]
    public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Add_Customer_Already_Exists()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        _customerServiceMock.Setup(x => x.ValidateAlreadyExists(It.IsAny<Customer>())).Returns(true);

        // Act
        var action = () => _customerAppService.Add(createCustomerRequest);

        // Assert
        action.Should()
              .ThrowExactly<ArgumentException>()
              .WithMessage("'Customer' já existente, por favor insira um novo 'Customer'");

        _customerServiceMock.Verify(x => x.ValidateAlreadyExists(It.IsAny<Customer>()), Times.Once);
        _customerServiceMock.Verify(x => x.Add(It.IsAny<Customer>()), Times.Never);
        _customerBankInfoAppServiceMock.Verify(x => x.Add(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Should_Pass_When_Trying_To_Add_A_Customer()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();
        _customerServiceMock.Setup(x => x.ValidateAlreadyExists(It.IsAny<Customer>())).Returns(false);

        // Act
        var action = () => _customerAppService.Add(createCustomerRequest);

        // Assert
        action.Should().NotThrow();
        _customerServiceMock.Verify(x => x.ValidateAlreadyExists(It.IsAny<Customer>()), Times.Once);
        _customerServiceMock.Verify(x => x.Add(It.IsAny<Customer>()), Times.Once);
        _customerBankInfoAppServiceMock.Verify(x => x.Add(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Update_Customer_Doesnt_Exists()
    {
        // Arrange
        var id = 1;
        var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
        _customerServiceMock.Setup(x => x.AnyForId(It.IsAny<int>())).Returns(false);

        // Act
        var action = () => _customerAppService.Update(id, updateCustomerRequest);

        // Assert
        action.Should()
              .ThrowExactly<ArgumentException>()
              .WithMessage($"'Customer' não encontrado para o ID: {id}");

        _customerServiceMock.Verify(x => x.AnyForId(It.IsAny<int>()), Times.Once);
        _customerServiceMock.Verify(x => x.ValidateAlreadyExists(It.IsAny<Customer>()), Times.Never);
        _customerServiceMock.Verify(x => x.Update(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Update_Customer_Already_Exists()
    {
        // Arrange
        var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();

        _customerServiceMock.Setup(x => x.AnyForId(It.IsAny<int>())).Returns(true);
        _customerServiceMock.Setup(x => x.ValidateAlreadyExists(It.IsAny<Customer>())).Returns(true);
        // Act
        var action = () => _customerAppService.Update(It.IsAny<int>(), updateCustomerRequest);

        // Assert
        action.Should()
              .ThrowExactly<ArgumentException>()
              .WithMessage("'Customer' já existente, por favor insira um novo 'Customer'");

        _customerServiceMock.Verify(x => x.AnyForId(It.IsAny<int>()), Times.Once);
        _customerServiceMock.Verify(x => x.ValidateAlreadyExists(It.IsAny<Customer>()), Times.Once);
        _customerServiceMock.Verify(x => x.Update(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public void Should_Pass_When_Trying_To_Update_Customer()
    {
        // Arrange
        var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();

        _customerServiceMock.Setup(x => x.AnyForId(It.IsAny<int>())).Returns(true);
        _customerServiceMock.Setup(x => x.ValidateAlreadyExists(It.IsAny<Customer>())).Returns(false);
        // Act
        var action = () => _customerAppService.Update(It.IsAny<int>(), updateCustomerRequest);

        // Assert
        action.Should().NotThrow();
        _customerServiceMock.Verify(x => x.AnyForId(It.IsAny<int>()), Times.Once);
        _customerServiceMock.Verify(x => x.ValidateAlreadyExists(It.IsAny<Customer>()), Times.Once);
        _customerServiceMock.Verify(x => x.Update(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public void Should_Pass_And_Return_CustomerResult_When_Trying_To_Get_A_Customer()
    {
        // Arrange
        var customer = CustomerFactory.FakeCustomer();

        var customerExpected = _mapper.Map<CustomerResult>(customer);

        _customerServiceMock.Setup(x => x.Get(x => x.Id.Equals(customer.Id))).Returns(customer);
        // Act
        var result = _customerAppService.Get(x => x.Id.Equals(customer.Id));

        // Assert
        result.Should().BeEquivalentTo(customerExpected);
        _customerServiceMock.Verify(x => x.Get(x => x.Id.Equals(customer.Id)), Times.Once);
    }

    [Fact]
    public void Should_Pass_And_Return_Valid_CustomerResults_Successfully_When_Trying_To_Get_All_CustomerResult()
    {
        // Arrange
        var customers = CustomerFactory.FakeCustomers();
        var customersExpected = _mapper.Map<IEnumerable<CustomerResult>>(customers);
        _customerServiceMock.Setup(x => x.GetAll()).Returns(customers);

        // Act
        var result = _customerAppService.GetAll();

        // Assert
        result.Should().BeEquivalentTo(customersExpected);
        _customerServiceMock.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public void Should_Pass_And_Return_Valid_CustomerResults_Successfully_When_Trying_To_Get_All_CustomerResult_With_Filters()
    {
        // Arrange
        var customers = CustomerFactory.FakeCustomers();
        customers[0].FullName = "José Leandro Pereira Júnior";
        customers[1].FullName = "José Leandro Pereira Júnior";
        customers[2].FullName = "José Leandro Pereira Júnior";

        var customersFound = customers.Where(x => x.FullName.Equals("José Leandro Pereira Júnior"));
        var customersExpected = _mapper.Map<IEnumerable<CustomerResult>>(customersFound);

        _customerServiceMock.Setup(x => x.GetAll(x => x.FullName.Equals("José Leandro Pereira Júnior"))).Returns(customersFound);

        // Act
        var result = _customerAppService.GetAll(x => x.FullName.Equals("José Leandro Pereira Júnior"));

        // Assert
        result.Should().BeEquivalentTo(customersExpected);
        _customerServiceMock.Verify(x => x.GetAll(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once);
    }

    [Fact]
    public void Should_Fail_And_Return_ArgumentException_WithMessage_When_Trying_To_Delete_Customer_Doesnt_Exists()
    {
        // Arrange
        var id = 1;
        _customerServiceMock.Setup(x => x.AnyForId(It.IsAny<int>())).Returns(false);

        // Act
        var action = () => _customerAppService.Delete(id);

        // Assert
        action.Should()
              .ThrowExactly<ArgumentException>()
              .WithMessage($"'Customer' não encontrado para o ID: {id}");

        _customerServiceMock.Verify(x => x.AnyForId(It.IsAny<int>()), Times.Once);
        _customerBankInfoAppServiceMock.Verify(x => x.AccountBalanceIsNotZero(It.IsAny<int>()), Times.Never);
        _portfolioAppServiceMock.Verify(x => x.AnyPortfolioFromACustomerArentEmpty(It.IsAny<int>()), Times.Never);
        _customerServiceMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Should_Fail_And_Return_ArgumentException_WithMessage_When_Trying_To_Delete_AccountBalance_That_Isnt_Zero()
    {
        // Arrange
        _customerServiceMock.Setup(x => x.AnyForId(It.IsAny<int>())).Returns(true);
        _customerBankInfoAppServiceMock.Setup(x => x.AccountBalanceIsNotZero(It.IsAny<int>())).Returns(true);

        // Act
        var action = () => _customerAppService.Delete(It.IsAny<int>());

        // Assert
        action.Should()
              .ThrowExactly<ArgumentException>()
              .WithMessage("Você precisa sacar o saldo da sua conta antes de deletá-la");

        _customerServiceMock.Verify(x => x.AnyForId(It.IsAny<int>()), Times.Once);
        _customerBankInfoAppServiceMock.Verify(x => x.AccountBalanceIsNotZero(It.IsAny<int>()), Times.Once);
        _portfolioAppServiceMock.Verify(x => x.AnyPortfolioFromACustomerArentEmpty(It.IsAny<int>()), Times.Never);
        _customerServiceMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Should_Fail_And_Return_ArgumentException_WithMessage_When_Trying_To_Delete_Any_Portfolio_Arent_Empty()
    {
        // Arrange
        _customerServiceMock.Setup(x => x.AnyForId(It.IsAny<int>())).Returns(true);
        _customerBankInfoAppServiceMock.Setup(x => x.AccountBalanceIsNotZero(It.IsAny<int>())).Returns(false);
        _portfolioAppServiceMock.Setup(x => x.AnyPortfolioFromACustomerArentEmpty(It.IsAny<int>())).Returns(true);
        _customerServiceMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);

        // Act
        var action = () => _customerAppService.Delete(It.IsAny<int>());

        // Assert
        action.Should()
              .ThrowExactly<ArgumentException>()
              .WithMessage("Você precisa sacar o saldo das suas carteiras antes de deletá-las");

        _customerServiceMock.Verify(x => x.AnyForId(It.IsAny<int>()), Times.Once);
        _customerBankInfoAppServiceMock.Verify(x => x.AccountBalanceIsNotZero(It.IsAny<int>()), Times.Once);
        _portfolioAppServiceMock.Verify(x => x.AnyPortfolioFromACustomerArentEmpty(It.IsAny<int>()), Times.Once);
        _customerServiceMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Should_Pass_When_Trying_To_Delete_A_Portfolio()
    {
        // Arrange
        _customerServiceMock.Setup(x => x.AnyForId(It.IsAny<int>())).Returns(true);
        _customerBankInfoAppServiceMock.Setup(x => x.AccountBalanceIsNotZero(It.IsAny<int>())).Returns(false);
        _portfolioAppServiceMock.Setup(x => x.AnyPortfolioFromACustomerArentEmpty(It.IsAny<int>())).Returns(false);

        // Act
        var action = () => _customerAppService.Delete(It.IsAny<int>());

        // Assert
        action.Should().NotThrow();
        _customerServiceMock.Verify(x => x.AnyForId(It.IsAny<int>()), Times.Once);
        _customerBankInfoAppServiceMock.Verify(x => x.AccountBalanceIsNotZero(It.IsAny<int>()), Times.Once);
        _portfolioAppServiceMock.Verify(x => x.AnyPortfolioFromACustomerArentEmpty(It.IsAny<int>()), Times.Once);
        _customerServiceMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
    }
}
