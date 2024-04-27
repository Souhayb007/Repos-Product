namespace Api.Models;

public class PharmacyProduct
{
    public Guid Id { get; set; }

    public Guid PharmacyId { get; set; }
    public Guid ProductId { get; set; }

    public decimal Price { get; set; }

    public bool Available { get; set; }
}
