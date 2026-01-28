using Ambev.DeveloperEvaluation.Application.Sales.Base;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services.Policies;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.IncludeSaleItem;

public class IncludeSaleProductHandler : IRequestHandler<IncludeSaleProductCommand, BaseSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<IncludeSaleProductHandler> _logger;
    private readonly IDiscountPolicy _discountPolicy;
    private readonly IMediator _mediator;
    
    public IncludeSaleProductHandler(
        ISaleRepository saleRepository,
        ILogger<IncludeSaleProductHandler> logger,
        IDiscountPolicy discountPolicy,
        IMediator mediator)
    {
        _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _discountPolicy = discountPolicy ?? throw new ArgumentNullException(nameof(discountPolicy));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }


    public async Task<BaseSaleResult> Handle(IncludeSaleProductCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Including item {ProductId} to Sale {SaleId}", command.ProductId, command.SaleId);
        var saleData = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if (saleData is null)
            throw new EntityNotFoundException(nameof(Sale), command.SaleId);

        if (saleData.IsCancelled)
            throw new DomainException("Cannot include items to a cancelled sale");

        decimal discountPercent = _discountPolicy.CalculateDiscountPercent(command.ProductId, command.Quantity);
        var saleItem = new SaleProduct(command.ProductId, command.UnitPrice, command.Quantity, discountPercent);
        saleData.AddItem(saleItem);

        var updatedSale = await _saleRepository.UpdateAsync(saleData, cancellationToken);

        _logger.LogInformation("Item {ProductId} included to sale {SaleId} wit success!", command.ProductId,
            command.SaleId);
        
        await _mediator.Publish(new SaleProductIncludedNotification(command.SaleId, command.ProductId, command.Quantity, command.UnitPrice), cancellationToken);

        return updatedSale.ToResult();
    }
}