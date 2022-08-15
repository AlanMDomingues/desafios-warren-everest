namespace Application.Models.Requests
{
    public class CreateProductRequest
    {
        public string Symbol { get; set; } // nome do ativo
        public decimal UnitPrice { get; set; } // preço de cada cota de um ativo
    }
}
