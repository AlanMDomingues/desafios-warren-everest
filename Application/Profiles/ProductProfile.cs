using Application.Models.Requests;
using Application.Models.Response;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductRequest, Product>();

            CreateMap<UpdateProductRequest, Product>();

            CreateMap<Product, ProductResult>();
        }
    }
}
