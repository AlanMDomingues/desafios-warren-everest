using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using System;

namespace Domain.Services.Services
{
    public class InvestmentService : IInvestmentService
    {
        private readonly IRepositoryFactory _repository;

        public InvestmentService(IRepositoryFactory repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void DepositMoneyInPortfolio(int portfolioId, decimal amount)
        {
            var service = _repository.CustomRepository<IPortfolioService>();
            service.Deposit(portfolioId, amount);
        }

        public void DepositMoneyInCustomerBankInfo(int customerBankInfoId, decimal amount)
        {
            var service = _repository.CustomRepository<ICustomerBankInfoService>();
            service.Deposit(customerBankInfoId, amount);
        }
    }
}