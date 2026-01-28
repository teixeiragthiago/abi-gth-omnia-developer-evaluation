using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.IncludeSaleProduct;

public class PublishSaleProductIncludedHandler : INotificationHandler<SaleProductIncludedNotification>
{
    public Task Handle(SaleProductIncludedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Product {notification.ProductId} has been included to Sale {notification.SaleId} with quantity {notification.Quantity} and unitPrice {notification.UnitPrice}");
        return Task.CompletedTask;
    }
}