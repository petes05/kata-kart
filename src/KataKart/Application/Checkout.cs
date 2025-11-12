using KataKart.Domain;

namespace KataKart.Application;

/// <summary>
/// Used for managing the scanning of items for purchasing at a checkout.
/// </summary>
public class Checkout : ICheckout
{
    private readonly Dictionary<string, Product> _products;
    private readonly Dictionary<string, int> _scannedProductCounts = [];

    /// <summary>
    /// Initialises new instance of Checkout.
    /// </summary>
    /// <param name="products">List of current products available for purchase.</param>
    public Checkout(Dictionary<string, Product> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        _products = products;
    }

    /// <inheritdoc/>
    public bool Scan(string itemSKU)
    {
        if (string.IsNullOrEmpty(itemSKU))
        {
            // This should be logged and an error result returned to client. But just returning false for now.
            return false;
        }

        if (!_products.ContainsKey(itemSKU))
        {
            // This should be logged and an error result returned to client. But just returning false for now.
            return false;
        }

        var itemExists = _scannedProductCounts.TryGetValue(itemSKU, out int count);
        _scannedProductCounts[itemSKU] = itemExists ? count + 1 : 1;

        return true;
    }

    /// <inheritdoc/>
    public int GetTotalPrice()
    {
        var totalPrice = 0;

        foreach (var scannedProductCount in _scannedProductCounts)
        {
            totalPrice += GetTotalPriceForProduct(scannedProductCount.Key, scannedProductCount.Value);
        }

        return totalPrice;
    }

    private int GetTotalPriceForProduct(string productSKU, int count)
    {
        if (!_products.TryGetValue(productSKU, out var product))
        {
            throw new InvalidOperationException("Scanned product does not exist, this should be impossible!");
        }

        if (product.Multibuy != null && product.Multibuy.Quantity > 0)
        {
            int numberOfMultibuys = Math.DivRem(count, product.Multibuy.Quantity, out int remainingCount);
            return product.Multibuy.Price * numberOfMultibuys + product.UnitPrice * remainingCount;
        }
        else
        {
            return count * product.UnitPrice;
        }
    }
}