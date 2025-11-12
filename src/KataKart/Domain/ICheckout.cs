namespace KataKart.Domain;

/// <summary>
/// Defines the contract for managing the scanning of items for purchasing at a checkout.
/// </summary>
public interface ICheckout
{
    /// <summary>
    /// Record an item scan.
    /// </summary>
    /// <param name="itemSKU"></param>
    /// <returns>True if item scan was successfully recorded.</returns>
    bool Scan(string itemSKU);

    /// <summary>
    /// Get the total price of all items scanned.
    /// </summary>
    /// <returns></returns>
    int GetTotalPrice();
}