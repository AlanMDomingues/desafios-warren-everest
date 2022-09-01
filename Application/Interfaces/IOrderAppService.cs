using Application.Models.Requests;
using Application.Models.Response;
using Domain.Models;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IOrderAppService : IAppServicesBase
    {
        IEnumerable<OrderResult> GetAll(int id);

        OrderResult Get(int id);

        void Add(Order order);
    }
}
