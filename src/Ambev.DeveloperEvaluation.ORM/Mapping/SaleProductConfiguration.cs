using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleProductConfiguration
{
    public void Configure(EntityTypeBuilder<SaleProduct> builder)
    {
        builder.ToTable(nameof(SaleProduct));

        builder.HasKey(i => i.Id);

        builder.Property(u => u.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(i => i.SaleId)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(i => i.ProductId)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(i => i.Quantity)
            .IsRequired();
        builder.Property(i => i.UnitPrice)
            .HasColumnType("numeric(18,2)")
            .IsRequired();
        builder.Property(i => i.DiscountPercentage)
            .HasColumnType("numeric(5,4)")
            .IsRequired();
        builder.Property(i => i.DiscountAmount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();
        builder.Property(i => i.TotalAmount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("timezone('utc', now())")
            .ValueGeneratedOnAdd();
        
        builder.Property(s => s.UpdatedAt);

        builder.HasOne<Sale>()
            .WithMany(s => s.Items)
            .HasForeignKey(i => i.SaleId);
    }
}