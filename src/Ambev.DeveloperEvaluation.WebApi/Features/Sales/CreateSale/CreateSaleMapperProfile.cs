using Ambev.DeveloperEvaluation.Application.Sales.Base;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleMapperProfile : Profile
{
    public CreateSaleMapperProfile()
    {
        CreateMap<CreateSaleRequest, CreateSaleCommand>();
        CreateMap<BaseSaleResult, SaleResponse>();

        CreateMap<CreateSaleItemRequest, SaleItemDto>();
        CreateMap<SaleItemResult, SaleItemDto>();
    }
}