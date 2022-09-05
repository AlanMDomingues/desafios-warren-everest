using Application.Models.Requests;
using Application.Models.Response;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface ICustomerAppService : IAppServicesBase
    {
        IEnumerable<CustomerResult> GetAll();

        IEnumerable<CustomerResult> GetAll(params Expression<Func<Customer, bool>>[] predicate);

        CustomerResult Get(params Expression<Func<Customer, bool>>[] predicate);

        int Add(CreateCustomerRequest newCustomerDto);

        void Update(int id, UpdateCustomerRequest customerToUpdateDto);

        void Delete(int id);
    }
}