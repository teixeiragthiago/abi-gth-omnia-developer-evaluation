using Ambev.DeveloperEvaluation.Domain;
using Ambev.DeveloperEvaluation.Domain.Services.Policies;

namespace Ambev.DeveloperEvaluation.Application.Services;

public class DiscountPolicy : IDiscountPolicy
{
    public decimal CalculateDiscountPercent(Guid productId, int quantity)
    {
        if (quantity <= 0)
            return 0m;

        if (quantity >= 10)
            return 0.20m;

        if (quantity >= 4)
            return 0.10m;

        return 0m;
    }
}