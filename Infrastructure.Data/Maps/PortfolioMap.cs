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

            builder.HasKey(x => x.PortfolioId);

            builder.Property(x => x.PortfolioId)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.TotalBalance);
        }
    }
}
