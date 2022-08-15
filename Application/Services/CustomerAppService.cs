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
    public class CustomerAppService : ICustomerAppService
    {
        private readonly ICustomerService _customerServices;
        private readonly IMapper _mapper;

        public CustomerAppService(ICustomerService customerServices, IMapper mapper)
        {
            _customerServices = customerServices ?? throw new ArgumentNullException(nameof(customerServices));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IEnumerable<CustomerResult> GetAll()
        {
            var customers = _customerServices.GetAll();
            var result = _mapper.Map<IEnumerable<CustomerResult>>(customers);
            return result;
        }

        public IEnumerable<CustomerResult> GetAll(params Expression<Func<Customer, bool>>[] predicate)
        {
            var customers = _customerServices.GetAll(predicate);
            var result = _mapper.Map<IEnumerable<CustomerResult>>(customers);
            return result;
        }

        public CustomerResult Get(params Expression<Func<Customer, bool>>[] predicate)
        {
            var customer = _customerServices.Get(predicate);
            var result = _mapper.Map<CustomerResult>(customer);
            return result;
        }

        public (bool status, string messageResult) Add(CreateCustomerRequest newCustomerDto)
        {
            var customer = _mapper.Map<Customer>(newCustomerDto);
            return _customerServices.Add(customer);
        }

        public (bool status, string messageResult) Update(int id, UpdateCustomerRequest customerToUpdateDto)
        {
            var customerToUpdate = _mapper.Map<Customer>(customerToUpdateDto);
            customerToUpdate.Id = id;
            return _customerServices.Update(customerToUpdate);
        }

        public bool Delete(int id) => _customerServices.Delete(id);
    }
}