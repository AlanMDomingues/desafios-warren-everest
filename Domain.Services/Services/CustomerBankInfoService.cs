using Domain.Models;
using Domain.Services.Interfaces;
using EntityFrameworkCore.Repository.Extensions;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data.Context;
using System;

namespace Domain.Services.Services
{
    public class CustomerBankInfoService : ServiceBase, ICustomerBankInfoService
    {
        public CustomerBankInfoService(
            IUnitOfWork<DataContext> unitOfWork,
            IRepositoryFactory<DataContext> repositoryFactory)
            : base(repositoryFactory, unitOfWork) { }

        public CustomerBankInfo Get(int id)
        {
            var repository = RepositoryFactory.Repository<CustomerBankInfo>();

            var query = repository.MultipleResultQuery()
                                  .AndFilter(x => x.Id.Equals(id));

            var result = repository.FirstOrDefault(query);

            return result;
        }

        public bool AccountBalanceIsBiggerThanZero(int customerId)
        {
            var repository = RepositoryFactory.Repository<CustomerBankInfo>();

            if (!AnyCustomerBankInfoForId(customerId)) throw new ArgumentException($"'CustomerBankInfo' não encontrado para o ID: {customerId}");

            return repository.Any(x => x.CustomerId.Equals(customerId) && x.AccountBalance > 0);
        }

        public bool AnyCustomerBankInfoForId(int customerId)
        {
            var repository = RepositoryFactory.Repository<CustomerBankInfo>();

            return repository.Any(x => x.CustomerId.Equals(customerId));
        }

        public void Add(CustomerBankInfo customerBankInfo)
        {
            var repository = UnitOfWork.Repository<CustomerBankInfo>();

            repository.Add(customerBankInfo);
            UnitOfWork.SaveChanges();
        }

        public void Withdraw(int id, decimal amount)
        {
            var customerBankInfo = Get(id)
                ?? throw new ArgumentException($"'CustomerBankInfo' não encontrado para o ID: {id}");

            if (!customerBankInfo.ValidateTransaction(amount)) throw new ArgumentException("Saldo insuficiente");

            customerBankInfo.AccountBalance -= amount;

            var repository = UnitOfWork.Repository<CustomerBankInfo>();
            repository.RemoveTracking(customerBankInfo);

            repository.Update(customerBankInfo, x => x.AccountBalance);
            UnitOfWork.SaveChanges();
        }

        public void Deposit(int id, decimal amount)
        {
            var customerBankInfo = Get(id)
                ?? throw new ArgumentException($"'CustomerBankInfo' não encontrado para o ID: {id}");

            customerBankInfo.AccountBalance += amount;

            var repository = UnitOfWork.Repository<CustomerBankInfo>();
            repository.RemoveTracking(customerBankInfo);

            repository.Update(customerBankInfo, x => x.AccountBalance);
            UnitOfWork.SaveChanges();
        }
    }
}
