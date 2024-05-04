using Api.Data.Migrations;
using Api.Dtos;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmacyProductController : ControllerBase
    {
        private readonly PharmacyDbContext db;

        public PharmacyProductController(PharmacyDbContext db) { this.db = db; }

        [HttpGet]
        public async Task<List<PharmacyProduct>> GetAllPharmaciesProduct()
        {
            return await this.db.PharmacyProducts.ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<PharmacyProduct>> AjouterPharmacyProduct(DPharmacyProduct pharmacy)
        {
            var p = await db.PharmacyProducts.FirstOrDefaultAsync(pr => pr.PharmacyId == pharmacy.PharmacyId && pr.ProductId == pharmacy.ProductId);
         
            if (p == null)
            {
                var Phar = new PharmacyProduct
                {
                    ProductId = pharmacy.ProductId,
                    PharmacyId = pharmacy.PharmacyId,
                    Price = pharmacy.Price,
                    Available = pharmacy.Available
                };

                db.PharmacyProducts.Add(Phar);
                await db.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { Id = Phar.Id}, Phar);
            }



            return Conflict($" La Pharmacie Products Avec cet Id {p.Id} Deja Connue ");
        }
        [HttpPost]
        public async Task<ActionResult<PharmacyProduct>> GetById(Guid Id)
        {
            PharmacyProduct p = await db.PharmacyProducts.FirstOrDefaultAsync(pr => pr.Id == Id);
            if (p != null)
            {
                return Ok(p);
            }
            return NotFound($"La Pharmacie Products Avec cet Id {Id} N'Existe Pas !");
        }
        [HttpPut]
        public async Task<IActionResult> ModifierPharmacyProduct(Guid Id, DPharmacyProduct pharmacy)
        {
            PharmacyProduct p = await db.PharmacyProducts.FirstOrDefaultAsync(pr => pr.Id == Id);

            if (p == null)
            {
                return NotFound();
            }
            if (p.ProductId != pharmacy.ProductId && p.PharmacyId != pharmacy.PharmacyId)
            { return Conflict(); }


            var Phar = new PharmacyProduct
            {

                ProductId = pharmacy.ProductId,
                PharmacyId = pharmacy.PharmacyId,
                Price = pharmacy.Price,
                Available = pharmacy.Available
            };

            await db.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete]
        public async Task<ActionResult<PharmacyProduct>> DeleteById(Guid Id)
        {
            PharmacyProduct p = await db.PharmacyProducts.FirstOrDefaultAsync(pr => pr.Id == Id);
            if (p != null)
            {
                db.PharmacyProducts.Remove(p);
                await db.SaveChangesAsync();
                return NoContent();
            }
            return NotFound($"La Pharmacie Avec cet Nom {Id} N'Existe Pas ");
        }
        [HttpDelete]
        public async Task<ActionResult<PharmacyProduct>> DeleteByProductandPharmacy(Guid IdProduct,Guid IdPharmacy)
        {
            PharmacyProduct p = await db.PharmacyProducts.FirstOrDefaultAsync(pr => pr.ProductId == IdProduct && pr.PharmacyId ==IdPharmacy);
            if (p != null)
            {
                db.PharmacyProducts.Remove(p);
                await db.SaveChangesAsync();
                return NoContent();
            }
            return NotFound($"La Pharmacie Product Avec cet IdProduct {IdProduct} et PharmacyProduct {IdPharmacy} N'Existe Pas ");
        }
    }
}
