using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class PharmacyProduct
{
    
    public Guid Id { get; set; }
    public Pharmacy Pharmacy { get; set; }  
    public Guid PharmacyId { get; set; }
   
    public Product Product { get; set; }
    public Guid ProductId { get; set; }

    public ICollection<CommandeProduct> CommandeProduct { get; set; } = null!;
    public decimal Price { get; set; }

    public bool Available { get; set; }
}
