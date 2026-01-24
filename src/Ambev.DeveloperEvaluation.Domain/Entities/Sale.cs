using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    private readonly List<SaleItem> _items = Enumerable.Empty<SaleItem>().ToList();
    
    public int SaleNumber { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid BranchId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<SaleItem> Items => _items;


    public void AddItem(SaleItem item)
    {
        if (IsCancelled)
            throw new DomainException("Cannot add items to a cancelled sale.");

        _items.Add(item);
        RecalculateTotal();
    }
    
    private void RecalculateTotal()
    {
        TotalAmount = _items.Sum(i => i.TotalAmount);
        UpdatedAt = DateTime.UtcNow;
    }

}