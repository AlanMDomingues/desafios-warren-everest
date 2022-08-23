using Domain.Models;
using System.Collections.Generic;

namespace Domain.Services.Interfaces
{
    public interface IProductService : IServiceBase
    {
        public IEnumerable<Product> GetAll();

        public Product Get(int id);

        public void Add(Product product);

        public void Update(Product product);

        public void Delete(int id);
    }
}
