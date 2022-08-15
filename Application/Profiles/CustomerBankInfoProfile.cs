using Application.Models.Response;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class CustomerBankInfoProfile : Profile
    {
        public CustomerBankInfoProfile()
        {
            CreateMap<CustomerBankInfo, CustomerBankInfoResult>();
        }
    }
}
