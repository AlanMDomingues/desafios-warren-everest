using System;

namespace Application.Models.Response
{
    public class OrderResult
    {
        public int Quotes { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal NetValue { get; set; }
        public DateTime ConvertedAt { get; set; }
    }
}
