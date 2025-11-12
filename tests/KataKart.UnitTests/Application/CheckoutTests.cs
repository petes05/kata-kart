using KataKart.Application;
using KataKart.Domain;
using Xunit.Internal;

namespace KataKart.UnitTests.Application;

public class CheckoutTests
{
    private readonly Checkout _checkout;

    public CheckoutTests()
    {
        var products = new Dictionary<string, Product>
        {
            { "A", new Product{ SKU = "A", UnitPrice = 50, Multibuy = new Multibuy(3, 130) } },
            { "B", new Product{ SKU = "B", UnitPrice = 30, Multibuy = new Multibuy(2, 45) } },
            { "C", new Product{ SKU = "C", UnitPrice = 20 } },
            { "D", new Product{ SKU = "D", UnitPrice = 15 } },
            { "E", new Product{ SKU = "E", UnitPrice = 25, Multibuy = new Multibuy(0, 10) } },
            { "F", new Product{ SKU = "F", UnitPrice = 5, Multibuy = new Multibuy(3, 20) } }
        };

        _checkout = new Checkout(products);
    }

    [Fact]
    public void Scan_ValidSKU_ReturnTrue()
    {
        // Arrange
        var sku = "A";

        // Act
        var result = _checkout.Scan(sku);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Scan_ValidSKU_TotalPriceUpdated()
    {
        // Arrange
        var sku = "A";

        // Act
        _checkout.Scan(sku);

        // Assert
        Assert.Equal(50, _checkout.GetTotalPrice());
    }

    [Fact]
    public void Scan_InvalidSKU_ReturnFalse()
    {
        // Arrange
        var sku = "Z";

        // Act
        var result = _checkout.Scan(sku);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Scan_InvalidSKU_TotalPriceNotUpdated()
    {
        // Arrange
        var sku = "Z";

        // Act
        _checkout.Scan(sku);

        // Assert
        Assert.Equal(0, _checkout.GetTotalPrice());
    }

    [Fact]
    public void Scan_NullSKU_ReturnFalse()
    {
        // Arrange

        // Act
        var result = _checkout.Scan(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Scan_NullSKU_TotalPriceNotUpdated()
    {
        // Arrange

        // Act
        _checkout.Scan(null);

        // Assert
        Assert.Equal(0, _checkout.GetTotalPrice());
    }

    [Fact]
    public void GetTotalPrice_NoScans_Returns0()
    {
        // Arrange

        // Act
        var result = _checkout.GetTotalPrice();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetTotalPrice_MultipleSameProductWithNoMultibuyScanned_ReturnsCorrectTotal()
    {
        // Arrange
        _checkout.Scan("C");
        _checkout.Scan("C");
        _checkout.Scan("C");
        _checkout.Scan("C");
        _checkout.Scan("C");

        // Act
        var result = _checkout.GetTotalPrice();

        // Assert
        Assert.Equal(100, result);
    }

    [Fact]
    public void GetTotalPrice_MultipleDifferentProductsScannedOutOfOrder_ReturnsCorrectTotal()
    {
        // Arrange
        _checkout.Scan("C");
        _checkout.Scan("A");
        _checkout.Scan("C");
        _checkout.Scan("D");

        // Act
        var result = _checkout.GetTotalPrice();

        // Assert
        Assert.Equal(105, result);
    }

    [Theory]
    [InlineData(new string[] { "A", "A", "A" }, 130)]
    [InlineData(new string[] { "A", "A", "A", "A" }, 180)]
    [InlineData(new string[] { "A", "B", "A", "A", "B", "A", "B", "B" }, 270)]
    [InlineData(new string[] { "A", "B", "A", "C", "A", "B", "D", "A", "B", "B" }, 305)]
    public void GetTotalPrice_MultipleProductsWithMultibuyScanned_ReturnsCorrectTotal(string[] skusToScan, double expectedResult)
    {
        // Arrange
        skusToScan.ForEach(sku => _checkout.Scan(sku));

        // Act
        var result = _checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetTotalPrice_MultibuyQuantityIsZero_MultibuyIgnored()
    {
        // Arrange
        _checkout.Scan("E");
        _checkout.Scan("E");
        _checkout.Scan("E");

        // Act
        var result = _checkout.GetTotalPrice();

        // Assert
        Assert.Equal(75, result);
    }

    [Fact]
    public void GetTotalPrice_MultibuyOverPriced_ReturnsLowestPrice()
    {
        // Arrange
        _checkout.Scan("F");
        _checkout.Scan("F");
        _checkout.Scan("F");

        // Act
        var result = _checkout.GetTotalPrice();

        // Assert
        Assert.Equal(15, result);
    }
}
