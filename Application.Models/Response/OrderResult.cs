using System;

namespace Application.Models.Response
{
    public class OrderResult
    {
        public int Quotes { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal NetValue { get; set; }
        public DateTime ConvertedAt { get; set; }
        public int PortfolioId { get; set; }
        public int ProductId { get; set; }
    }
}
