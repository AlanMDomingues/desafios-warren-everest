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

        public PortfolioAppService(
            IMapper mapper,
            IPortfolioService portfolioService,
            ICustomerBankInfoAppService customerBankInfoAppService)
            : base(mapper)
        {
            _portfolioService = portfolioService ?? throw new ArgumentNullException(nameof(portfolioService));
            _customerBankInfoAppService = customerBankInfoAppService ?? throw new ArgumentNullException(nameof(customerBankInfoAppService));
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

            var (status, message) = ValidateWithdrawMoneyBeforeDelete(portfolio.TotalBalance);

            return !status
                ? (false, message)
                : (true, default);
        }

        public (bool status, string message) TransferMoneyToPortfolio(int customerBankInfoId, int portfolioId, decimal cash)
        {
            var customerBankInfo = _customerBankInfoAppService.GetWithoutMap(customerBankInfoId) ?? throw new ArgumentException($"'Customer' not found for ID: {customerBankInfoId}");
            var portfolio = GetPortfolioByCustomer(customerBankInfoId, portfolioId) ?? throw new ArgumentException($"'Portfolio' not found for ID: {portfolioId}, on customer with ID: {customerBankInfoId}");

            var (status, message) = ValidateTransaction(customerBankInfo.AccountBalance, cash);
            if (!status) return (status, message);

            customerBankInfo.AccountBalance -= cash;
            portfolio.TotalBalance += cash;

            _portfolioService.TransferMoneyToPortfolioOrAccountBalance(customerBankInfo, portfolio);
            return (true, default);
        }

        public (bool status, string message) TransferMoneyToAccountBalance(int customerBankInfoId, int portfolioId, decimal cash)
        {
            var customerBankInfo = _customerBankInfoAppService.GetWithoutMap(customerBankInfoId) ?? throw new ArgumentException($"'Customer' not found for Id: {customerBankInfoId}");
            var portfolio = GetPortfolioByCustomer(customerBankInfoId, portfolioId) ?? throw new ArgumentException($"'Portfolio' not found for ID: {portfolioId}, on Customer with ID: {customerBankInfoId}");

            var (status, message) = ValidateTransaction(portfolio.TotalBalance, cash);
            if (!status) return (status, message);

            portfolio.TotalBalance -= cash;
            customerBankInfo.AccountBalance += cash;

            _portfolioService.TransferMoneyToPortfolioOrAccountBalance(customerBankInfo, portfolio);
            return (true, default);
        }

        private static (bool status, string message) ValidateTransaction(decimal totalBalance, decimal cash)
        {
            return totalBalance < cash
                ? (false, "Insufficient balance")
                : (true, default);
        }

        private static (bool status, string message) ValidateWithdrawMoneyBeforeDelete(decimal totalBalance)
        {
            return totalBalance > 0
                ? (false, "You must withdraw money from the portfolio before deleting it")
                : (true, default);
        }
    }
}
