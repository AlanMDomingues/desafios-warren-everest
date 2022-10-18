using System.Collections.Generic;

namespace Domain.Models
{
    public class Portfolio
    {
        public Portfolio(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal TotalBalance { get; set; }

        public ICollection<PortfolioProduct> PortfoliosProducts { get; set; } = new List<PortfolioProduct>();

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public bool ValidateTransaction(decimal cash)
        {
            return (TotalBalance - cash) >= 0;
        }
    }
}
