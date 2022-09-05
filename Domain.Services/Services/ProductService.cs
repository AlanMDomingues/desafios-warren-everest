using Domain.Models;
using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using System;
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

        private bool AnyProductForSymbol(string symbol)
        {
            var repository = RepositoryFactory.Repository<Product>();

            return repository.Any(x => x.Symbol.Equals(symbol));
        }

        public void Add(Product product)
        {
            if (AnyProductForSymbol(product.Symbol)) throw new ArgumentException($"Product already exists for Symbol: {product.Symbol}");

            var repository = UnitOfWork.Repository<Product>();

            repository.Add(product);
            UnitOfWork.SaveChanges();
        }

        public void Update(Product product)
        {
            if (!AnyProductForId(product.Id)) throw new ArgumentException($"Product already doesn't exists for Id: {product.Id}");

            var repository = UnitOfWork.Repository<Product>();

            repository.Update(product);
            UnitOfWork.SaveChanges();
        }

        public void Delete(int id)
        {
            if (!AnyProductForId(id)) throw new ArgumentException($"Product already doesn't exists for Id: {id}");

            var repository = UnitOfWork.Repository<Product>();

            repository.Remove(x => x.Id.Equals(id));
        }
    }
}
