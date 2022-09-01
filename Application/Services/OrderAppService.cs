using Application.Interfaces;
using Application.Models.Requests;
using Application.Models.Response;
using AutoMapper;
using Domain.Models;
using Domain.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Application.Services
{
    public class OrderAppService : AppServicesBase, IOrderAppService
    {
        private readonly IOrderService _orderService;

        public OrderAppService(
            IMapper mapper,
            IOrderService orderService)
            : base(mapper) 
            => _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));

        public IEnumerable<OrderResult> GetAll(int id)
        {
            var orders = _orderService.GetAll(id);
            var results = Mapper.Map<IEnumerable<OrderResult>>(orders);

            return results;
        }

        public OrderResult Get(int id)
        {
            var order = _orderService.Get(id);
            var result = Mapper.Map<OrderResult>(order);

            return result;
        }

        public void Add(Order order) => _orderService.Add(order);
    }
}
