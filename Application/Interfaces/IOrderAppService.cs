using Application.Models.Requests;
using Application.Models.Response;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IOrderAppService : IAppServicesBase
    {
        public IEnumerable<OrderResult> GetAll(int id);

        public OrderResult Get(int id);

        public (bool status, string message) Add(int customerBankInfoId, CreateOrderRequest orderRequest);
    }
}
