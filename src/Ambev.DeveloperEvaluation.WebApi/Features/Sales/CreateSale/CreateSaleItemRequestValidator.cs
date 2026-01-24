using Ambev.DeveloperEvaluation.Application.Options;
using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleItemRequestValidator : AbstractValidator<CreateSaleItemRequest>
{
    public CreateSaleItemRequestValidator(SaleUnitOptions options)
    {
        RuleFor(saleItem => saleItem.ProductId)
            .NotEmpty();

        RuleFor(saleItem => saleItem.Quantity)
            .GreaterThan(options.UnitQuantity.Min)
            .WithMessage(ValidationMessages.Min("Quantity", options.UnitQuantity.Min))
            .LessThanOrEqualTo(options.UnitQuantity.Max)
            .WithMessage(ValidationMessages.Max("Quantity", options.UnitQuantity.Max));

        RuleFor(saleItem => saleItem.UnitPrice)
            .GreaterThan(options.UnitPrice.Min).WithMessage(ValidationMessages.Min("UnitPrice", options.UnitPrice.Min))
            .LessThanOrEqualTo(options.UnitPrice.Max).WithMessage(ValidationMessages.Max("UnitPrice", options.UnitPrice.Max));
    }
}