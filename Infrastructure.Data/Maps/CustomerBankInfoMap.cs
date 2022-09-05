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

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Account)
                .HasColumnType("char(20)")
                .IsRequired();

            builder.Property(x => x.AccountBalance);
        }
    }
}
