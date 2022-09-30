using Application.Models.Response;
using Application.Profiles;
using Application.Services;
using AutoMapper;
using Domain.Services.Interfaces;
using FluentAssertions;
using Moq;
using Tests.Factories;
using Xunit;

namespace Tests;

public class CustomerBankInfoAppServiceTest
{
    private readonly IMapper _mapper;
    private readonly Mock<ICustomerBankInfoService> _customerBankInfoServiceMock;
    private readonly Mock<IInvestmentService> _investmentService;
    private readonly CustomerBankInfoAppService _customerBankInfoAppService;

    public CustomerBankInfoAppServiceTest(IMapper mapper)
    {
        _mapper = mapper;
        _customerBankInfoServiceMock = new();
        _investmentService = new();
        _customerBankInfoAppService = new(
            _mapper,
            _customerBankInfoServiceMock.Object,
            _investmentService.Object);
    }

    [Fact]
    public void Should_Pass_And_Return_A_CustomerBankInfoResult_When_Trying_To_Get_A_CustomerBankInfo()
    {
        // Arrange
        var fakeCustomerBankInfo = CustomerBankInfoFactory.FakeCustomerBankInfo();
        fakeCustomerBankInfo.Id = 1;

        var expectedFakeCustomerBankInfo = _mapper.Map<CustomerBankInfoResult>(fakeCustomerBankInfo);

        _customerBankInfoServiceMock.Setup(x => x.Get(fakeCustomerBankInfo.Id)).Returns(fakeCustomerBankInfo);
        // Act
        var actionTest = _customerBankInfoAppService.Get(fakeCustomerBankInfo.Id);

        // Assert
        actionTest.Should().BeEquivalentTo(expectedFakeCustomerBankInfo);
    }
}
