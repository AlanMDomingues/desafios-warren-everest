using Domain.Models;
using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using System.Collections.Generic;

namespace Domain.Services.Services
{
    public class PortfolioService : ServiceBase, IPortfolioService
    {
        private readonly ICustomerService _customerService;
        public PortfolioService(
            IRepositoryFactory repositoryFactory,
            IUnitOfWork unitOfWork,
            ICustomerService customerService)
            : base(repositoryFactory, unitOfWork)
            => _customerService = customerService;

        public IEnumerable<Portfolio> GetAll(int id)
        {
            var repository = RepositoryFactory.Repository<Portfolio>();

            var query = repository.MultipleResultQuery()
                                  .AndFilter(x => x.Id.Equals(id));

            var result = repository.Search(query);

            return result;
        }

        public Portfolio Get(int id)
        {
            var repository = RepositoryFactory.Repository<Portfolio>();

            var query = repository.MultipleResultQuery()
                                  .AndFilter(x => x.Id.Equals(id));

            var result = repository.FirstOrDefault(query);

            return result;
        }

        public bool Add(Portfolio portfolio)
        {
            var repositoryCustomer = RepositoryFactory.Repository<Customer>();
            // TODO: mover para appService
            if (!repositoryCustomer.Any(x => x.Id.Equals(portfolio.CustomerId))) return false;

            var repository = UnitOfWork.Repository<Portfolio>();

            repository.Add(portfolio);

            //var customer = _customerService.Get(x => x.Id.Equals(portfolio.CustomerId));

            //customer.Portfolios.Add(portfolio);

            //repositoryCustomer.Update(customer);
            UnitOfWork.SaveChanges();

            return true;
        }

        public bool Update(Portfolio portfolio)
        {
            var repository = UnitOfWork.Repository<Portfolio>();

            if (!repository.Any(x => x.Id.Equals(portfolio.Id))) return false;

            repository.Update(portfolio);
            UnitOfWork.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var repository = UnitOfWork.Repository<Portfolio>();

            if (!repository.Any(x => x.Id.Equals(id))) return false;

            repository.Remove(x => x.Id.Equals(id));
            return true;
        }
    }
}
