namespace Application.Models.Requests
{
    public class CreatePortfolioRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CustomerId { get; set; }
    }
}
