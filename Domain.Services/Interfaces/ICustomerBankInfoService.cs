using Domain.Models;

namespace Domain.Services.Interfaces
{
    public interface ICustomerBankInfoService : IServiceBase
    {
        CustomerBankInfo Get(int id);

        void Add(int customerId);

        void Update(CustomerBankInfo customerBankInfo);
    }
}
