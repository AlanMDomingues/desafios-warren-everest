using System.Collections.Generic;

namespace Domain.Models
{
    public class Portfolio
    {
        public int PortfolioId { get; set; }
        public decimal TotalBalance { get; set; } // patrimônio da carteira somando todos os ativos
        public virtual ICollection<Product> Products { get; set; } // lista de produtos comprados
    }
}
