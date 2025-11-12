using KataKart.Domain;

namespace KataKart.Application;

/// <summary>
/// Used for managing the scanning of items for purchasing at a checkout.
/// </summary>
public class Checkout : ICheckout
{
    private readonly Dictionary<string, Product> _products;

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
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public double GetTotalPrice()
    {
        throw new NotImplementedException();
    }
}