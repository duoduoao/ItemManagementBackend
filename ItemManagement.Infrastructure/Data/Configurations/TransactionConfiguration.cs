using ItemManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItemManagement.Infrastructure.Data.Configurations
{

    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(t => t.TransactionId);

            builder.Property(t => t.TimeStamp)
                .IsRequired();

            builder.Property(t => t.Price)
                .HasColumnType("decimal(18,2)");
        }
    }

}