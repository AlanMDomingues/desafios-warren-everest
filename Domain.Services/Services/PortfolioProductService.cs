using Domain.Models;
using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;

namespace Domain.Services.Services
{
    public class PortfolioProductService : ServiceBase, IPortfolioProductService
    {
        public PortfolioProductService(
            IRepositoryFactory repositoryFactory,
            IUnitOfWork unitOfWork)
            : base(repositoryFactory, unitOfWork) { }

        public void Add(PortfolioProduct portfolioProduct)
        {
            var repository = UnitOfWork.Repository<PortfolioProduct>();

            repository.Add(portfolioProduct);

            UnitOfWork.SaveChanges();
        }
    }
}
