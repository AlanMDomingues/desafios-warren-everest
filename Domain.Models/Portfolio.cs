using System.Collections.Generic;

namespace Domain.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal TotalBalance { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
