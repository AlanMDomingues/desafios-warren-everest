using System.Collections.Generic;

namespace Application.Models.Response
{
    public class PortfolioResult
    {
        public string Name { get; set; }
        public decimal TotalBalance { get; set; }
        public IEnumerable<PortfolioProductResult> Products { get; set; }
    }
}
