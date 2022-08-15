using Application.Interfaces;
using Application.Models.Response;
using AutoMapper;
using Domain.Services.Interfaces;
using System;

namespace Application.Services
{
    public class OrderAppService : IOrderAppService
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderAppService(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public OrderResult Get(int id)
        {
            var order = _orderService.Get(id);
            var result = _mapper.Map<OrderResult>(order);
            return result;
        }

        public bool Add(int portfolioId, int productId, int quotes) => _orderService.Add(portfolioId, productId, quotes);

    }
}
