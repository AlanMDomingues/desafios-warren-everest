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
            if (_customerServices.ValidateAlreadyExists(customer)) throw new ArgumentException("'Customer' já existente, por favor insira um novo 'Customer'");

            _customerServices.Add(customer);
            _customerBankInfoAppService.Add(customer.Id);

            return customer.Id;
        }

        public void Update(int id, UpdateCustomerRequest customerRequest)
        {
            if (!_customerServices.AnyForId(id)) throw new ArgumentException($"'Customer' não encontrado para o ID: {id}");

            var customer = Mapper.Map<Customer>(customerRequest);
            if (_customerServices.ValidateAlreadyExists(customer)) throw new ArgumentException("'Customer' já existente, por favor insira um novo 'Customer'");

            customer.Id = id;
            _customerServices.Update(customer);
        }

        public void Delete(int id)
        {
            if (!_customerServices.AnyForId(id)) throw new ArgumentException($"'Customer' não encontrado para o ID: {id}");

            if (_customerBankInfoAppService.AccountBalanceIsBiggerThanZero(id)) throw new ArgumentException("Você precisa sacar o saldo da sua conta antes de deletá-la");

            if (_portfolioAppService.AnyPortfolioFromACustomerArentEmpty(id)) throw new ArgumentException("Você precisa sacar o saldo das suas carteiras antes de deletá-las");

            _customerServices.Delete(id);
        }
    }
}