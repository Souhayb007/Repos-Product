using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class Category
{
    
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public string Image { get; set; } = null!;
    public IFormFile ImageFile { get; set; }
    public string ImagePath { get; set; }
    public ICollection<Product> Product { get; set; } = null!;  
}
