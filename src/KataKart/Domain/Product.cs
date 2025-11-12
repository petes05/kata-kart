namespace KataKart.Domain;

/// <summary>
/// Represents a product that can be scanned at checkout.
/// </summary>
public class Product
{
    /// <summary>
    /// Gets the Stock Keeping Unit (unique).
    /// </summary>
    public required string SKU { get; init; }

    /// <summary>
    /// Gets the price of one unit of this product.
    /// </summary>
    public required double UnitPrice { get; init; }

    /// <summary>
    /// Gets the multibuy discount if it exists.
    /// </summary>
    public Multibuy? Multibuy { get; init; } = null;
}