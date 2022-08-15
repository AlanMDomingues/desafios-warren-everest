using Domain.Models;
using System.Collections.Generic;

namespace Domain.Services.Interfaces
{
    public interface IPortfolioService : IServiceBase
    {
        public IEnumerable<Portfolio> GetAll(int id);
        public Portfolio Get(int id);
        public bool Add(Portfolio portfolio);
        public bool Update(Portfolio portfolio);
        public bool Delete(int id);
    }
}
