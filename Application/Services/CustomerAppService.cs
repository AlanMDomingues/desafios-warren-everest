using Application.Interfaces;
using Application.Models.Requests;
using Application.Models.Response;
using AutoMapper;
using Domain.Models;
using Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Application.Services
{
    public class CustomerAppService : AppServicesBase, ICustomerAppService
    {
        private readonly ICustomerService _customerServices;
        private readonly ICustomerBankInfoAppService _customerBankInfoAppService;
        private readonly IPortfolioAppService _portfolioAppService;

        public CustomerAppService(
            IMapper mapper,
            ICustomerService customerServices,
            ICustomerBankInfoAppService customerBankInfoAppService,
            IPortfolioAppService portfolioAppService)
            : base(mapper)
        {
            _customerServices = customerServices ?? throw new ArgumentNullException(nameof(customerServices));
            _customerBankInfoAppService = customerBankInfoAppService ?? throw new ArgumentNullException(nameof(customerBankInfoAppService));
            _portfolioAppService = portfolioAppService ?? throw new ArgumentNullException(nameof(portfolioAppService));
        }

        public IEnumerable<CustomerResult> GetAll()
        {
            var customers = _customerServices.GetAll();
            var result = Mapper.Map<IEnumerable<CustomerResult>>(customers);
            return result;
        }

        public IEnumerable<CustomerResult> GetAll(params Expression<Func<Customer, bool>>[] predicate)
        {
            var customers = _customerServices.GetAll(predicate);
            var result = Mapper.Map<IEnumerable<CustomerResult>>(customers);
            return result;
        }

        public CustomerResult Get(params Expression<Func<Customer, bool>>[] predicate)
        {
            var customer = _customerServices.Get(predicate);
            var result = Mapper.Map<CustomerResult>(customer);
            return result;
        }

        public Customer GetWithoutMap(params Expression<Func<Customer, bool>>[] predicate)
        {
            var customer = _customerServices.Get(predicate);
            return customer;
        }

        public (bool status, string messageResult) Add(CreateCustomerRequest customerRequest)
        {
            var customer = Mapper.Map<Customer>(customerRequest);
            var (status, message) = ValidateAlreadyExists(customer);
            if (status) return (false, message);

            _customerServices.Add(customer);
            _customerBankInfoAppService.Add(customer.Id);

            return (true, customer.Id.ToString());
        }

        public (bool status, string messageResult) Update(int id, UpdateCustomerRequest customerRequest)
        {
            var customer = Mapper.Map<Customer>(customerRequest);
            (bool exists, string message) = ValidateAlreadyExists(customer);
            if (exists) return (false, message);

            customer.Id = id;
            _customerServices.Update(customer);
            return (true, default);
        }

        public (bool status, string message) Delete(int id)
        {
            _ = GetWithoutMap(x => x.Id.Equals(id)) ?? throw new ArgumentException("'Customer' not found");

            var customerBankInfo = _customerBankInfoAppService.Get(id);
            var (status, message) = ValidateWithdrawMoneyBeforeDelete(customerBankInfo.AccountBalance);
            if (!status) return (status, message);

            var portfolios = _portfolioAppService.GetAllPortfoliosByCustomer(id);

            (status, message) = ValidateWithdrawMoneyBeforeDelete(portfolios);
            if (!status) return (status, message);

            _customerServices.Delete(id);
            return (true, default);
        }

        private (bool status, string message) ValidateAlreadyExists(Customer customer)
        {
            var status = _customerServices.ValidateAlreadyExists(customer);

            return status
                ? (true, "Customer already exists, please insert a new customer")
                : (false, default);
        }

        private static (bool status, string message) ValidateWithdrawMoneyBeforeDelete(decimal totalBalance)
        {
            return totalBalance > 0
                ? (false, "You must withdraw money from your account before deleting it")
                : (true, default);
        }

        private static (bool status, string message) ValidateWithdrawMoneyBeforeDelete(IEnumerable<Portfolio> portfolios)
        {
            foreach (var item in portfolios)
            {
                if (item.TotalBalance > 0)
                {
                    return (false, "You must withdraw money from your portfolios before deleting it");
                }
            }

            return (true, default);
        }
    }
}