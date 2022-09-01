using Application.Interfaces;
using Application.Models.Requests;
using Application.Models.Response;
using AutoMapper;
using Domain.Models;
using Domain.Services.Interfaces;
using System.Collections.Generic;
using System;

namespace Application.Services
{
    public class PortfolioAppService : AppServicesBase, IPortfolioAppService
    {
        private readonly IPortfolioService _portfolioService;
        private readonly ICustomerBankInfoAppService _customerBankInfoAppService;
        private readonly IProductAppService _productAppService;
        private readonly IOrderAppService _orderAppService;
        private readonly IPortfolioProductService _portfolioProductService;

        public PortfolioAppService(
            IMapper mapper,
            IPortfolioService portfolioService,
            ICustomerBankInfoAppService customerBankInfoAppService,
            IProductAppService productAppService,
            IOrderAppService orderAppService,
            IPortfolioProductService portfolioProductService)
            : base(mapper)
        {
            _portfolioService = portfolioService ?? throw new ArgumentNullException(nameof(portfolioService));
            _customerBankInfoAppService = customerBankInfoAppService ?? throw new ArgumentNullException(nameof(customerBankInfoAppService));
            _productAppService = productAppService ?? throw new ArgumentNullException(nameof(productAppService));
            _orderAppService = orderAppService ?? throw new ArgumentNullException(nameof(orderAppService));
            _portfolioProductService = portfolioProductService ?? throw new ArgumentNullException(nameof(portfolioProductService));
        }

        public IEnumerable<PortfolioResult> GetAll(int id)
        {
            var portfolios = _portfolioService.GetAll(id);
            var result = Mapper.Map<IEnumerable<PortfolioResult>>(portfolios);
            foreach (var portfoliosResult in result)
            {
                foreach (var portfolio in portfolios)
                {
                    portfoliosResult.Products = Mapper.Map<IEnumerable<PortfolioProductResult>>(portfolio.PortfoliosProducts);
                }
            }

            return result;
        }

        public PortfolioResult Get(int id)
        {
            var portfolio = _portfolioService.Get(id);
            var result = Mapper.Map<PortfolioResult>(portfolio);
            result.Products = Mapper.Map<IEnumerable<PortfolioProductResult>>(portfolio.PortfoliosProducts);
            return result;
        }

        public Portfolio GetWithoutMap(int id)
        {
            var portfolio = _portfolioService.Get(id);
            return portfolio;
        }

        public Portfolio GetPortfolioByCustomer(int customerId, int id) => _portfolioService.GetPortfolioByCustomer(customerId, id);

        public IEnumerable<Portfolio> GetAllPortfoliosByCustomer(int customerId) => _portfolioService.GetAllPortfoliosByCustomer(customerId);

        public void Add(CreatePortfolioRequest portfolio)
        {
            _ = _customerBankInfoAppService.Get(portfolio.CustomerId) ?? throw new ArgumentException($"'Customer' not found for ID: {portfolio.CustomerId}");

            var portfolioToCreate = Mapper.Map<Portfolio>(portfolio);
            _portfolioService.Add(portfolioToCreate);
        }

        public void Update(int id, UpdatePortfolioRequest portfolio)
        {
            _ = Get(id) ?? throw new ArgumentException($"'Portfolio' not found for ID: {id}");

            var portfolioToUpdate = Mapper.Map<Portfolio>(portfolio);
            portfolioToUpdate.Id = id;
            _portfolioService.Update(portfolioToUpdate);
        }

        public (bool status, string message) Delete(int id)
        {
            var portfolio = GetWithoutMap(id) ?? throw new ArgumentException($"'Portfolio' not found for ID: {id}");

            var (status, message) = portfolio.ValidateWithdrawMoneyBeforeDelete(portfolio.TotalBalance);
            if (!status) return (status, message);

            _portfolioService.Delete(id);

            return (true, default);
        }

        //public (bool status, string message) TransferMoneyToPortfolio(int customerBankInfoId, int portfolioId, decimal amount)
        //{
        //    var (status, message) = _customerBankInfoAppService.Withdraw(customerBankInfoId, amount);
        //    if (!status) return (status, message);

        //    _portfolioService.Deposit(customerBankInfoId, portfolioId, amount);

        //    return (true, default);
        //}

        public (bool status, string message) TransferMoneyToAccountBalance(int customerBankInfoId, int portfolioId, decimal amount)
        {
            _customerBankInfoAppService.Deposit(customerBankInfoId, amount);

            var (status, message) = _portfolioService.Withdraw(customerBankInfoId, portfolioId, amount);
            if (!status) return (status, message);

            return (true, default);
        }

        public (bool status, string message) Invest(int customerBankInfoId, CreateOrderRequest orderRequest)
        {
            _ = _customerBankInfoAppService.GetWithoutMap(customerBankInfoId)
                ?? throw new ArgumentException($"'Customer' not found for ID: {customerBankInfoId}");
            _ = GetPortfolioByCustomer(customerBankInfoId, orderRequest.PortfolioId) ?? throw new ArgumentException($"'Portfolio' not found for ID: {orderRequest.PortfolioId}");
            var product = _productAppService.Get(orderRequest.ProductId) ?? throw new ArgumentException($"'Product' not found for ID: {orderRequest.ProductId}");

            var order = Mapper.Map<Order>(orderRequest);
            order.UnitPrice = product.UnitPrice;
            order.SetNetValue();

            var (status, message) = _customerBankInfoAppService.Withdraw(customerBankInfoId, order.NetValue);
            if (!status) return (status, message);

            _orderAppService.Add(order);
            _portfolioProductService.Add(order.PortfolioId, order.ProductId);

            return (true, default);
        }

        public void Deposit(int customerBankInfoId, int id, decimal amount) => _portfolioService.Deposit(customerBankInfoId, id, amount);
    }
}
