using Domain.Models;
using System.Collections.Generic;

namespace Domain.Services.Interfaces
{
    public interface IPortfolioService : IServiceBase
    {
        public IEnumerable<Portfolio> GetAll(int id);

        public Portfolio Get(int id);

        public Portfolio GetPortfolioByCustomer(int customerId, int id);

        public IEnumerable<Portfolio> GetAllPortfoliosByCustomer(int customerId);

        public void Add(Portfolio portfolio);

        public void Update(Portfolio portfolio);

        public void Delete(int id);

        public void TransferMoneyToPortfolioOrAccountBalance(CustomerBankInfo customerBankInfo, Portfolio portfolio);
    }
}
