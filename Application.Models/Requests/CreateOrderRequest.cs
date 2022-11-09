namespace Application.Models.Requests
{
    public class CreateOrderRequest
    {
        public int Quotes { get; set; }
        public int PortfolioId { get; set; }
        public int ProductId { get; set; }
        public int CustomerBankInfoId { get; set; }
    }
}
