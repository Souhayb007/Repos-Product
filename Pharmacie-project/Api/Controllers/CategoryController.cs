using Api.Data.Migrations;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly PharmacyDbContext _DbContext;
        public CategoryController(PharmacyDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        {
            if (_DbContext.Categories == null)
            {
                return NotFound();
            }
            return await _DbContext.Categories.ToListAsync();
        }
        [HttpPost("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            if (_DbContext.Categories == null)
            {
                return NotFound();
            }
            var category = await _DbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return category;
        }
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _DbContext.Categories.Add(category);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category); ;
        }
        [HttpPut]
        public async Task<ActionResult> PutCategory(Guid id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();

            }
            _DbContext.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            try
            {
                await _DbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;

                }
            }
            return Ok();
        }
        private bool CategoryAvailable(Guid id)
        {
            return (_DbContext.Categories?.Any(x => x.Id == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(Guid id)
        {
            Category ct = await _DbContext.Categories.Where(c=> c.Id==id).FirstAsync();
            if (ct == null)
            {
                return NotFound();
            }
            _DbContext.Categories.Remove(ct);
            await _DbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}