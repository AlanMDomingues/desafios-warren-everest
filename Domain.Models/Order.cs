using System;

namespace Domain.Models;

public class Order
{
    public Order(int quotes, int portfolioId, decimal unitPrice)
    {
        Quotes = quotes;
        PortfolioId = portfolioId;
        SetNetValue(unitPrice);
    }

    public int Id { get; set; }
    public int Quotes { get; private set; } // quantidade de cotas
    public decimal NetValue { get; private set; } // valor liquido total multiplicando Quotes pelo UnitPrice
    public DateTime ConvertedAt { get; set; } = DateTime.UtcNow; // data da compra

    public int PortfolioId { get; set; }
    public virtual Portfolio Portfolio { get; set; }

    public virtual Product Product { get; set; }

    public void SetQuotes(int quotes)
    {
        Quotes = quotes;
        SetNetValue();
    }

    //private void SetNetValue(decimal? unitPrice = null)
    //{
    //    unitPrice ??= NetValue / Quotes;
    //    NetValue = unitPrice * Quotes;
    //}

    private void SetNetValue()
    {
        var unitPrice = NetValue / Quotes;
        NetValue = unitPrice * Quotes;
    }

    private void SetNetValue(decimal unitPrice)
    {
        NetValue = unitPrice * Quotes;
    }
}
