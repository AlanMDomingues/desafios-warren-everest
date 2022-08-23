using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Maps
{
    public class PortfolioMap : IEntityTypeConfiguration<Portfolio>
    {
        public void Configure(EntityTypeBuilder<Portfolio> builder)
        {
            builder.ToTable("Portfolio");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.Description);

            builder.Property(x => x.TotalBalance);

            builder.HasMany(x => x.Orders)
                .WithOne(x => x.Portfolio)
                .HasForeignKey(x => x.PortfolioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
