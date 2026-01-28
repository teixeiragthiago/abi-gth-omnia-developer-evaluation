using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Notifications;

public record SaleCreatedNotification(Guid SaleId, DateTime CreatedAt) : INotification;