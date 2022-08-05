using Application.Models.Requests;
using Application.Models.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using Domain.Models;
using System.Linq.Expressions;
using Domain.Services.Interfaces;
using Application.Interfaces;

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

        public CustomerResult GetBy(params Expression<Func<Customer, bool>>[] predicate)
        {
            var customer = _customerServices.GetBy(predicate);
            var result = _mapper.Map<CustomerResult>(customer);
            return result;
        }

        public (bool status, string messageResult) Create(CreateCustomerRequest newCustomerDto)
        {
            var customer = _mapper.Map<Customer>(newCustomerDto);
            return _customerServices.Create(customer);
        }

        public (bool status, string messageResult) Update(int id, UpdateCustomerRequest customerToUpdateDto)
        {
            var customerToUpdate = _mapper.Map<Customer>(customerToUpdateDto);
            customerToUpdate.Id = id;
            return _customerServices.Update(customerToUpdate);
        }

        public void Delete(int id) => _customerServices.Delete(id);
    }
}