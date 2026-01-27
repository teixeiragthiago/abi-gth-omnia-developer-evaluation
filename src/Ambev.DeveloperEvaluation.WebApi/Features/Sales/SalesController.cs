using Ambev.DeveloperEvaluation.Application.Options;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.IncludeSaleItem;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.IncludeSaleItem;
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
        
        return Created(string.Empty, new
        {
            Data = result
        });
     }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetSaleRequest(id);
        var validator = new GetSaleRequestValidator();
        var resultValidation = await validator.ValidateAsync(request, cancellationToken);
        if(!resultValidation.IsValid)
            return BadRequest(resultValidation.Errors);

        var command = new GetSaleCommand{Id = id};
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new
        {
            Data = response
        });
    }
    
    [HttpPost("{id}/cancel")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CancelSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new CancelSaleRequest(id);
        var validator = new CancelSaleRequestValidator();
        var resultValidation = await validator.ValidateAsync(request, cancellationToken);
        if(!resultValidation.IsValid)
            return BadRequest(resultValidation.Errors);

        var command = new CancelSaleCommand{ Id = id};
        await _mediator.Send(command, cancellationToken);
        
        return NoContent();
    }
    
    [HttpPost("{id}/include-items")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> IncludeSaleItem(
        [FromRoute] Guid id,
        [FromBody]IncludeSaleItemRequest request,
        CancellationToken cancellationToken)
    {
        var validator = new IncludeSaleItemRequestValidator(_options.Value);
        var resultValidation = await validator.ValidateAsync(request, cancellationToken);
        if(!resultValidation.IsValid)
            return BadRequest(resultValidation.Errors);

        var command = new IncludeSaleItemCommand
        {
            ProductId = request.ProductId,
            UnitPrice =  request.UnitPrice,
            Quantity = request.Quantity
        };
        
        await _mediator.Send(command, cancellationToken);

        return Ok();
    }
}