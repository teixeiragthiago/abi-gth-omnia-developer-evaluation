using Ambev.DeveloperEvaluation.Application.Sales.Base;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services.Policies;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.IncludeSaleItem;

public class IncludeSaleItemHandler : IRequestHandler<IncludeSaleItemCommand, BaseSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<IncludeSaleItemHandler> _logger;
    private readonly IDiscountPolicy _discountPolicy;
    
    public IncludeSaleItemHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<IncludeSaleItemHandler> logger, IDiscountPolicy discountPolicy)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
        _discountPolicy = discountPolicy;
    }

    public async Task<BaseSaleResult> Handle(IncludeSaleItemCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Including item {ProductId} to Sale {SaleId}", command.ProductId, command.SaleId);
        var saleData = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if(saleData is null)
            throw new EntityNotFoundException(nameof(Sale), command.SaleId);
        
        if(saleData.IsCancelled)
            throw new DomainException("Cannot include items to a cancelled sale");
        
        decimal discountPercent = _discountPolicy.CalculateDiscountPercent(command.ProductId, command.Quantity);
        var saleItem  = new SaleItem(command.ProductId, command.UnitPrice, command.Quantity, discountPercent);
        saleData.AddItem(saleItem);
        
        var updatedSale = await _saleRepository.UpdateAsync(saleData, cancellationToken);

        _logger.LogInformation("Item {ProductId} included to sale {SaleId} wit success!", command.ProductId, command.SaleId);

        return new BaseSaleResult()
        {
            Id = command.SaleId,
        };
    }
}