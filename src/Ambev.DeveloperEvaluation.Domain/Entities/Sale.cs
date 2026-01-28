using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    private readonly List<SaleProduct> _products = Enumerable.Empty<SaleProduct>().ToList();
    
    public int SaleNumber { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid BranchId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; private set; } = false;
    public DateTime? CancelledAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<SaleProduct> Items => _products;

    public Sale(Guid customerId, Guid branchId)
    {
        Id = Guid.NewGuid();
        BranchId = branchId;
        CustomerId = customerId;
        CreatedAt = DateTime.UtcNow; 
    }

    public void AddItem(SaleProduct product)
    {
        if (IsCancelled)
            throw new DomainException("Cannot add items to a cancelled sale.");

        EnsureProductQuantityLimitIsNotExceeded(product);
        
        _products.Add(product);
        RecalculateTotal();
    }

    private void EnsureProductQuantityLimitIsNotExceeded(SaleProduct newProduct)
    {
        var currentQuantity = _products
            .Where(i => i.ProductId == newProduct.ProductId)
            .Sum(i => i.Quantity);

        var totalQuantity = currentQuantity + newProduct.Quantity;

        if (totalQuantity > 20)
            throw new DomainException(
                $"Cannot add more than 20 identical items for product {newProduct.ProductId}."
            );
    }
    
    private void RecalculateTotal()
    {
        TotalAmount = _products.Sum(items => items.TotalAmount);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        IsCancelled = true;
        CancelledAt = DateTime.UtcNow;
    }
}