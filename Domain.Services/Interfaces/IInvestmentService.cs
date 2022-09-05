namespace Domain.Services.Interfaces;

public interface IInvestmentService
{
    void DepositMoneyInCustomerBankInfo(int customerBankInfoId, decimal amount);

    void DepositMoneyInPortfolio(int portfolioId, decimal amount);
}