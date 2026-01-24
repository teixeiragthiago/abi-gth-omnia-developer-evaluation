namespace Ambev.DeveloperEvaluation.Application.Options;

public record SaleUnitOptions
{
    public UnitPrice UnitPrice { get; set; }
    public UnitQuantity UnitQuantity { get; set; }
}

public record UnitPrice
{
    public decimal Max { get; set; }
    public decimal Min { get; set; }
}

public record UnitQuantity
{
    public int Max { get; set; }
    public int Min { get; set; }
}