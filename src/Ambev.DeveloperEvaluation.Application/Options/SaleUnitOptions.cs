namespace Ambev.DeveloperEvaluation.Application.Options;

public record SaleUnitOptions(UnitPrice UnitPrice, Quantity UnitQuantity); 

public record UnitPrice(decimal Max,  decimal Min);

public record Quantity(int Max,  decimal Int);
