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
        private readonly MyContext _DbContext;
        public CategoryController(MyContext dbContext)
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
        public async Task<ActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();

            }
            _DbContext.Entry(category).State = EntityState.Modified;
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
        private bool CategoryAvailable(int id)
        {
            return (_DbContext.Categories?.Any(x => x.Id == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            if (_DbContext.Categories == null)
            {
                return NotFound();
            }
            _DbContext.Categories.Remove(Category);
            await _DbContext.SaveChangesAsync();
            return Ok();
        }
    }
}