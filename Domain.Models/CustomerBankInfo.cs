namespace Domain.Models
{
    public class CustomerBankInfo
    {
        public int Id { get; set; }
        public string Account { get; set; } // código da conta
        public decimal AccountBalance { get; set; } // saldo da conta

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
