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

        public void Add(int portfolioId, int productId)
        {
            var repository = UnitOfWork.Repository<PortfolioProduct>();

            repository.Add(new PortfolioProduct(portfolioId, productId));

            UnitOfWork.SaveChanges();
        }
    }
}
