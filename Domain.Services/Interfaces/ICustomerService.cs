using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Services.Interfaces
{
    public interface ICustomerService : IServiceBase
    {
        IEnumerable<Customer> GetAll();

        IEnumerable<Customer> GetAll(params Expression<Func<Customer, bool>>[] predicate);

        Customer Get(params Expression<Func<Customer, bool>>[] predicate);

        void Add(Customer newCustomer);

        void Update(Customer newCustomer);

        void Delete(int id);

        bool ValidateAlreadyExists(Customer customer);

        bool AnyForId(int id);
    }
}