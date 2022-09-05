using Domain.Models;
using System.Collections.Generic;

namespace Domain.Services.Interfaces
{
    public interface IProductService : IServiceBase
    {
        IEnumerable<Product> GetAll();

        Product Get(int id);

        bool AnyProductForId(int id);

        void Add(Product product);

        void Update(Product product);

        void Delete(int id);
    }
}
