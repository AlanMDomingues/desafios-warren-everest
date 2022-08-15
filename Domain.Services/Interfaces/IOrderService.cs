using Domain.Models;

namespace Domain.Services.Interfaces
{
    public interface IOrderService : IServiceBase
    {
        public Order Get(int id);
        public bool Add(int portfolioId, int productId, int quotes);
    }
}
