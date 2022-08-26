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

        public void MoneyDeposit(int id, decimal cash)
        {
            var customerBankInfo = GetWithoutMap(id) ?? throw new ArgumentException($"'Customer' not found for ID: {id}");

            customerBankInfo.AccountBalance += cash;

            _customerBankInfoService.Update(customerBankInfo);
        }

        public (bool status, string message) WithdrawMoney(int id, decimal cash)
        {
            var customerBankInfo = GetWithoutMap(id) ?? throw new ArgumentException($"'Customer' not found for ID: {id}");
            var (status, message) = ValidateTransaction(customerBankInfo.AccountBalance, cash);
            if (!status) return (status, message);

            customerBankInfo.AccountBalance -= cash;

            _customerBankInfoService.Update(customerBankInfo);

            return (true, default);
        }

        private static (bool status, string message) ValidateTransaction(decimal totalBalance, decimal cash)
        {
            return totalBalance < cash
                ? (false, "Insufficient balance")
                : (true, default);
        }
    }
}
