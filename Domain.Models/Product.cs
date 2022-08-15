using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Symbol { get; set; } // nome do ativo
        public decimal UnitPrice { get; set; } // preço de cada cota de um ativo

        public virtual ICollection<Portfolio> Portfolios { get; set; } = new List<Portfolio>();

        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
