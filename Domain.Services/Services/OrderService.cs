using System;
using Domain.Models;
using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;

namespace Domain.Services.Services
{
    public class OrderService : ServiceBase, IOrderService
    {
        private readonly IProductService _productService;
        private readonly IPortfolioService _portfolioService;

        public OrderService(
            IRepositoryFactory repositoryFactory,
            IUnitOfWork unitOfWork,
            IProductService productService,
            IPortfolioService portfolioService)
            : base(repositoryFactory, unitOfWork)
        {
            _productService = productService;
            _portfolioService = portfolioService;
        }

        public Order Get(int id)
        {
            var repository = RepositoryFactory.Repository<Order>();

            var query = repository.SingleResultQuery()
                                  .AndFilter(x => x.Id.Equals(id));

            var result = repository.FirstOrDefault(query);

            return result;
        }

        public bool Add(int portfolioId, int productId, int quotes)
        {
            var portfolio = _portfolioService.Get(portfolioId);

            var product = _productService.Get(productId);

            // TODO: AppServices
            if (portfolio is null || product is null) return false;

            var order = new Order(quotes, portfolioId,);

            // TODO: fazer na Order.cs
            order.NetValue = product.UnitPrice * order.Quotes;

            var repositoryOrder = UnitOfWork.Repository<Order>();

            repositoryOrder.Add(order);
            UnitOfWork.SaveChanges();

            return true;
        }
    }
}
