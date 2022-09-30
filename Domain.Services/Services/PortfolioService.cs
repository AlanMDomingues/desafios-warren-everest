using Domain.Models;
using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Domain.Services.Services
{
    public class PortfolioService : ServiceBase, IPortfolioService
    {
        public PortfolioService(
            IRepositoryFactory<DataContext> repositoryFactory,
            IUnitOfWork<DataContext> unitOfWork)
            : base(repositoryFactory, unitOfWork) { }

        public IEnumerable<Portfolio> GetAll(int id)
        {
            var repository = RepositoryFactory.Repository<Portfolio>();

            var query = repository.MultipleResultQuery()
                                  .AndFilter(x => x.CustomerId.Equals(id))
                                  .Include(source => source.Include(x => x.PortfoliosProducts));

            var result = repository.Search(query);

            return result;
        }

        public Portfolio Get(int id)
        {
            var repository = RepositoryFactory.Repository<Portfolio>();

            var query = repository.SingleResultQuery()
                                  .AndFilter(x => x.Id.Equals(id))
                                  .Include(source => source.Include(x => x.PortfoliosProducts));

            var result = repository.FirstOrDefault(query);

            return result;
        }

        public bool AnyPortfolioFromACustomerArentEmpty(int customerId)
        {
            var repository = RepositoryFactory.Repository<Portfolio>();

            return repository.Any(x => x.CustomerId.Equals(customerId) && x.TotalBalance > 0);
        }

        public bool AnyPortfolioForId(int id)
        {
            var repository = RepositoryFactory.Repository<Portfolio>();

            return repository.Any(x => x.Id.Equals(id));
        }

        public void Add(Portfolio portfolio)
        {
            var repository = UnitOfWork.Repository<Portfolio>();

            repository.Add(portfolio);
            UnitOfWork.SaveChanges();
        }

        public void Update(Portfolio portfolio)
        {
            var repository = UnitOfWork.Repository<Portfolio>();

            repository.Update(portfolio);
            UnitOfWork.SaveChanges();
        }

        public void Delete(int id)
        {
            var repository = UnitOfWork.Repository<Portfolio>();

            repository.Remove(x => x.Id.Equals(id));
        }

        public void Deposit(int id, decimal amount)
        {
            var portfolio = Get(id)
                ?? throw new ArgumentException($"'Portfolio' not found for ID: {id}");

            portfolio.TotalBalance += amount;

            var repository = UnitOfWork.Repository<Portfolio>();

            repository.Update(portfolio, x => x.TotalBalance);
            UnitOfWork.SaveChanges();
        }

        public void Withdraw(int id, decimal amount)
        {
            var portfolio = Get(id)
                ?? throw new ArgumentException($"'Portfolio' not found for ID: {id}");

            if (!portfolio.ValidateTransaction(amount)) throw new ArgumentException("Insufficient balance");

            portfolio.TotalBalance -= amount;

            var repository = UnitOfWork.Repository<Portfolio>();

            repository.Update(portfolio, x => x.TotalBalance);
            UnitOfWork.SaveChanges();
        }
    }
}
