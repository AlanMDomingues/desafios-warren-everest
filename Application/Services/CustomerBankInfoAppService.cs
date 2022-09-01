using Application.Interfaces;
using Application.Models.Response;
using AutoMapper;
using Domain.Models;
using Domain.Services.Interfaces;
using System;

namespace Application.Services
{
    public class CustomerBankInfoAppService : AppServicesBase, ICustomerBankInfoAppService
    {
        private readonly ICustomerBankInfoService _customerBankInfoService;

        public CustomerBankInfoAppService(
            IMapper mapper,
            ICustomerBankInfoService customerBankInfoService)
            : base(mapper)
            => _customerBankInfoService = customerBankInfoService ?? throw new ArgumentNullException(nameof(customerBankInfoService));

        public CustomerBankInfoResult Get(int id)
        {
            var customerBankInfo = _customerBankInfoService.Get(id);
            var result = Mapper.Map<CustomerBankInfoResult>(customerBankInfo);
            return result;
        }

        public CustomerBankInfo GetWithoutMap(int id)
        {
            var customerBankInfo = _customerBankInfoService.Get(id);
            return customerBankInfo;
        }

        public void Update(CustomerBankInfo customerBankInfo) => _customerBankInfoService.Update(customerBankInfo);

        public void Add(int customerId)
        {
            var randomNumber = new Random();
            string accountNumber = "";
            for (int i = 0; i < 20; i++)
                accountNumber += randomNumber.Next(0, 9).ToString();

            var customerBankInfo = new CustomerBankInfo
            {
                Account = accountNumber,
                CustomerId = customerId
            };

            _customerBankInfoService.Add(customerBankInfo);
        }

        public void Deposit(int id, decimal cash) => _customerBankInfoService.Deposit(id, cash);

        public (bool status, string message) Withdraw(int id, decimal amount) => _customerBankInfoService.Withdraw(id, amount);

        public (bool status, string message) TransferMoneyToPortfolio(int customerBankInfoId, int portfolioId, decimal amount)
        {
            var (status, message) = Withdraw(customerBankInfoId, amount);
            if (!status) return (status, message);

            //_portfolioAppService.Deposit(customerBankInfoId, portfolioId, amount);

            return (true, default);
        }
    }
}
