using Application.Models.Response;
using Domain.Models;

namespace Application.Interfaces
{
    public interface ICustomerBankInfoAppService : IAppServicesBase
    {
        public CustomerBankInfoResult Get(int id);

        public CustomerBankInfo GetWithoutMap(int id);

        public void Add(int customerId);

        public void Update(CustomerBankInfo customerBankInfo);

        public (bool status, string message) MoneyDeposit(int id, decimal cash);

        public (bool status, string message) WithdrawMoney(int id, decimal cash);
    }
}