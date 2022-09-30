using Application.Models.Response;
using Application.Profiles;
using Application.Services;
using AutoMapper;
using Domain.Services.Interfaces;
using Domain.Services.Services;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Tests.Factories;
using Xunit;

namespace Tests;

public class OrderAppServiceTest
{
    private readonly IMapper _mapper;
    private readonly Mock<IOrderService> _orderServiceMock;
    private readonly OrderAppService _orderAppService;

    public OrderAppServiceTest(IMapper mapper)
    {
        _mapper = mapper;
        _orderServiceMock = new Mock<IOrderService>();
        _orderAppService = new(
            _mapper,
            _orderServiceMock.Object);
    }

    [Fact]
    public void Should_Pass_And_Return_Orders_List_When_Trying_To_GetAll_Orders()
    {
        // Arrange
        var fakeOrders = OrderFactory.CreateOrders();
        var expectedOrders = _mapper.Map<IEnumerable<OrderResult>>(fakeOrders);

        _orderServiceMock.Setup(x => x.GetAll(It.IsAny<int>())).Returns(fakeOrders);

        // Act
        var actionResult = _orderAppService.GetAll(It.IsAny<int>());

        // Assert
        actionResult.Should().BeEquivalentTo(expectedOrders);
    }

    [Fact]
    public void Should_Pass_And_Return_Order_When_Trying_To_Get_Order()
    {
        // Arrange
        var fakeOrder = OrderFactory.CreateOrder();
        var expectedOrder = _mapper.Map<OrderResult>(fakeOrder);

        _orderServiceMock.Setup(x => x.Get(It.IsAny<int>())).Returns(fakeOrder);

        // Act
        var actionResult = _orderAppService.Get(It.IsAny<int>());

        // Assert
        actionResult.Should().BeEquivalentTo(expectedOrder);
    }
}
