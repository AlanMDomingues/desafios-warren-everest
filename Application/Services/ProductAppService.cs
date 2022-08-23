using Application.Interfaces;
using Application.Models.Requests;
using Application.Models.Response;
using AutoMapper;
using Domain.Models;
using Domain.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Application.Services
{
    public class ProductAppService : AppServicesBase, IProductAppService
    {
        private readonly IProductService _productService;

        public ProductAppService(
            IMapper mapper,
            IProductService productService)
            : base(mapper)
            => _productService = productService ?? throw new ArgumentNullException(nameof(productService));

        public IEnumerable<ProductResult> GetAll()
        {
            var products = _productService.GetAll();
            var result = Mapper.Map<IEnumerable<ProductResult>>(products);
            return result;
        }

        public ProductResult Get(int id)
        {
            var product = _productService.Get(id);
            var result = Mapper.Map<ProductResult>(product);
            return result;
        }

        public void Add(CreateProductRequest product)
        {
            var productToCreate = Mapper.Map<Product>(product);

            _productService.Add(productToCreate);
        }

        public (bool status, string message) Update(int id, UpdateProductRequest product)
        {
            var productExists = Get(id);
            var (status, message) = ValidateAlreadyExists(productExists);
            if (!status) return (status, message);

            var productToUpdate = Mapper.Map<Product>(product);
            productToUpdate.Id = id;
            _productService.Update(productToUpdate);
            return (true, default);
        }

        public (bool status, string message) Delete(int id)
        {
            var product = Get(id);
            var (status, message) = ValidateAlreadyExists(product);
            if (!status) return (status, message);

            _productService.Delete(id);
            return (true, default);
        }

        private static (bool status, string message) ValidateAlreadyExists(ProductResult product)
        {
            return product is null
                ? (false, "'Product' not found")
                : (true, default);
        }
    }
}
