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

        public void Add(Customer newCustomer);

        public void Update(Customer newCustomer);

        public void Delete(int id);

        public bool ValidateAlreadyExists(Customer customer);
    }
}