using Domain.Models;
using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Services.Services
{
    public class ProductService : ServiceBase, IProductService
    {
        public ProductService(IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork
        ) : base(repositoryFactory, unitOfWork)
        { }

        public IEnumerable<Product> GetAllProducts()
        {
            var repository = RepositoryFactory.Repository<Product>();

            var query = repository.MultipleResultQuery();

            var result = repository.Search(query);

            return result;
        }

        public Product GetProduct(Expression<Func<Product, bool>> predicate)
        {
            var repository = RepositoryFactory.Repository<Product>();

            var query = repository.MultipleResultQuery()
                                  .AndFilter(predicate);

            var result = repository.FirstOrDefault(query);

            return result;
        }

        public void Create(Product product)
        {
            var repository = UnitOfWork.Repository<Product>();

            repository.Add(product);
            UnitOfWork.SaveChanges();
        }

        public bool Update(Product product)
        {
            var repository = UnitOfWork.Repository<Product>();

            if (repository.Any(x => x.ProductId == product.ProductId)) return false;

            repository.Update(product);
            UnitOfWork.SaveChanges();

            return true;
        }

        public void Delete(int id)
        {
            var repository = UnitOfWork.Repository<Product>();

            repository.Remove(x => x.ProductId.Equals(id));
        }

        private void CalculateNetValue(Product product)
        {
            var repository = UnitOfWork.Repository<Product>();

            product.NetValue = product.Quotes * product.UnitPrice;
            repository.Update(product);
            UnitOfWork.SaveChanges();
        }
    }
}
