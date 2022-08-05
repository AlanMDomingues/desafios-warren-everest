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

        CustomerResult GetBy(params Expression<Func<Customer, bool>>[] predicate);

        public (bool status, string messageResult) Create(CreateCustomerRequest newCustomerDto);

        public (bool status, string messageResult) Update(int id, UpdateCustomerRequest customerToUpdateDto);

        public void Delete(int id);
    }
}