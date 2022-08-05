using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Maps
{
    public class CustomerBankInfoMap : IEntityTypeConfiguration<CustomerBankInfo>
    {
        public void Configure(EntityTypeBuilder<CustomerBankInfo> builder)
        {
            builder.ToTable("CustomerBankInfo");

            builder.HasKey(x => x.CustomerId);

            builder.Property(x => x.Account);

            builder.Property(x => x.AccountBalance);

            builder.HasOne(x => x.Customer)
                .WithOne(x => x.CustomerBankInfo)
                .HasForeignKey<CustomerBankInfo>(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
