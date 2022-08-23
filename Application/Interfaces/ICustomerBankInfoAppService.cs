using Application.Models.Response;
using Domain.Models;

namespace Application.Interfaces
{
    public interface ICustomerBankInfoAppService : IAppServicesBase
    {
        CustomerBankInfoResult Get(int id);

        CustomerBankInfo GetWithoutMap(int id);

        void Add(int customerId);

        void Update(CustomerBankInfo customerBankInfo);

        void MoneyDeposit(int id, decimal cash);

        (bool status, string message) WithdrawMoney(int id, decimal cash);
    }
}