using Application.Models.Requests;
using Application.Models.Response;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IProductAppService : IAppServicesBase
    {
        IEnumerable<ProductResult> GetAll();

        ProductResult Get(int id);

        bool AnyProductForId(int id);

        void Add(CreateProductRequest product);

        void Update(int id, UpdateProductRequest product);

        void Delete(int id);
    }
}
