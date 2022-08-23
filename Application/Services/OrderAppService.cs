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
        private readonly IPortfolioAppService _portfolioAppService;
        private readonly IProductAppService _productAppService;
        private readonly IPortfolioProductAppService _portfolioProductAppService;
        private readonly ICustomerBankInfoAppService _customerBankInfoAppService;

        public OrderAppService(
            IMapper mapper,
            IOrderService orderService,
            IPortfolioAppService portfolioAppService,
            IProductAppService productAppService,
            IPortfolioProductAppService portfolioProductAppService,
            ICustomerBankInfoAppService customerBankInfoAppService)
            : base(mapper)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _portfolioAppService = portfolioAppService ?? throw new ArgumentNullException(nameof(portfolioAppService));
            _productAppService = productAppService ?? throw new ArgumentNullException(nameof(productAppService));
            _portfolioProductAppService = portfolioProductAppService ?? throw new ArgumentNullException(nameof(portfolioProductAppService));
            _customerBankInfoAppService = customerBankInfoAppService ?? throw new ArgumentNullException(nameof(customerBankInfoAppService));
        }

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

        public (bool status, string message) Add(int customerBankInfoId, CreateOrderRequest orderRequest)
        {
            var customerBankInfo = _customerBankInfoAppService.GetWithoutMap(customerBankInfoId);
            var portfolio = _portfolioAppService.GetPortfolioByCustomer(customerBankInfoId, orderRequest.PortfolioId);

            var (status, message) = ValidateAlreadyExists(customerBankInfo, portfolio);
            if (!status) return (false, message);

            var product = _productAppService.Get(orderRequest.ProductId);
            (status, message) = ValidateAlreadyExists(product);
            if (!status) return (false, message);

            var order = Mapper.Map<Order>(orderRequest);
            order.UnitPrice = product.UnitPrice;
            order.SetNetValue();

            (status, message) = ValidateTransaction(customerBankInfo.AccountBalance, order.NetValue);
            if (!status) return (status, message);

            customerBankInfo.AccountBalance -= order.NetValue;

            _orderService.Add(order);
            _portfolioProductAppService.Add(order.PortfolioId, order.ProductId);
            _customerBankInfoAppService.Update(customerBankInfo);

            return (true, default);
        }

        private static (bool status, string message) ValidateAlreadyExists(CustomerBankInfo customerBankInfo, Portfolio portfolio)
        {
            return customerBankInfo is null || portfolio is null
                ? (false, "'Customer' or/and 'Portfolio' not found")
                : (true, default);
        }

        private static (bool status, string message) ValidateAlreadyExists(ProductResult product)
        {
            return product is null
                ? (false, "'Product' not found")
                : (true, default);
        }

        private static (bool status, string message) ValidateTransaction(decimal totalBalance, decimal cash)
        {
            return totalBalance < cash
                ? (false, "Insufficient balance")
                : (true, default);
        }
    }
}
