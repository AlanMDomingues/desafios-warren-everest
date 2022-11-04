using Domain.Models;
using System.Collections.Generic;

namespace Domain.Services.Interfaces
{
    public interface IOrderService : IServiceBase
    {
        IEnumerable<Order> GetAll(int portfolioId);

        Order Get(int id);

        void Add(Order order);
    }
}
