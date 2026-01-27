using Ambev.DeveloperEvaluation.Application.Options;
using Ambev.DeveloperEvaluation.Application.Sales.Base;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, BaseSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IOptions<SaleUnitOptions> _options;
    private readonly ILogger<CreateSaleHandler> _logger;

    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IOptions<SaleUnitOptions> options, ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _options = options;
        _logger = logger;
    }

    public async Task<BaseSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator(_options.Value);
        var resultValidation = await validator.ValidateAsync(command, cancellationToken);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);
        
        _logger.LogInformation("Starting handle of new sale creation");

        var saleEntity = new Sale(command.CustomerId, command.BranchId);
        var sale = await _saleRepository.InsertAsync(saleEntity, cancellationToken);
        
        var result = _mapper.Map<BaseSaleResult>(sale); //TODO criar mapper
        
        _logger.LogInformation("Finishing handle of new sale creation");
        
        return result;
    }

    private Sale MapeSaleFromCommandToEntity(CreateSaleCommand command)
    {
        var saleEntity = new Sale(command.CustomerId, command.BranchId);
        
        foreach (var item in command.Items)
        {
            var saleItem = new SaleItem(
                item.ProductId,
                item.UnitPrice,
                item.Quantity,
                1 //TODO calculate
            );
        }
        
        return saleEntity;
    }
}