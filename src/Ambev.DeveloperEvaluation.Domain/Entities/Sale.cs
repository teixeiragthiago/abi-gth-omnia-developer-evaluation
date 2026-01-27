using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    private readonly List<SaleItem> _items = Enumerable.Empty<SaleItem>().ToList();
    
    public int SaleNumber { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid BranchId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; private set; } = false;
    public DateTime? CancelledAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<SaleItem> Items => _items;

    public Sale(Guid customerId, Guid branchId)
    {
        Id = Guid.NewGuid();
        BranchId = branchId;
        CustomerId = customerId;
    }

    public void AddItem(SaleItem item)
    {
        if (IsCancelled)
            throw new DomainException("Cannot add items to a cancelled sale.");

        EnsureProductQuantityLimitIsNotExceeded(item);
        
        _items.Add(item);
        RecalculateTotal();
    }

    private void EnsureProductQuantityLimitIsNotExceeded(SaleItem newItem)
    {
        var currentQuantity = _items
            .Where(i => i.ProductId == newItem.ProductId)
            .Sum(i => i.Quantity);

        var totalQuantity = currentQuantity + newItem.Quantity;

        if (totalQuantity > 20)
            throw new DomainException(
                $"Cannot add more than 20 identical items for product {newItem.ProductId}."
            );
    }
    
    private void RecalculateTotal()
    {
        TotalAmount = _items.Sum(items => items.TotalAmount);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        IsCancelled = true;
        CancelledAt = DateTime.UtcNow;
    }
}