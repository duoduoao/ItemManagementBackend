using ItemManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItemManagement.Infrastructure.Data.Configurations
{

    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Items");

            builder.HasKey(i => i.ItemId);

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(i => i.Quantity)
                .HasDefaultValue(0);

            builder.Property(i => i.Price)
                .HasColumnType("decimal(18,2)");

            // If you have properties not mapped, add:
            // builder.Ignore(i => i.Image);
        }
    }
}