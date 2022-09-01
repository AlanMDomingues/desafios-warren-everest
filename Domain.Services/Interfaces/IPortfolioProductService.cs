using Domain.Models;

namespace Domain.Services.Interfaces
{
    public interface IPortfolioProductService: IServiceBase
    {
        void Add(int portfolioId, int productId);
    }
}
