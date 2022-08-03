using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Maps
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(x => x.ProductId);

            builder.Property(x => x.ProductId);

            builder.Property(x => x.Symbol);

            builder.Property(x => x.Quotes);

            builder.Property(x => x.UnitPrice);

            builder.Property(x => x.NetValue);

            builder.Property(x => x.ConvertedAt);

            builder.HasOne(x => x.Portfolio)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.PortfolioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
