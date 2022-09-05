using Domain.Models;
using System.Collections.Generic;

namespace Domain.Services.Interfaces
{
    public interface IPortfolioService : IServiceBase
    {
        IEnumerable<Portfolio> GetAll(int id);

        Portfolio Get(int id);

        bool AnyPortfolioFromACustomerArentEmpty(int customerId);

        bool AnyPortfolioForId(int id);

        void Add(Portfolio portfolio);

        void Update(Portfolio portfolio);

        void Delete(int id);

        void Deposit(int id, decimal amount);

        void Withdraw(int id, decimal amount);
    }
}
