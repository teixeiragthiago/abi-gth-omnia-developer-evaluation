
namespace Ambev.DeveloperEvaluation.Domain.Services.Policies;

public interface IDiscountPolicy
{
    decimal CalculateDiscountPercent(Guid productId, int quantity);
}