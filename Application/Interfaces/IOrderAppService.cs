using Application.Models.Requests;
using Application.Models.Response;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IOrderAppService : IAppServicesBase
    {
        IEnumerable<OrderResult> GetAll(int id);

        OrderResult Get(int id);

        (bool status, string message) Add(int customerBankInfoId, CreateOrderRequest orderRequest);
    }
}
