using Domain.Models;

namespace Domain.Services.Interfaces
{
    public interface ICustomerBankInfoService : IServiceBase
    {
        public CustomerBankInfo Get(int id);

        public void Add(int customerId);

        public void Update(CustomerBankInfo customerBankInfo);
    }
}
