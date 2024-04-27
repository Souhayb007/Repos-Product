namespace Api.Models;

public class OrderProduct
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }
    public Guid PharmacyProductId { get; set; }

    public int Quantity { get; set; }
}
