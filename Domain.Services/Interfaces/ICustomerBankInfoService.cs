using Domain.Models;

namespace Domain.Services.Interfaces
{
    public interface ICustomerBankInfoService : IServiceBase
    {
        CustomerBankInfo Get(int id);

        void Add(CustomerBankInfo customerBankInfo);

        void Update(CustomerBankInfo customerBankInfo);
    }
}
