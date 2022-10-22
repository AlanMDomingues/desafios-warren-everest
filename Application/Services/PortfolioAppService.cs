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
        private readonly IInvestmentService _investmentService;

        public PortfolioAppService(
            IMapper mapper,
            IPortfolioService portfolioService,
            ICustomerBankInfoAppService customerBankInfoAppService,
            IProductAppService productAppService,
            IOrderAppService orderAppService,
            IPortfolioProductService portfolioProductService,
            IInvestmentService investmentService)
            : base(mapper)
        {
            _portfolioService = portfolioService ?? throw new ArgumentNullException(nameof(portfolioService));
            _customerBankInfoAppService = customerBankInfoAppService ?? throw new ArgumentNullException(nameof(customerBankInfoAppService));
            _productAppService = productAppService ?? throw new ArgumentNullException(nameof(productAppService));
            _orderAppService = orderAppService ?? throw new ArgumentNullException(nameof(orderAppService));
            _portfolioProductService = portfolioProductService ?? throw new ArgumentNullException(nameof(portfolioProductService));
            _investmentService = investmentService ?? throw new ArgumentNullException(nameof(investmentService));
        }

        public IEnumerable<PortfolioResult> GetAll(int customerId)
        {
            var portfolios = _portfolioService.GetAll(customerId);
            var result = Mapper.Map<IEnumerable<PortfolioResult>>(portfolios);
            return result;
        }

        public PortfolioResult Get(int id)
        {
            var portfolio = _portfolioService.Get(id);
            var result = Mapper.Map<PortfolioResult>(portfolio);
            return result;
        }

        public bool AnyPortfolioFromACustomerArentEmpty(int customerId) => _portfolioService.AnyPortfolioFromACustomerArentEmpty(customerId);

        public void Add(CreatePortfolioRequest portfolio)
        {
            if (!_customerBankInfoAppService.AnyCustomerBankInfoForId(portfolio.CustomerId)) throw new ArgumentException($"'Customer' não encontrado para o ID: {portfolio.CustomerId}");

            var portfolioToCreate = Mapper.Map<Portfolio>(portfolio);
            _portfolioService.Add(portfolioToCreate);
        }

        public void Update(int id, UpdatePortfolioRequest portfolio)
        {
            if (!_portfolioService.AnyPortfolioForId(id)) throw new ArgumentException($"'Portfolio' não encontrado para o ID: {id}");

            var portfolioToUpdate = Mapper.Map<Portfolio>(portfolio);
            portfolioToUpdate.Id = id;
            _portfolioService.Update(portfolioToUpdate);
        }

        public void Delete(int id)
        {
            if (!_portfolioService.AnyPortfolioForId(id)) throw new ArgumentException($"'Portfolio' não encontrado para o ID: {id}");

            if (_portfolioService.AnyPortfolioFromACustomerArentEmpty(id)) throw new ArgumentException("Você precisa sacar o saldo das sua carteira antes de deletá-la");

            _portfolioService.Delete(id);
        }

        public void TransferMoneyToAccountBalance(int customerBankInfoId, int portfolioId, decimal amount)
        {
            _investmentService.DepositMoneyInCustomerBankInfo(customerBankInfoId, amount);

            _portfolioService.Withdraw(portfolioId, amount);
        }

        public void Invest(CreateOrderRequest orderRequest)
        {
            if (!_customerBankInfoAppService.AnyCustomerBankInfoForId(orderRequest.CustomerBankInfoId)) throw new ArgumentException($"'Customer' não encontrado para o ID: {orderRequest.CustomerBankInfoId}");

            if (!_portfolioService.AnyPortfolioForId(orderRequest.PortfolioId)) throw new ArgumentException($"'Portfolio' não encontrado para o ID: {orderRequest.PortfolioId}");

            var product = _productAppService.Get(orderRequest.ProductId) ?? throw new ArgumentException($"'Product' não encontrado para o ID: {orderRequest.ProductId}");

            var order = Mapper.Map<Order>(orderRequest);
            order.UnitPrice = product.UnitPrice;
            order.SetNetValue();

            _portfolioService.Withdraw(order.PortfolioId, order.NetValue);

            _orderAppService.Add(order);
            _portfolioProductService.Add(order.PortfolioId, order.ProductId);
        }
    }
}
