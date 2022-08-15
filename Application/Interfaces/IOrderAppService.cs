using Application.Models.Response;

namespace Application.Interfaces
{
    public interface IOrderAppService
    {
        public OrderResult Get(int id);
        public bool Add(int portfolioId, int productId, int quotes);
    }
}
