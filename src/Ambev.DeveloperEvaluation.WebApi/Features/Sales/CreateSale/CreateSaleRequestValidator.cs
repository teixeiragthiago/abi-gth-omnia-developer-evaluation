using Ambev.DeveloperEvaluation.Application.Options;
using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    public CreateSaleRequestValidator(SaleProductOptions options)
    {
        RuleFor(x => x.Products)
            .NotEmpty()
            .WithMessage("You must specify at least one item.")
            .ForEach(item => item.SetValidator(new CreateSaleProductRequestValidator(options)));

        RuleFor(x => x.Products)
            .Must(items => ValidateQuantityProductLimit(items, options.UnitQuantity.Max))
            .WithMessage(ValidationMessages.Max("Items total quantity", options.UnitQuantity.Max))
            .When(x => x?.Products != null && x.Products.Any());        
        
        RuleFor(x => x.BranchId)
            .NotEmpty();
        
        RuleFor(x => x.CustomerId)
            .NotEmpty();
    }
    
    private static bool ValidateQuantityProductLimit(
        IEnumerable<CreateSaleProductRequest> items,
        int maxQuantityPerProduct)
    {
        if (items == null || !items.Any())
            return true;

        var totalQuantityPerProduct = AggroupTotalQuantityPerProduct(items);

        return totalQuantityPerProduct.All(
            total => total <= maxQuantityPerProduct
        );
    }

    private static IEnumerable<int> AggroupTotalQuantityPerProduct(
        IEnumerable<CreateSaleProductRequest> items)
    {
        return items
            .GroupBy(item => item.ProductId)
            .Select(group => group.Sum(item => item.Quantity));
    }
}