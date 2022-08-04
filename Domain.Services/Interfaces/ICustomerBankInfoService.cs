using Domain.Models;

namespace Domain.Services.Interfaces
{
    public interface ICustomerBankInfoService : IServiceBase
    {
        public void Create(Customer customer);
    }
}
