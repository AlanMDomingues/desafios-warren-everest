using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Maps
{
    public class PortfolioProductMap : IEntityTypeConfiguration<PortfolioProduct>
    {
        public void Configure(EntityTypeBuilder<PortfolioProduct> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PortfolioId)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.ProductId);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.PortfoliosProducts)
                .HasForeignKey(x => x.ProductId);

            builder.HasOne(x => x.Portfolio)
                .WithMany(x => x.PortfoliosProducts)
                .HasForeignKey(x => x.PortfolioId);
        }
    }
}
