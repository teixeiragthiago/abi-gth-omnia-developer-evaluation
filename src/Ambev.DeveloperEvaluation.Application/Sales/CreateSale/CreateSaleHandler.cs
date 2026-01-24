using Ambev.DeveloperEvaluation.Application.Options;
using Ambev.DeveloperEvaluation.Application.Sales.Base;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, SaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly SaleUnitOptions _options;
    private readonly ILogger<CreateSaleHandler> _logger;

    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, SaleUnitOptions options, ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _options = options;
        _logger = logger;
    }

    public async Task<SaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator(_options);
        var resultValidation = await validator.ValidateAsync(command, cancellationToken);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var saleData = new Sale(command.CustomerId, command.BranchId);
        
        var result = _mapper.Map<SaleResult>(saleData); //TODO criar mapper
        
        return result;
    }

    private Sale MapeSaleFromCommandToEntity(CreateSaleCommand command)
    {
        var saleEntity = new Sale(command.CustomerId, command.BranchId);
        
        foreach (var item in command.Items)
        {
            // var discountStrategy = _discountStrategyFactory.GetDiscountStrategy(item.Quantity);
            // var discountPercentage = discountStrategy.CalculateDiscountPercentage();
            // var strategyName = discountStrategy.GetType().Name;

            var saleItem = new SaleItem(
                item.ProductId,
                item.UnitPrice,
                item.Quantity,
                1 //TODO calculate
            );

            // saleEntity.AddItem(saleItem, strategyName); //TODO
        }
        
        return saleEntity;
    }
}