using Application.Interfaces;
using Application.Models.Response;
using AutoMapper;
using Domain.Models;
using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Factories;
using System;

namespace Application.Services
{
    public class CustomerBankInfoAppService : AppServicesBase, ICustomerBankInfoAppService
    {
        private readonly ICustomerBankInfoService _customerBankInfoService;
        private readonly IInvestmentService _investmentService;

        public CustomerBankInfoAppService(
            IMapper mapper,
            ICustomerBankInfoService customerBankInfoService,
            IInvestmentService investmentService)
            : base(mapper)
        {
            _customerBankInfoService = customerBankInfoService ?? throw new ArgumentNullException(nameof(customerBankInfoService));
            _investmentService = investmentService ?? throw new ArgumentNullException(nameof(investmentService));
        }

        public CustomerBankInfoResult Get(int id)
        {
            var customerBankInfo = _customerBankInfoService.Get(id);
            var result = Mapper.Map<CustomerBankInfoResult>(customerBankInfo);
            return result;
        }

        public bool AccountBalanceIsNotZero(int customerId) => _customerBankInfoService.AccountBalanceIsNotZero(customerId);

        public bool AnyCustomerBankInfoForId(int customerId) => _customerBankInfoService.AnyCustomerBankInfoForId(customerId);

        public void Add(int customerId)
        {
            var randomNumber = new Random();
            string accountNumber = "";
            for (int i = 0; i < 20; i++)
                accountNumber += randomNumber.Next(0, 9).ToString();

            var customerBankInfo = new CustomerBankInfo(accountNumber, customerId);

            _customerBankInfoService.Add(customerBankInfo);
        }

        public void Deposit(int id, decimal cash) => _customerBankInfoService.Deposit(id, cash);

        public void Withdraw(int id, decimal amount) => _customerBankInfoService.Withdraw(id, amount);

        public void TransferMoneyToPortfolio(int customerBankInfoId, int portfolioId, decimal amount)
        {
            using var transactionScope = TransactionScopeFactory.CreateTransactionScope();

            _customerBankInfoService.Withdraw(customerBankInfoId, amount);

            _investmentService.DepositMoneyInPortfolio(portfolioId, amount);

            transactionScope.Complete();
        }
    }
}
