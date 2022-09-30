using Domain.Models;
using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data.Context;
using System;

namespace Domain.Services.Services
{
    public class CustomerBankInfoService : ServiceBase, ICustomerBankInfoService
    {
        public CustomerBankInfoService(
            IRepositoryFactory<DataContext> repositoryFactory,
            IUnitOfWork<DataContext> unitOfWork)
            : base(repositoryFactory, unitOfWork) { }

        public CustomerBankInfo Get(int id)
        {
            var repository = RepositoryFactory.Repository<CustomerBankInfo>();

            var query = repository.MultipleResultQuery()
                                  .AndFilter(x => x.Id.Equals(id));

            var result = repository.FirstOrDefault(query);

            return result;
        }

        public bool AnyAccountBalanceThatIsntZeroForCustomerId(int customerId)
        {
            var repository = RepositoryFactory.Repository<CustomerBankInfo>();

            return repository.Any(x => x.CustomerId.Equals(customerId) && x.AccountBalance > 0 && x.AccountBalance < 0);
        }

        public bool AnyCustomerBankInfoForId(int id)
        {
            var repository = RepositoryFactory.Repository<CustomerBankInfo>();

            return repository.Any(x => x.CustomerId.Equals(id));
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

        public void Withdraw(int id, decimal amount)
        {
            var customerBankInfo = Get(id)
                ?? throw new ArgumentException($"'CustomerBankInfo' not found for ID: {id}");

            var isValidTransaction = customerBankInfo.ValidateTransaction(amount);
            if (isValidTransaction) throw new ArgumentException("Insufficient balance");

            customerBankInfo.AccountBalance -= amount;

            var repository = UnitOfWork.Repository<CustomerBankInfo>();

            repository.Update(customerBankInfo, x => x.AccountBalance);
            UnitOfWork.SaveChanges();
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
