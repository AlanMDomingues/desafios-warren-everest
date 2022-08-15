using Application.Models.Requests;
using Application.Models.Response;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerResult>();

            CreateMap<CreateCustomerRequest, Customer>();

            CreateMap<UpdateCustomerRequest, Customer>();
        }
    }
}