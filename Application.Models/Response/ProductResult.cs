using System;

namespace Application.Models.Response
{
    public class ProductResult
    {
        public string Symbol { get; set; } // nome do ativo
        public decimal UnitPrice { get; set; } // preço de cada cota de um ativo
    }
}
