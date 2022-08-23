namespace Domain.Services.Interfaces
{
    public interface IPortfolioProductService: IServiceBase
    {
        public void Add(int portfolioId, int productId);
    }
}
