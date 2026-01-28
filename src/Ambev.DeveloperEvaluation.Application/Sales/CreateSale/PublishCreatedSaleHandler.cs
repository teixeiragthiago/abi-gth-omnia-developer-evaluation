using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class PublishCreatedSaleHandler : INotificationHandler<SaleCreatedNotification>
{
    public Task Handle(SaleCreatedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Sale {notification.SaleId} has been created at {notification.CreatedAt}");
        return Task.CompletedTask;
    }
}