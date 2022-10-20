using Application.Models.Response;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Domain.Services.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Tests.Factories;
using Xunit;

namespace Tests.AppServiceTests;

public class OrderAppServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IOrderService> _orderServiceMock;
    private readonly OrderAppService _orderAppService;

    public OrderAppServiceTests(IMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _orderServiceMock = new();
        _orderAppService = new(
            _mapper,
            _orderServiceMock.Object);
    }

    [Fact]
    public void Should_Pass_And_Return_Orders_List_When_Trying_To_Get_All_Orders()
    {
        // Arrange
        var fakeOrders = OrderFactory.FakeOrders();
        var ordersExpected = _mapper.Map<IEnumerable<OrderResult>>(fakeOrders);

        _orderServiceMock.Setup(x => x.GetAll(It.IsAny<int>())).Returns(fakeOrders);

        // Act
        var actionResult = _orderAppService.GetAll(It.IsAny<int>());

        // Assert
        actionResult.Should().BeEquivalentTo(ordersExpected);
        _orderServiceMock.Verify(x => x.GetAll(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void Should_Pass_And_Return_Order_When_Trying_To_Get_Order()
    {
        // Arrange
        var fakeOrder = OrderFactory.FakeOrder();
        var orderExpected = _mapper.Map<OrderResult>(fakeOrder);

        _orderServiceMock.Setup(x => x.Get(It.IsAny<int>())).Returns(fakeOrder);

        // Act
        var actionResult = _orderAppService.Get(It.IsAny<int>());

        // Assert
        actionResult.Should().BeEquivalentTo(orderExpected);
        _orderServiceMock.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void Should_Pass_When_Trying_To_Call_Add()
    {
        // Act
        _orderAppService.Add(It.IsAny<Order>());

        // Assert
        _orderServiceMock.Verify(x => x.Add(It.IsAny<Order>()), Times.Once);
    }
}
