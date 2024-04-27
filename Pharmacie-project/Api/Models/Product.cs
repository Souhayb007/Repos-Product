namespace Api.Models;

public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Image { get; set; } = null!;
    public string Barcode { get; set; } = null!;

    public Guid CategoryId { get; set; }
}
