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

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Symbol);

            builder.Property(x => x.UnitPrice);

            builder.HasMany(x => x.Portfolios)
                .WithMany(x => x.Products);

            builder.HasOne(x => x.Order)
                .WithOne(x => x.Product)
                .HasForeignKey<Product>(x => x.OrderId);
        }
    }
}
