using System;

namespace Application.Models.Response
{
    public class OrderResult
    {
        public int Quotes { get; set; } // quantidade de cotas
        public decimal NetValue { get; set; } // valor liquido total multiplicando Quotes pelo UnitPrice
        public DateTime ConvertedAt { get; set; } // data da compra
    }
}
