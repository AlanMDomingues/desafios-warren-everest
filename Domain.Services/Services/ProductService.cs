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

        public bool AnyProductForId(int id)
        {
            var repository = RepositoryFactory.Repository<Product>();

            return repository.Any(x => x.Id.Equals(id));
        }

        public void Add(Product product)
        {
            var repository = UnitOfWork.Repository<Product>();

            repository.Add(product);
            UnitOfWork.SaveChanges();
        }

        public void Update(Product product)
        {
            var repository = UnitOfWork.Repository<Product>();

            repository.Update(product);
            UnitOfWork.SaveChanges();
        }

        public void Delete(int id)
        {
            var repository = UnitOfWork.Repository<Product>();

            repository.Remove(x => x.Id.Equals(id));
        }
    }
}
