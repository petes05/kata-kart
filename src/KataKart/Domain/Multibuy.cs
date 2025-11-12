namespace KataKart.Domain;

/// <summary>
/// Reprents a multibuy special offer for a product.
/// </summary>
/// <param name="Quantity">The quantity of items needed to qualify for the special offer.</param>
/// <param name="Price">The total price for the multibuy (in the smallest unit of currency e.g. pence).</param>
public record Multibuy(int Quantity, int Price);