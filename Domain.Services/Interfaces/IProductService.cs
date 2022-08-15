using Domain.Models;
using System.Collections.Generic;

namespace Domain.Services.Interfaces
{
    public interface IProductService : IServiceBase
    {
        public IEnumerable<Product> GetAll();

        public Product Get(int id);

        public (bool status, string message) Add(Product product);

        public (bool status, string message) Update(Product product);

        public void Delete(int id);
    }
}
