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
            var portfolio = _portfolioService.GetAll(id);
            var result = Mapper.Map<IEnumerable<PortfolioResult>>(portfolio);
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

        public (bool status, string message) Add(CreatePortfolioRequest portfolio)
        {
            var customer = _customerBankInfoAppService.Get(portfolio.CustomerId);
            var (status, message) = ValidateAlreadyExists(customer);
            if (!status) return (status, message);

            var portfolioToCreate = Mapper.Map<Portfolio>(portfolio);
            _portfolioService.Add(portfolioToCreate);
            return (true, default);
        }

        public (bool status, string message) Update(int id, UpdatePortfolioRequest portfolio)
        {
            var portfolioExists = GetWithoutMap(id);
            var (status, message) = ValidateAlreadyExists(portfolioExists);
            if (!status) return (status, message);

            var portfolioToUpdate = Mapper.Map<Portfolio>(portfolio);
            portfolioToUpdate.Id = id;
            _portfolioService.Update(portfolioToUpdate);

            return (true, default);
        }

        public (bool status, string message) Delete(int id)
        {
            var portfolio = GetWithoutMap(id);
            var (status, message) = ValidateAlreadyExists(portfolio);
            if (!status) return (status, message);

            (status, message) = ValidateWithdrawMoneyBeforeDelete(portfolio.TotalBalance);

            return !status
                ? (false, message)
                : (true, default);
        }

        public (bool status, string message) TransferMoneyToPortfolio(int customerBankInfoId, int portfolioId, decimal cash)
        {
            var customerBankInfo = _customerBankInfoAppService.GetWithoutMap(customerBankInfoId);
            var portfolio = GetPortfolioByCustomer(customerBankInfoId, portfolioId);

            var (status, message) = ValidateAlreadyExists(customerBankInfo, portfolio);
            if (status) return (status, message);

            (status, message) = ValidateTransaction(customerBankInfo.AccountBalance, cash);
            if (!status) return (status, message);

            customerBankInfo.AccountBalance -= cash;
            portfolio.TotalBalance += cash;

            _portfolioService.TransferMoneyToPortfolioOrAccountBalance(customerBankInfo, portfolio);
            return (true, default);
        }

        public (bool status, string message) TransferMoneyToAccountBalance(int customerBankInfoId, int portfolioId, decimal cash)
        {
            var customerBankInfo = _customerBankInfoAppService.GetWithoutMap(customerBankInfoId);
            var portfolio = GetPortfolioByCustomer(customerBankInfoId, portfolioId);

            var (status, message) = ValidateAlreadyExists(customerBankInfo, portfolio);
            if (!status) return (status, message);

            (status, message) = ValidateTransaction(portfolio.TotalBalance, cash);
            if (!status) return (status, message);

            portfolio.TotalBalance -= cash;
            customerBankInfo.AccountBalance += cash;

            _portfolioService.TransferMoneyToPortfolioOrAccountBalance(customerBankInfo, portfolio);
            return (true, default);
        }

        private static (bool status, string message) ValidateAlreadyExists(CustomerBankInfo customerBankInfo, Portfolio portfolio)
        {
            return customerBankInfo is null || portfolio is null
                ? (false, "'Customer' or 'Portfolio' not found")
                : (true, default);
        }

        private static (bool status, string message) ValidateAlreadyExists(Portfolio portfolio)
        {
            return portfolio is null
                ? (false, "'Portfolio' not found")
                : (true, default);
        }

        private static (bool status, string message) ValidateAlreadyExists(CustomerBankInfoResult customer)
        {
            return customer is null
                ? (false, "'Customer' not found")
                : (true, default);
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
