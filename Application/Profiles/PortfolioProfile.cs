using Application.Models.Requests;
using Application.Models.Response;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class PortfolioProfile : Profile
    {
        public PortfolioProfile()
        {
            CreateMap<CreatePortfolioRequest, Portfolio>();

            CreateMap<Portfolio, PortfolioResult>();
        }
    }
}
