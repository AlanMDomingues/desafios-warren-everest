using Domain.Models;
using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using System.Collections.Generic;

namespace Domain.Services.Services
{
    public class ProductService : ServiceBase, IProductService
    {
        public ProductService(
            IRepositoryFactory repositoryFactory,
            IUnitOfWork unitOfWork)
            : base(repositoryFactory, unitOfWork) { }

        public IEnumerable<Product> GetAll()
        {
            var repository = RepositoryFactory.Repository<Product>();

            var query = repository.MultipleResultQuery();

            var result = repository.Search(query);

            return result;
        }

        public Product Get(int id)
        {
            var repository = RepositoryFactory.Repository<Product>();

            var query = repository.SingleResultQuery()
                                  .AndFilter(x => x.Id.Equals(id));

            var result = repository.FirstOrDefault(query);

            return result;
        }

        public (bool status, string message) Add(Product product)
        {
            var repository = UnitOfWork.Repository<Product>();

            var (status, message) = ValidateAlreadyExists(product);
            if (status) return (false, message);

            repository.Add(product);
            UnitOfWork.SaveChanges();

            return (true, default);
        }

        public (bool status, string message) Update(Product product)
        {
            var repository = UnitOfWork.Repository<Product>();

            var (status, message) = ValidateAlreadyExists(product);
            if (!status) return (false, message);

            repository.Update(product);
            UnitOfWork.SaveChanges();

            return (true, default);
        }

        public void Delete(int id)
        {
            var repository = UnitOfWork.Repository<Product>();

            repository.Remove(x => x.Id.Equals(id));
        }

        private (bool status, string message) ValidateAlreadyExists(Product product)
        {
            var repository = RepositoryFactory.Repository<Product>();

            if (repository.Any(x => x.Id.Equals(product.Id)))
            {
                return (true, "Product already exists");
            }

            return (false, "Product does not exists");
        }
    }
}
