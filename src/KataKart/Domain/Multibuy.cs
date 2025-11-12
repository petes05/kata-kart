namespace KataKart.Domain;

/// <summary>
/// Reprents a multibuy special offer for a product.
/// </summary>
/// <param name="Quantity">The quantity of items needed to qualify for the special offer.</param>
/// <param name="Price">The total price for the multibuy.</param>
public record Multibuy(int Quantity, double Price);