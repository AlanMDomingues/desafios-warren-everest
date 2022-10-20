using System;

namespace Domain.Models;

public class Order
{
    public Order(int quotes, int portfolioId, int productId)
    {
        Quotes = quotes;
        PortfolioId = portfolioId;
        ProductId = productId;
    }

    public int Id { get; set; }
    public int Quotes { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal NetValue { get; private set; }
    public DateTime ConvertedAt { get; set; } = DateTime.UtcNow;

    public int PortfolioId { get; set; }
    public Portfolio Portfolio { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public void SetNetValue()
    {
        IsValidUnitPriceAndQuotes();
        NetValue = UnitPrice * Quotes;
    }

    public void IsValidUnitPriceAndQuotes()
    {
        if (UnitPrice <= 0 || Quotes <= 0) throw new ArgumentException("UnitPrice and Quotes must be higher than 0");
    }
}
