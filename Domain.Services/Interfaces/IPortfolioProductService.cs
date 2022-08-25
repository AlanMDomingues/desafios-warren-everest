using Domain.Models;

namespace Domain.Services.Interfaces
{
    public interface IPortfolioProductService: IServiceBase
    {
        void Add(PortfolioProduct portfolioProduct);
    }
}
