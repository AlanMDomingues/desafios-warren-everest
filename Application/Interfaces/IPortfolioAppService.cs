using Application.Models.Requests;
using Application.Models.Response;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IPortfolioAppService
    {
        public IEnumerable<PortfolioResult> GetAll(int id);
        public PortfolioResult Get(int id);
        public bool Add(CreatePortfolioRequest portfolio);
        public bool Update(int id, UpdatePortfolioRequest portfolio);
        public bool Delete(int id);
    }
}