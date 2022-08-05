using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Services.Interfaces
{
    public interface IProductService : IServiceBase
    {
        public IEnumerable<Product> GetAllProducts();

        public Product GetProduct(Expression<Func<Product, bool>> predicate);

        public void Create(Product product);

        public bool Update(Product product);

        public void Delete(int id);
    }
}
