using System.Runtime.InteropServices.JavaScript;
using Ambev.DeveloperEvaluation.Application.Sales.Base;
using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, BaseSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelSaleHandler> _logger;
    private readonly IMediator _mediator;

    public CancelSaleHandler(ISaleRepository saleRepository, ILogger<CancelSaleHandler> logger, IMapper mapper, IMediator mediator)
    {
        _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    
    public async Task<BaseSaleResult> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
            _logger.LogError("command is null");
        
        _logger.LogInformation("Starting cancelling sale: {SaleId}", command.Id);
        
        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if(sale is null)
            throw new EntityNotFoundException(nameof(Sale), command.Id);
           
        sale.Cancel();
            
        var cancelledSale = await _saleRepository.UpdateAsync(sale, cancellationToken);
        
        _logger.LogInformation("Sale cancelled with success: {SaleId}", command.Id);
        
        await _mediator.Publish(new SaleCancelledNotification(cancelledSale.Id, DateTime.UtcNow), cancellationToken);
        
        return sale.ToResult();
    }
}