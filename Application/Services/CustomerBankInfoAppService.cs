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

        public void Add(int customerId) => _customerBankInfoService.Add(customerId);

        public (bool status, string message) MoneyDeposit(int id, decimal cash)
        {
            var customerBankInfo = GetWithoutMap(id);
            var (status, message) = ValidateAlreadyExists(customerBankInfo);
            if (!status) return (status, message);

            customerBankInfo.AccountBalance += cash;

            _customerBankInfoService.Update(customerBankInfo);

            return (true, default);
        }

        public (bool status, string message) WithdrawMoney(int id, decimal cash)
        {
            var customerBankInfo = GetWithoutMap(id);
            var (status, message) = ValidateAlreadyExists(customerBankInfo);
            if (!status) return (status, message);

            customerBankInfo.AccountBalance -= cash;

            _customerBankInfoService.Update(customerBankInfo);

            return (true, default);
        }

        private static (bool status, string message) ValidateAlreadyExists(CustomerBankInfo customerBankInfo)
        {
            return customerBankInfo is null
                ? (false, "'Customer' not found")
                : (true, default);
        }
    }
}
