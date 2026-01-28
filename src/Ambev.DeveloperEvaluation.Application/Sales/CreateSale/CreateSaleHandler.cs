using Ambev.DeveloperEvaluation.Application.Options;
using Ambev.DeveloperEvaluation.Application.Sales.Base;
using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services.Policies;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, BaseSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IOptions<SaleProductOptions> _options;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly IDiscountPolicy _discountPolicy;
    private readonly IMediator _mediator;

    public CreateSaleHandler(ISaleRepository saleRepository, IOptions<SaleProductOptions> options, ILogger<CreateSaleHandler> logger, IDiscountPolicy discountPolicy,  IMediator mediator)
    {
        _saleRepository = saleRepository;
        _options = options;
        _logger = logger;
        _discountPolicy = discountPolicy;
        _mediator = mediator;
    }

    public async Task<BaseSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator(_options.Value);
        var resultValidation = await validator.ValidateAsync(command, cancellationToken);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);
        
        _logger.LogInformation("Starting Sale handle creation");

        var sale = MapSaleFromCommandToEntity(command);
        
        var persistedSale = await _saleRepository.InsertAsync(sale, cancellationToken);
        _logger.LogInformation("Finishing Sale handle creation {SaleId}", persistedSale.Id);
        
        await _mediator.Publish(new SaleCreatedNotification(persistedSale.Id, DateTime.UtcNow), cancellationToken);
        
        return persistedSale.ToResult();
    }

    private Sale MapSaleFromCommandToEntity(CreateSaleCommand command)
    {
        var sale = new Sale(command.CustomerId, command.BranchId);
        
        foreach (var prod in command.Products)
        {
            var saleProducts = new SaleProduct(
                prod.ProductId,
                prod.UnitPrice,
                prod.Quantity,
                _discountPolicy.CalculateDiscountPercent(prod.ProductId, prod.Quantity)
            );
            
            sale.AddItem(saleProducts);
        }
        
        return sale;
    }
}