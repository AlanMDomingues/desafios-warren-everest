using Domain.Models;
using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using System;

namespace Domain.Services.Services
{
    public class CustomerBankInfoService : ServiceBase, ICustomerBankInfoService
    {
        public CustomerBankInfoService(
            IRepositoryFactory repositoryFactory,
            IUnitOfWork unitOfWork)
            : base(repositoryFactory, unitOfWork) { }

        public CustomerBankInfo Get(int id)
        {
            var repository = RepositoryFactory.Repository<CustomerBankInfo>();

            var query = repository.MultipleResultQuery()
                                  .AndFilter(x => x.Id.Equals(id));

            var result = repository.FirstOrDefault(query);

            return result;
        }

        public void Add(CustomerBankInfo customerBankInfo)
        {
            var repository = UnitOfWork.Repository<CustomerBankInfo>();

            repository.Add(customerBankInfo);
            UnitOfWork.SaveChanges();
        }

        public void Update(CustomerBankInfo customerBankInfo)
        {
            var repository = UnitOfWork.Repository<CustomerBankInfo>();

            repository.Update(customerBankInfo);
            UnitOfWork.SaveChanges();
        }

        public (bool status, string message) Withdraw(int id, decimal amount)
        {
            var customerBankInfo = Get(id)
                ?? throw new ArgumentException($"'CustomerBankInfo' not found for ID: {id}");

            var (status, message) = customerBankInfo.ValidateTransaction(amount);
            if (!status) return (status, message);

            customerBankInfo.AccountBalance -= amount;

            var repository = UnitOfWork.Repository<CustomerBankInfo>();

            repository.Update(customerBankInfo, x => x.AccountBalance);
            UnitOfWork.SaveChanges();

            return (true, default);
        }

        public void Deposit(int id, decimal amount)
        {
            var customerBankInfo = Get(id)
                ?? throw new ArgumentException($"'CustomerBankInfo' not found for ID: {id}");

            customerBankInfo.AccountBalance += amount;

            var repository = UnitOfWork.Repository<CustomerBankInfo>();

            repository.Update(customerBankInfo, x => x.AccountBalance);
            UnitOfWork.SaveChanges();
        }
    }
}
