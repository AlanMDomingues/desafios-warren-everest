using Domain.Models;
using System.Collections.Generic;

namespace Domain.Services.Interfaces
{
    public interface IOrderService : IServiceBase
    {
        public IEnumerable<Order> GetAll(int id);

        public Order Get(int id);

        public void Add(Order order);
    }
}
