using Application.Models.Response;
using Domain.Models;

namespace Application.Interfaces
{
    public interface ICustomerBankInfoAppService : IAppServicesBase
    {
        CustomerBankInfoResult Get(int id);

        bool AccountBalanceIsBiggerThanZero(int customerId);

        bool AnyCustomerBankInfoForId(int customerId);

        void Add(int customerId);

        void Deposit(int id, decimal amount);

        void Withdraw(int id, decimal amount);

        void TransferMoneyToPortfolio(int customerBankInfoId, int portfolioId, decimal amount);
    }
}