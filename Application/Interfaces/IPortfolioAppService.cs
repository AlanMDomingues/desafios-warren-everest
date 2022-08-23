using Application.Models.Requests;
using Application.Models.Response;
using Domain.Models;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IPortfolioAppService : IAppServicesBase
    {
        IEnumerable<PortfolioResult> GetAll(int id);

        PortfolioResult Get(int id);

        Portfolio GetWithoutMap(int id);

        Portfolio GetPortfolioByCustomer(int customerId, int id);

        IEnumerable<Portfolio> GetAllPortfoliosByCustomer(int customerId);

        void Add(CreatePortfolioRequest portfolio);

        void Update(int id, UpdatePortfolioRequest portfolio);

        (bool status, string message) Delete(int id);

        (bool status, string message) TransferMoneyToPortfolio(int customerBankInfoId, int portfolioId, decimal cash);

        (bool status, string message) TransferMoneyToAccountBalance(int customerBankInfoId, int portfolioId, decimal cash);
    }
}