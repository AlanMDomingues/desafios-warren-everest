using Application.Interfaces;
using Application.Models.Response;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Domain.Services.Interfaces;
using FluentAssertions;
using Infrastructure.Data.Context;
using Moq;
using System;
using System.Collections.Generic;
using Tests.Factories;
using Xunit;

namespace Tests.AppServiceTests;

public class CustomerAppServiceTest
{
    private readonly IMapper _mapper;
    private readonly Mock<ICustomerService> _customerServiceMock;
    private readonly Mock<ICustomerBankInfoAppService> _customerBankInfoAppServiceMock;
    private readonly Mock<IPortfolioAppService> _portfolioAppServiceMock;
    private readonly CustomerAppService _customerAppService;

    public CustomerAppServiceTest(IMapper mapper)
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
              .WithMessage("Customer already exists, please insert a new customer");

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
        var action = _customerAppService.Add(createCustomerRequest);

        // Assert
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
              .WithMessage($"'Customer' not found for ID: {id}");

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
              .WithMessage("Customer already exists, please insert a new customer");

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
        _customerAppService.Update(It.IsAny<int>(), updateCustomerRequest);

        // Assert
        _customerServiceMock.Verify(x => x.AnyForId(It.IsAny<int>()), Times.Once);
        _customerServiceMock.Verify(x => x.ValidateAlreadyExists(It.IsAny<Customer>()), Times.Once);
        _customerServiceMock.Verify(x => x.Update(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public void Should_Pass_When_Trying_To_Get_A_Customer()
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
    public void Should_Return_Valid_CustomerResults_Successfully_When_Trying_To_Get_All_CustomerResult()
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
              .WithMessage($"'Customer' not found for ID: {id}");

        _customerServiceMock.Verify(x => x.AnyForId(It.IsAny<int>()), Times.Once);
        _customerBankInfoAppServiceMock.Verify(x => x.AnyAccountBalanceThatIsntZeroForCustomerId(It.IsAny<int>()), Times.Never);
        _portfolioAppServiceMock.Verify(x => x.AnyPortfolioFromACustomerArentEmpty(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Should_Fail_And_Return_ArgumentException_WithMessage_When_Trying_To_Delete_AccountBalance_That_Isnt_Zero()
    {
        // Arrange
        _customerServiceMock.Setup(x => x.AnyForId(It.IsAny<int>())).Returns(true);
        _customerBankInfoAppServiceMock.Setup(x => x.AnyAccountBalanceThatIsntZeroForCustomerId(It.IsAny<int>())).Returns(true);

        // Act
        var action = () => _customerAppService.Delete(It.IsAny<int>());

        // Assert
        action.Should()
              .ThrowExactly<ArgumentException>()
              .WithMessage("You must withdraw money from your account balance before deleting it");

        _customerServiceMock.Verify(x => x.AnyForId(It.IsAny<int>()), Times.Once);
        _customerBankInfoAppServiceMock.Verify(x => x.AnyAccountBalanceThatIsntZeroForCustomerId(It.IsAny<int>()), Times.Once);
        _portfolioAppServiceMock.Verify(x => x.AnyPortfolioFromACustomerArentEmpty(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Should_Fail_And_Return_ArgumentException_WithMessage_When_Trying_To_Delete_Any_Portfolio_Arent_Empty()
    {
        // Arrange
        _customerServiceMock.Setup(x => x.AnyForId(It.IsAny<int>())).Returns(true);
        _customerBankInfoAppServiceMock.Setup(x => x.AnyAccountBalanceThatIsntZeroForCustomerId(It.IsAny<int>())).Returns(false);
        _portfolioAppServiceMock.Setup(x => x.AnyPortfolioFromACustomerArentEmpty(It.IsAny<int>())).Returns(true);

        // Act
        var action = () => _customerAppService.Delete(It.IsAny<int>());

        // Assert
        action.Should()
              .ThrowExactly<ArgumentException>()
              .WithMessage("You must withdraw money from your portfolios before deleting it");

        _customerServiceMock.Verify(x => x.AnyForId(It.IsAny<int>()), Times.Once);
        _customerBankInfoAppServiceMock.Verify(x => x.AnyAccountBalanceThatIsntZeroForCustomerId(It.IsAny<int>()), Times.Once);
        _portfolioAppServiceMock.Verify(x => x.AnyPortfolioFromACustomerArentEmpty(It.IsAny<int>()), Times.Once);
    }
}
