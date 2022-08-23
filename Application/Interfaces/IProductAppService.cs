using Application.Models.Requests;
using Application.Models.Response;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IProductAppService : IAppServicesBase
    {
        IEnumerable<ProductResult> GetAll();

        ProductResult Get(int id);

        void Add(CreateProductRequest product);

        (bool status, string message) Update(int id, UpdateProductRequest product);

        (bool status, string message) Delete(int id);
    }
}
