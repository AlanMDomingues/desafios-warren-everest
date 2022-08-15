using Application.Models.Requests;
using Application.Models.Response;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface ICustomerAppService
    {
        IEnumerable<CustomerResult> GetAll();

        IEnumerable<CustomerResult> GetAll(params Expression<Func<Customer, bool>>[] predicate);

        CustomerResult Get(params Expression<Func<Customer, bool>>[] predicate);

        public (bool status, string messageResult) Add(CreateCustomerRequest newCustomerDto);

        public (bool status, string messageResult) Update(int id, UpdateCustomerRequest customerToUpdateDto);

        public bool Delete(int id);
    }
}