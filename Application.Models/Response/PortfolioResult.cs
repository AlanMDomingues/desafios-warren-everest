using Domain.Models;
using System.Collections.Generic;

namespace Application.Models.Response
{
    public class PortfolioResult
    {
        public string Name { get; set; }
        public decimal TotalBalance { get; set; } // patrimônio da carteira somando todos os ativos
        public virtual ICollection<Product> Products { get; set; } // lista de produtos comprados
    }
}
