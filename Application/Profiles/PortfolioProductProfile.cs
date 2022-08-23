using Application.Models.Response;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class PortfolioProductProfile : Profile
    {
        public PortfolioProductProfile()
        {
            CreateMap<PortfolioProduct, PortfolioProductResult>();
        }
    }
}
