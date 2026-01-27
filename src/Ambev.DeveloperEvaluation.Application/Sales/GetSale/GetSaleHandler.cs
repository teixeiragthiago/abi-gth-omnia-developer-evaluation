using Ambev.DeveloperEvaluation.Application.Sales.Base;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class GetSaleHandler : IRequestHandler<GetSaleCommand, BaseSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private IRequestHandler<GetSaleCommand, BaseSaleResult> _requestHandlerImplementation;

    public GetSaleHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public async Task<BaseSaleResult> Handle(GetSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if(sale is null)
            throw new EntityNotFoundException(nameof(Sale), command.Id);
        
        return new BaseSaleResult
        {
            
        }; //TODO map this
    }
}