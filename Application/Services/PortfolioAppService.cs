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

        public bool AnyPortfolioFromACustomerArentEmpty(int customerId) => _portfolioService.AnyPortfolioFromACustomerArentEmpty(customerId);

        public void Add(CreatePortfolioRequest portfolio)
        {
            var customerBankInfoExists = _customerBankInfoAppService.AnyCustomerBankInfoForId(portfolio.CustomerId);
            if (!customerBankInfoExists) throw new ArgumentException($"'Customer' not found for ID: {portfolio.CustomerId}");

            var portfolioToCreate = Mapper.Map<Portfolio>(portfolio);
            _portfolioService.Add(portfolioToCreate);
        }

        public void Update(int id, UpdatePortfolioRequest portfolio)
        {
            var portfolioExists = _portfolioService.AnyPortfolioForId(id);
            if (!portfolioExists) throw new ArgumentException($"'Portfolio' not found for ID: {id}");

            var portfolioToUpdate = Mapper.Map<Portfolio>(portfolio);
            portfolioToUpdate.Id = id;
            _portfolioService.Update(portfolioToUpdate);
        }

        public void Delete(int id)
        {
            var portfolioExists = _portfolioService.AnyPortfolioForId(id);
            if (!portfolioExists) throw new ArgumentException($"'Portfolio' not found for ID: {id}");

            var portfoliosArentEmpty = _portfolioService.AnyPortfolioFromACustomerArentEmpty(id);
            if (portfoliosArentEmpty) throw new ArgumentException("You must withdraw money from the portfolio before deleting it");

            _portfolioService.Delete(id);
        }

        public void TransferMoneyToAccountBalance(int customerBankInfoId, int portfolioId, decimal amount)
        {
            _investmentService.DepositMoneyInCustomerBankInfo(customerBankInfoId, amount);

            _portfolioService.Withdraw(portfolioId, amount);
        }

        public void Invest(int customerBankInfoId, CreateOrderRequest orderRequest)
        {
            var customerBankInfoExists = _customerBankInfoAppService.AnyCustomerBankInfoForId(customerBankInfoId);
            if (!customerBankInfoExists) throw new ArgumentException($"'Customer' not found for ID: {customerBankInfoId}");

            var portfolioExists = _portfolioService.AnyPortfolioForId(orderRequest.PortfolioId);
            if (!portfolioExists) throw new ArgumentException($"'Portfolio' not found for ID: {orderRequest.PortfolioId}");

            var product = _productAppService.Get(orderRequest.ProductId) ?? throw new ArgumentException($"'Product' not found for ID: {orderRequest.ProductId}");

            var order = Mapper.Map<Order>(orderRequest);
            order.UnitPrice = product.UnitPrice;
            order.SetNetValue();

            _portfolioService.Withdraw(order.PortfolioId, order.NetValue);

            _orderAppService.Add(order);
            _portfolioProductService.Add(order.PortfolioId, order.ProductId);
        }
    }
}
