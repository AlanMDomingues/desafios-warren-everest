namespace Domain.Models
{
    public class PortfolioProduct
    {
        public PortfolioProduct(int portfolioId, int productId)
        {
            PortfolioId = portfolioId;
            ProductId = productId;
        }

        public int Id { get; set; }
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
