using API.Tests.Fixtures;
using Application.Models.Response;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Domain.Services.Interfaces;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace API.Tests.ApplicationTests;

public class CustomerBankInfoAppServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ICustomerBankInfoService> _customerBankInfoServiceMock;
    private readonly Mock<IInvestmentService> _investmentServiceMock;
    private readonly CustomerBankInfoAppService _customerBankInfoAppService;

    public CustomerBankInfoAppServiceTests(IMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _customerBankInfoServiceMock = new();
        _investmentServiceMock = new();
        _customerBankInfoAppService = new(
            _mapper,
            _customerBankInfoServiceMock.Object,
            _investmentServiceMock.Object);
    }

    [Fact]
    public void Should_Pass_And_Return_A_CustomerBankInfoResult_When_Trying_To_Get_A_CustomerBankInfo()
    {
        // Arrange
        var fakeCustomerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();

        var fakeCustomerBankInfoExpected = _mapper.Map<CustomerBankInfoResult>(fakeCustomerBankInfo);

        _customerBankInfoServiceMock.Setup(x => x.Get(It.IsAny<int>())).Returns(fakeCustomerBankInfo);

        // Act
        var actionTest = _customerBankInfoAppService.Get(fakeCustomerBankInfo.Id);

        // Assert
        actionTest.Should().BeEquivalentTo(fakeCustomerBankInfoExpected);
        _customerBankInfoServiceMock.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void Should_Pass_When_Trying_To_Add_A_CustomerBankInfo()
    {
        // Act
        var actionResult = () => _customerBankInfoAppService.Add(It.IsAny<int>());

        // Assert
        actionResult.Should().NotThrow();
        _customerBankInfoServiceMock.Verify(x => x.Add(It.IsAny<CustomerBankInfo>()), Times.Once);
    }

    [Fact]
    public void Should_Pass_When_Trying_To_Call_AnyAccountBalanceThatIsntZeroForCustomerId()
    {
        // Act
        var actionResult = () => _customerBankInfoAppService.IsAccountBalanceThatIsntZeroForCustomerId(It.IsAny<int>());

        // Assert
        actionResult.Should().NotThrow();
        _customerBankInfoServiceMock.Verify(x => x.IsAccountBalanceThatIsntZeroForCustomerId(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void Should_Pass_When_Trying_To_Call_AnyCustomerBankInfoForId()
    {
        // Act
        var actionResult = () => _customerBankInfoAppService.AnyCustomerBankInfoForId(It.IsAny<int>());

        // Assert
        actionResult.Should().NotThrow();
        _customerBankInfoServiceMock.Verify(x => x.AnyCustomerBankInfoForId(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void Should_Pass_When_Trying_To_Call_Deposit()
    {
        // Act
        var actionResult = () => _customerBankInfoAppService.Deposit(It.IsAny<int>(), It.IsAny<decimal>());

        // Assert
        actionResult.Should().NotThrow();
        _customerBankInfoServiceMock.Verify(x => x.Deposit(It.IsAny<int>(), It.IsAny<decimal>()), Times.Once);
    }

    [Fact]
    public void Should_Pass_When_Trying_To_Call_Withdraw()
    {
        // Act
        var actionResult = () => _customerBankInfoAppService.Withdraw(It.IsAny<int>(), It.IsAny<decimal>());

        // Assert
        actionResult.Should().NotThrow();
        _customerBankInfoServiceMock.Verify(x => x.Withdraw(It.IsAny<int>(), It.IsAny<decimal>()), Times.Once);
    }

    [Fact]
    public void Should_Pass_When_Trying_To_Transfer_Money_To_Portfolio()
    {
        // Act
        var actionResult = () => _customerBankInfoAppService.TransferMoneyToPortfolio(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>());

        // Assert
        actionResult.Should().NotThrow();
        _customerBankInfoServiceMock.Verify(x => x.Withdraw(It.IsAny<int>(), It.IsAny<decimal>()), Times.Once);
        _investmentServiceMock.Verify(x => x.DepositMoneyInPortfolio(It.IsAny<int>(), It.IsAny<decimal>()), Times.Once);
    }
}
