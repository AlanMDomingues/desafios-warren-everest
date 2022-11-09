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

            CreateMap<UpdatePortfolioRequest, Portfolio>();

            CreateMap<Portfolio, PortfolioResult>()
                .ForMember(x => x.Products, y => y.MapFrom(z => z.PortfoliosProducts));
        }
    }
}
