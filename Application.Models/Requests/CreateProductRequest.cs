namespace Application.Models.Requests
{
    public class CreateProductRequest
    {
        public string Symbol { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
