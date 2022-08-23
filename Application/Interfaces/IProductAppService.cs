using Application.Models.Requests;
using Application.Models.Response;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IProductAppService : IAppServicesBase
    {
        public IEnumerable<ProductResult> GetAll();

        public ProductResult Get(int id);

        public void Add(CreateProductRequest product);

        public (bool status, string message) Update(int id, UpdateProductRequest product);

        public (bool status, string message) Delete(int id);
    }
}
