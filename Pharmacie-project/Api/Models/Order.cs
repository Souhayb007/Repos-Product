namespace Api.Models;

public class Order
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }
    public Guid PharmacyId { get; set; }

    public DateTime Date { get; set; }
    public bool WithDelivery { get; set; }
}
