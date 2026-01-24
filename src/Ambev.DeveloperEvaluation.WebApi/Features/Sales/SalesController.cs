using Ambev.DeveloperEvaluation.Application.Options;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IOptions<SaleUnitOptions> _options;

    public SalesController(
        IMediator mediator,
        IMapper mapper,
        IOptions<SaleUnitOptions> options)
    {
        _mediator = mediator;
        _mapper = mapper;
        _options = options;
    }
    
    [HttpPost]
    // [ProducesResponseType(typeof(ApiResponseWithData<SaleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleRequestValidator(_options.Value);
        var resultValidation = await validator.ValidateAsync(request, cancellationToken);
        if(!resultValidation.IsValid)
            return BadRequest(resultValidation.Errors);
        
        var command = _mapper.Map<CreateSaleCommand>(request); //TODO create Mapper
        var result = await _mediator.Send(command, cancellationToken);
        
        return Accepted(result);
     }
    
    [HttpPost("{id}/cancel")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CancelSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return NoContent();
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        throw  new NotImplementedException();
    }
    
    [HttpPost("{id}/items")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> IncludeSaleItem(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        throw  new NotImplementedException();
    }
}