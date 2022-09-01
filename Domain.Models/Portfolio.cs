using System.Collections.Generic;

namespace Domain.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal TotalBalance { get; set; }

        public ICollection<PortfolioProduct> PortfoliosProducts { get; set; } = new List<PortfolioProduct>();

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public (bool status, string message) ValidateWithdrawMoneyBeforeDelete(decimal totalBalance)
        {
            return totalBalance > 0
                ? (false, "You must withdraw money from the portfolio before deleting it")
                : (true, default);
        }

        public (bool status, string message) ValidateTransaction(decimal cash)
        {
            return (TotalBalance - cash) < 0
                ? (false, "Insufficient balance")
                : (true, default);
        }
    }
}
