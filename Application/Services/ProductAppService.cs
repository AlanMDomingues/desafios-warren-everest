using Application.Interfaces;
using Application.Models.Requests;
using Application.Models.Response;
using AutoMapper;
using Domain.Models;
using Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Application.Services
{
    public class ProductAppService : IProductAppService
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductAppService(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public IEnumerable<ProductResult> GetAll()
        {
            var products = _productService.GetAll();
            var result = _mapper.Map<IEnumerable<ProductResult>>(products);
            return result;
        }

        public ProductResult Get(int id)
        {
            var product = _productService.Get(id);
            var result = _mapper.Map<ProductResult>(product);
            return result;
        }

        public (bool status, string message) Add(CreateProductRequest product)
        {
            var productToCreate = _mapper.Map<Product>(product);
            return _productService.Add(productToCreate);
        }

        public (bool status, string message) Update(int id, UpdateProductRequest product)
        {
            var productToUpdate = _mapper.Map<Product>(product);
            productToUpdate.Id = id;
            return _productService.Update(productToUpdate);
        }

        public void Delete(int id) => _productService.Delete(id);
    }
}
