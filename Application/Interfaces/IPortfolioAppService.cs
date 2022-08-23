using Application.Models.Requests;
using Application.Models.Response;
using Domain.Models;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IPortfolioAppService : IAppServicesBase
    {
        public IEnumerable<PortfolioResult> GetAll(int id);

        public PortfolioResult Get(int id);

        public Portfolio GetWithoutMap(int id);

        public Portfolio GetPortfolioByCustomer(int customerId, int id);

        public IEnumerable<Portfolio> GetAllPortfoliosByCustomer(int customerId);

        public (bool status, string message) Add(CreatePortfolioRequest portfolio);

        public (bool status, string message) Update(int id, UpdatePortfolioRequest portfolio);

        public (bool status, string message) Delete(int id);

        public (bool status, string message) TransferMoneyToPortfolio(int customerBankInfoId, int portfolioId, decimal cash);

        public (bool status, string message) TransferMoneyToAccountBalance(int customerBankInfoId, int portfolioId, decimal cash);
    }
}