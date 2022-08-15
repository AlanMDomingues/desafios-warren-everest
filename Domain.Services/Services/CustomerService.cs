using Domain.Models;
using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Services.Services
{
    public class CustomerService : ServiceBase, ICustomerService
    {
        private readonly ICustomerBankInfoService _customerBankInfoService;

        public CustomerService(
            ICustomerBankInfoService customerBankInfoService,
            IUnitOfWork unitOfWork,
            IRepositoryFactory repositoryFactory)
            : base(repositoryFactory, unitOfWork) 
            => _customerBankInfoService = customerBankInfoService ?? throw new ArgumentNullException(nameof(customerBankInfoService));

        public IEnumerable<Customer> GetAll()
        {
            var repository = RepositoryFactory.Repository<Customer>();

            var query = repository.MultipleResultQuery();

            var result = repository.Search(query);

            return result;
        }

        public IEnumerable<Customer> GetAll(params Expression<Func<Customer, bool>>[] predicates)
        {
            var repository = RepositoryFactory.Repository<Customer>();

            var query = repository.MultipleResultQuery();

            foreach (var item in predicates)
            {
                query.AndFilter(item);
            }

            var result = repository.Search(query);

            return result;
        }

        public Customer Get(params Expression<Func<Customer, bool>>[] predicates)
        {
            var repository = RepositoryFactory.Repository<Customer>();

            var query = repository.SingleResultQuery();
            foreach (var item in predicates)
            {
                query.AndFilter(item);
            }

            var result = repository.FirstOrDefault(query);

            return result;
        }

        public (bool exists, string message) Add(Customer newCustomer)
        {
            (bool exists, string message) = ValidateAlreadyExists(newCustomer);
            if (exists) return (false, message);

            var repository = UnitOfWork.Repository<Customer>();
            _customerBankInfoService.Add(newCustomer);

            repository.Add(newCustomer);
            UnitOfWork.SaveChanges();

            return (true, newCustomer.Id.ToString());
        }

        public (bool status, string messageResult) Update(Customer customer)
        {
            (bool exists, string message) = ValidateAlreadyExists(customer);
            if (exists) return (false, message);

            var repository = UnitOfWork.Repository<Customer>();
            repository.Update(customer);
            UnitOfWork.SaveChanges();

            return (true, $"Customer for ID: {customer.Id} updated successfully");
        }

        public bool Delete(int id)
        {
            var repository = UnitOfWork.Repository<Customer>();

            if (!repository.Any(x => x.Id.Equals(id))) return false;

            repository.Remove(x => x.Id.Equals(id));
            return true;
        }

        private (bool exists, string message) ValidateAlreadyExists(Customer customer)
        {
            var repository = RepositoryFactory.Repository<Customer>();

            if (repository.Any(x => x.Id != customer.Id && (x.Email.Equals(customer.Email) || x.Cpf.Equals(customer.Cpf))))
            {
                return (true, "Customer already exists, please insert a new customer");
            }

            return (default, customer.Id.ToString());
        }
    }
}