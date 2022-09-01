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

        void Deposit(int id, decimal amount);

        (bool status, string message) Withdraw(int id, decimal amount);

        (bool status, string message) TransferMoneyToPortfolio(int customerBankInfoId, int portfolioId, decimal amount);
    }
}