using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable(nameof(Sale));

        builder.HasKey(x => x.Id);
        builder.Property(u => u.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.SaleNumber)
            .UseIdentityAlwaysColumn()
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("timezone('utc', now())")
            .ValueGeneratedOnAdd();
        
        builder.Property(s => s.UpdatedAt);
        builder.Property(s => s.CancelledAt);

        builder.Property(s => s.CustomerId)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(s => s.BranchId)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(s => s.TotalAmount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(s => s.IsCancelled)
            .IsRequired();

        builder.HasMany(s => s.Items)
            .WithOne()
            .HasForeignKey("SaleId")  
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(s => s.Items)
            .HasField("_items")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        
    }
}