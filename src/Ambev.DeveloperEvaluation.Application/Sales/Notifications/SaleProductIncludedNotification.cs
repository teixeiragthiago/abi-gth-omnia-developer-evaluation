using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Notifications;

public record SaleProductIncludedNotification(Guid SaleId, Guid ProductId, int Quantity, decimal UnitPrice) :  INotification;