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

        public int Add(CreateCustomerRequest customerRequest)
        {
            var customer = Mapper.Map<Customer>(customerRequest);
            var customerExists = _customerServices.ValidateAlreadyExists(customer);
            if (customerExists) throw new ArgumentException("Customer already exists, please insert a new customer");

            _customerServices.Add(customer);
            _customerBankInfoAppService.Add(customer.Id);

            return customer.Id;
        }

        public void Update(int id, UpdateCustomerRequest customerRequest)
        {
            var customerExists = _customerServices.AnyForId(id);
            if (!customerExists) throw new ArgumentException($"'Customer' not found for ID: {id}");

            var customer = Mapper.Map<Customer>(customerRequest);
            var customerEmailOrAndCpfExists = _customerServices.ValidateAlreadyExists(customer);
            if (customerEmailOrAndCpfExists) throw new ArgumentException("Customer already exists, please insert a new customer");

            customer.Id = id;
            _customerServices.Update(customer);
        }

        public void Delete(int id)
        {
            var customerExists = _customerServices.AnyForId(id);
            if (!customerExists) throw new ArgumentException($"'Customer' not found for ID: {id}");

            var accountBalanceArentEmpty = _customerBankInfoAppService.AnyAccountBalanceThatIsntZeroForCustomerId(id);
            if (accountBalanceArentEmpty) throw new ArgumentException("You must withdraw money from your account balance before deleting it");

            var result = _portfolioAppService.AnyPortfolioFromACustomerArentEmpty(id);
            if (result) throw new ArgumentException("You must withdraw money from your portfolios before deleting it");

            _customerServices.Delete(id);
        }
    }
}