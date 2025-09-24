using ItemManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItemManagement.Infrastructure.Data.Configurations
{

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.CategoryId);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .HasMaxLength(255);

            builder.HasMany(c => c.Items)
                .WithOne(i => i.Category)
                .HasForeignKey(i => i.CategoryId);
        }
    }
}