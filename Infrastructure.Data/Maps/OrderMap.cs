using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Maps
{
    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Quotes)
                .IsRequired();

            builder.Property(x => x.UnitPrice)
                .IsRequired();

            builder.Property(x => x.NetValue)
                .IsRequired();

            builder.Property(x => x.ConvertedAt)
                .IsRequired();
        }
    }
}
