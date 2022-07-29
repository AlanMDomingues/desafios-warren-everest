using System;

namespace Domain.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Symbol { get; set; } // nome do ativo
        public int Quotes { get; set; } // quantidade de cotas
        public decimal UnitPrice { get; set; } // preço de cada cota de um ativo
        public decimal NetValue { get; set; } // valor liquido total multiplicando Quotes pelo UnitPrice
        public DateTime ConvertedAt { get; set; } // data da compra
        public Portfolio Portfolio { get; set; }
    }
}
