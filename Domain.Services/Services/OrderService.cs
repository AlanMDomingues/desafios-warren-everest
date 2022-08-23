using Domain.Models;
using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using System.Collections.Generic;

namespace Domain.Services.Services
{
    public class OrderService : ServiceBase, IOrderService
    {
        public OrderService(
            IRepositoryFactory repositoryFactory,
            IUnitOfWork unitOfWork)
            : base(repositoryFactory, unitOfWork) { }

        public IEnumerable<Order> GetAll(int id)
        {
            var repository = RepositoryFactory.Repository<Order>();

            var query = repository.MultipleResultQuery()
                                  .AndFilter(x => x.Id.Equals(id));

            var result = repository.Search(query);

            return result;
        }

        public Order Get(int id)
        {
            var repository = RepositoryFactory.Repository<Order>();

            var query = repository.SingleResultQuery()
                                  .AndFilter(x => x.Id.Equals(id));

            var result = repository.FirstOrDefault(query);

            return result;
        }

        public void Add(Order order)
        {
            var repositoryOrder = UnitOfWork.Repository<Order>();

            repositoryOrder.Add(order);
            UnitOfWork.SaveChanges();
        }
    }
}
