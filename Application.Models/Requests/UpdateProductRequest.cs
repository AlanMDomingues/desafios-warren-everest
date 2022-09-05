namespace Application.Models.Requests
{
    public class UpdateProductRequest
    {
        public string Symbol { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
