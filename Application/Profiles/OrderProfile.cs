using Application.Models.Requests;
using Application.Models.Response;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderRequest, Order>();

            CreateMap<Order, OrderResult>();
        }
    }
}
