using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Notifications;

public record SaleCancelledNotification(Guid SaleId, DateTime CancelledAt) : INotification;