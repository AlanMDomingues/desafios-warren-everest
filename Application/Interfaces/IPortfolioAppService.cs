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

        bool AnyPortfolioFromACustomerArentEmpty(int customerId);

        void Add(CreatePortfolioRequest portfolio);

        void Update(int id, UpdatePortfolioRequest portfolio);

        void Delete(int id);

        void TransferMoneyToAccountBalance(int customerBankInfoId, int portfolioId, decimal amount);

        void Invest(CreateOrderRequest orderRequest);
    }
}