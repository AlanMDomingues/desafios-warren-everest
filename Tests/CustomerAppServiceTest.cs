using Application.Interfaces;
using Application.Models.Response;
using Application.Profiles;
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

namespace Tests;

public class CustomerAppServiceTest
{
    private readonly IMapper _mapper;
    private readonly Mock<ICustomerService> _customerServiceMock;
    private readonly Mock<ICustomerBankInfoAppService> _customerBankInfoAppServiceMock;
    private readonly Mock<IPortfolioAppService> _portfolioAppServiceMock;
    private readonly CustomerAppService _customerAppService;
    private readonly DataContext _dataContext;

    public CustomerAppServiceTest(IMapper mapper, DataContext dataContext)
    {
        _mapper = mapper;
        _customerServiceMock = new Mock<ICustomerService>();
        _customerBankInfoAppServiceMock = new Mock<ICustomerBankInfoAppService>();
        _portfolioAppServiceMock = new Mock<IPortfolioAppService>();
        _customerAppService = new CustomerAppService(
            _mapper,
            _customerServiceMock.Object,
            _customerBankInfoAppServiceMock.Object,
            _portfolioAppServiceMock.Object);
        _dataContext = dataContext;
    }

    [Fact]
    public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Add_Customer_Already_Exists()
    {
        // Arrange
        var createCustomerRequest = CustomerFactory.FakeCreateCustomerRequest();

        _customerServiceMock.Setup(x => x.ValidateAlreadyExists(It.IsAny<Customer>())).Returns(true);
        _customerServiceMock.Setup(x => x.Add()).Returns(true);
        // Act
        var action = () => _customerAppService.Add(createCustomerRequest);
        
        // Assert
        action.Should()
              .ThrowExactly<ArgumentException>()
              .WithMessage("Customer already exists, please insert a new customer");
    }

    [Fact]
    public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Update_Customer_Doesnt_Exists()
    {
        // Arrange
        int id = 1;
        var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();
        _customerServiceMock.Setup(x => x.AnyForId(id)).Returns(false);

        // Act
        var action = () => _customerAppService.Update(id, updateCustomerRequest);

        // Assert
        action.Should()
              .ThrowExactly<ArgumentException>()
              .WithMessage($"'Customer' not found for ID: {id}");
        _customerServiceMock.Verify(x => x.AnyForId(id), Times.Once);
        _customerServiceMock.Verify(x => x.Update(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public void Should_Fail_And_Return_ArgumentException_When_Trying_To_Update_Customer_Already_Exists()
    {
        // Arrange
        int id = 1;
        var updateCustomerRequest = CustomerFactory.FakeUpdateCustomerRequest();

        _customerServiceMock.Setup(x => x.AnyForId(1)).Returns(true);
        _customerServiceMock.Setup(x => x.ValidateAlreadyExists(It.IsAny<Customer>())).Returns(true);
        // Act
        var action = () => _customerAppService.Update(id, updateCustomerRequest);

        // Assert
        action.Should()
              .ThrowExactly<ArgumentException>()
              .WithMessage("Customer already exists, please insert a new customer");
        _customerServiceMock.Verify(x => x.AnyForId(id), Times.Once);
        _customerServiceMock.Verify(x => x.Update(It.IsAny<Customer>()), Times.Never);

    }

    [Fact]
    public void Should_Return_Valid_CustomerResult_Successfully_When_Trying_To_Get()
    {
        // Arrange
        var customer = CustomerFactory.FakeCustomer();

        var customerExpected = _mapper.Map<CustomerResult>(customer);

        _customerServiceMock.Setup(x => x.Get(x => x.Id.Equals(customer.Id))).Returns(customer);
        // Act
        var result = _customerAppService.Get(x => x.Id.Equals(customer.Id));

        // Assert
        result.Should().BeEquivalentTo(customerExpected);
    }

    [Fact]
    public void Should_Return_Valid_CustomersResult_Successfully_When_Trying_To_GetAll()
    {
        // Arrange
        var customers = CustomerFactory.FakeCustomers();
        var customersExpected = _mapper.Map<IEnumerable<CustomerResult>>(customers);
        _customerServiceMock.Setup(x => x.GetAll()).Returns(customers);

        // Act
        var result = _customerAppService.GetAll();

        // Assert
        result.Should().BeEquivalentTo(customersExpected);
    }

    [Fact]
    public void Should_Fail_And_Return_ArgumentException_WithMessage_When_Trying_To_Delete_Customer_Doesnt_Exists()
    {
        // Arrange
        int id = 1;
        _customerServiceMock.Setup(x => x.AnyForId(id)).Returns(false);

        // Act
        var action = () => _customerAppService.Delete(id);

        // Assert
        action.Should()
              .ThrowExactly<ArgumentException>()
              .WithMessage($"'Customer' not found for ID: {id}");
    }

    [Fact]
    public void Should_Fail_And_Return_ArgumentException_WithMessage_When_Trying_To_Delete_AccountBalance_That_Isnt_Zero()
    {
        // Arrange
        int id = 1;
        _customerServiceMock.Setup(x => x.AnyForId(id)).Returns(true);
        _customerBankInfoAppServiceMock.Setup(x => x.AnyAccountBalanceThatIsntZeroForCustomerId(id)).Returns(true);

        // Act
        var action = () => _customerAppService.Delete(id);

        // Assert
        action.Should()
              .ThrowExactly<ArgumentException>()
              .WithMessage("You must withdraw money from your account balance before deleting it");
    }

    [Fact]
    public void Should_Fail_And_Return_ArgumentException_WithMessage_When_Trying_To_Delete_Any_Portfolio_Arent_Empty()
    {
        // Arrange
        int id = 1;
        _customerServiceMock.Setup(x => x.AnyForId(id)).Returns(true);
        _customerBankInfoAppServiceMock.Setup(x => x.AnyAccountBalanceThatIsntZeroForCustomerId(id)).Returns(false);
        _portfolioAppServiceMock.Setup(x => x.AnyPortfolioFromACustomerArentEmpty(id)).Returns(true);

        // Act
        var action = () => _customerAppService.Delete(id);

        // Assert
        action.Should()
              .ThrowExactly<ArgumentException>()
              .WithMessage("You must withdraw money from your portfolios before deleting it");
    }
}
