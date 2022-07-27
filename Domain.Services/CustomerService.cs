using Domain.Models;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositoryFactory _repositoryFactory;

        public CustomerService(IUnitOfWork unitOfWork, IRepositoryFactory repositoryFactory)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
        }

        public IEnumerable<Customer> GetAll()
        {
            var repository = _repositoryFactory.Repository<Customer>();

            var query = repository.MultipleResultQuery();

            var result = repository.Search(query);

            return result;
        }

        public IEnumerable<Customer> GetAll(Expression<Func<Customer, bool>> predicate)
        {
            var repository = _repositoryFactory.Repository<Customer>();

            var queryFiltered = repository.MultipleResultQuery()
                                          .AndFilter(predicate);

            var resultFiltered = repository.Search(queryFiltered);

            return resultFiltered;
        }

        public Customer GetBy(Expression<Func<Customer, bool>> predicate)
        {
            var repository = _repositoryFactory.Repository<Customer>();

            var query = repository.SingleResultQuery()
                                  .AndFilter(predicate);

            var result = repository.FirstOrDefault(query);

            return result;
        }

        public (bool exists, string message) Create(Customer newCustomer)
        {
            (bool exists, string message) = ValidateEmailAndCpfAlreadyExistsToCreate(newCustomer);

            if (exists)
            {
                return (false, message);
            }

            var repository = _unitOfWork.Repository<Customer>();

            repository.Add(newCustomer);
            _unitOfWork.SaveChanges();

            return (true, newCustomer.Id.ToString());
        }

        public (bool status, string messageResult) Update(Customer customer)
        {
            var repository = _unitOfWork.Repository<Customer>();

            var customerFound = GetBy(x => x.Id == customer.Id);
            if (customerFound is null) return (false, $"Customer not found for Id: {customer.Id}");

            (bool exists, string message) = ValidateEmailAlreadyExistsToUpdate(customerFound, customer);
            if (exists) return (false, message);

            repository.Update(customer);
            _unitOfWork.SaveChanges();

            return (true, $"Customer for ID: {customer.Id} updated successfully");
        }

        public bool Delete(int id)
        {
            var repository = _unitOfWork.Repository<Customer>();

            var customerToDelete = GetBy(x => x.Id == id);
            if (customerToDelete is null) return false;

            repository.Remove(customerToDelete);
            _unitOfWork.SaveChanges();

            return true;
        }

        private (bool exists, string message) ValidateEmailAndCpfAlreadyExistsToCreate(Customer newCustomer)
        {
            var repository = _repositoryFactory.Repository<Customer>();

            if (repository.Any(x => x.Email == newCustomer.Email)
                || repository.Any(x => x.Cpf == newCustomer.Cpf))
            {
                return (true, "Customer already exists, please insert a new customer");
            }

            return (false, newCustomer.Id.ToString());
        }

        private (bool exists, string message) ValidateEmailAlreadyExistsToUpdate(Customer oldCustomer, Customer newCustomer)
        {
            var repository = _repositoryFactory.Repository<Customer>();

            if (newCustomer.Email != oldCustomer.Email)
            {
                if (repository.Any(x => x.Email == newCustomer.Email))
                {
                    return (true, "'Email' already exists, please insert a new 'Email'");
                }
            }

            return (false, "");
        }
    }
}