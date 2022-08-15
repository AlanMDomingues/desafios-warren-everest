using Application.Models.Requests;
using Application.Models.Response;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IProductAppService
    {
        public IEnumerable<ProductResult> GetAll();

        public ProductResult Get(int id);

        public (bool status, string message) Add(CreateProductRequest product);

        public (bool status, string message) Update(int id, UpdateProductRequest product);

        public void Delete(int id);
    }
}
