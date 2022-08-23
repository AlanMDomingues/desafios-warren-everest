namespace Domain.Models
{
    public class CustomerBankInfo
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public decimal AccountBalance { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
