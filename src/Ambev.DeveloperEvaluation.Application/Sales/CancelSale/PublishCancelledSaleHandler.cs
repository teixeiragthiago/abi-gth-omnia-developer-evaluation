using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class PublishCancelledSaleHandler : INotificationHandler<SaleCancelledNotification>
{
    public Task Handle(SaleCancelledNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Sale {notification.SaleId} has been cancelled at {notification.CancelledAt}");
        return Task.CompletedTask;
    }
}