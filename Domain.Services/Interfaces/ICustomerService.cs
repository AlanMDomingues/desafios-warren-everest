using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Services.Interfaces
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll();

        IEnumerable<Customer> GetAll(params Expression<Func<Customer, bool>>[] predicate);

        Customer GetBy(params Expression<Func<Customer, bool>>[] predicate);

        public (bool exists, string message) Create(Customer newCustomer);

        public (bool status, string messageResult) Update(Customer newCustomer);

        public bool Delete(int id);
    }
}