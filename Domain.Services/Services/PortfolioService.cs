using Domain.Models;
using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Domain.Services.Services
{
    public class PortfolioService : ServiceBase, IPortfolioService
    {
        public PortfolioService(
            IRepositoryFactory repositoryFactory,
            IUnitOfWork unitOfWork)
            : base(repositoryFactory, unitOfWork) { }

        public IEnumerable<Portfolio> GetAll(int id)
        {
            var repository = RepositoryFactory.Repository<Portfolio>();

            var query = repository.MultipleResultQuery()
                                  .AndFilter(x => x.Id.Equals(id))
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

        public Portfolio GetPortfolioByCustomer(int customerId, int id)
        {
            var repository = RepositoryFactory.Repository<Portfolio>();

            var query = repository.SingleResultQuery()
                                  .AndFilter(x => x.Id.Equals(id))
                                  .AndFilter(x => x.CustomerId.Equals(customerId));

            var result = repository.FirstOrDefault(query);

            return result;
        }

        public IEnumerable<Portfolio> GetAllPortfoliosByCustomer(int customerId)
        {
            var repository = RepositoryFactory.Repository<Portfolio>();

            var query = repository.MultipleResultQuery()
                                  .AndFilter(x => x.CustomerId.Equals(customerId));

            var result = repository.Search(query);

            return result;
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

        public void TransferMoneyToPortfolioOrAccountBalance(CustomerBankInfo customerBankInfo, Portfolio portfolio)
        {
            var repository = UnitOfWork.Repository<CustomerBankInfo>();
            var repositoryPortfolio = UnitOfWork.Repository<Portfolio>();

            repository.Update(customerBankInfo);
            repositoryPortfolio.Update(portfolio);
            UnitOfWork.SaveChanges();
        }
    }
}
