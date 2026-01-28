using Ambev.DeveloperEvaluation.Application.Services;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Policies;
public class DiscountPolicyTests
{
    private readonly DiscountPolicy _sut;

    public DiscountPolicyTests()
    {
        _sut = new DiscountPolicy();
    }

    [Theory(DisplayName = "Should return 0% discount when quantity is zero or negative")]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void CalculateDiscountPercent_ShouldReturnZero_WhenQuantityIsZeroOrNegative(int quantity)
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act
        var discount = _sut.CalculateDiscountPercent(productId, quantity);

        // Assert
        discount.Should().Be(0m);
    }

    [Theory(DisplayName = "Should return 20% discount when quantity is 10 or more")]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(100)]
    public void CalculateDiscountPercent_ShouldReturnTwentyPercent_WhenQuantityIsTenOrMore(int quantity)
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act
        var discount = _sut.CalculateDiscountPercent(productId, quantity);

        // Assert
        discount.Should().Be(0.20m);
    }

    [Theory(DisplayName = "Should return 10% discount when quantity is between 4 and 9")]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(9)]
    public void CalculateDiscountPercent_ShouldReturnTenPercent_WhenQuantityIsBetweenFourAndNine(int quantity)
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act
        var discount = _sut.CalculateDiscountPercent(productId, quantity);

        // Assert
        discount.Should().Be(0.10m);
    }

    [Theory(DisplayName = "Should return 0% discount when quantity is between 1 and 3")]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void CalculateDiscountPercent_ShouldReturnZero_WhenQuantityIsBetweenOneAndThree(int quantity)
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act
        var discount = _sut.CalculateDiscountPercent(productId, quantity);

        // Assert
        discount.Should().Be(0m);
    }
}
