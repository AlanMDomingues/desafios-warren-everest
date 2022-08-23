using Domain.Models;
using System.Collections.Generic;

namespace Domain.Services.Interfaces
{
    public interface IPortfolioService : IServiceBase
    {
        IEnumerable<Portfolio> GetAll(int id);

        Portfolio Get(int id);

        Portfolio GetPortfolioByCustomer(int customerId, int id);

        IEnumerable<Portfolio> GetAllPortfoliosByCustomer(int customerId);

        void Add(Portfolio portfolio);

        void Update(Portfolio portfolio);

        void Delete(int id);

        void TransferMoneyToPortfolioOrAccountBalance(CustomerBankInfo customerBankInfo, Portfolio portfolio);
    }
}
