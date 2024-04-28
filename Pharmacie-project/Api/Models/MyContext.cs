using Microsoft.EntityFrameworkCore;

namespace Api.Models
{
    public class MyContext: DbContext
    {
        public DbSet<User> Users { get; set; }  
        public DbSet<Delivery> Deliverys { get; set; } 
        public DbSet<DeliveryOrder> DeliveryOrders { get; set; } 
        public DbSet<Commande> Commandes { get; set; }  
        public DbSet<CommandeProduct> CommandeProducts { get; set; }  
        public DbSet<Category> Categories { get; set; }  
        public DbSet<Pharmacy> Pharmacies { get; set; }  
        public DbSet<PharmacyProduct> PharmacyProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        MyContext(DbContextOptions<MyContext> opt):base(opt)
        { } 
    }
}
