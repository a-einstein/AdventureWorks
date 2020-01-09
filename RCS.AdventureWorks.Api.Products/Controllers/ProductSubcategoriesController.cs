using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCS.AdventureWorks.Products.Standard;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RCS.AdventureWorks.Api.Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSubcategoriesController : ControllerBase
    {
        #region construction
        private readonly AdventureWorks2014Context _context;

        public ProductSubcategoriesController(AdventureWorks2014Context context)
        {
            _context = context;
        }
        #endregion

        #region API
        // GET: api/ProductSubcategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductSubcategory>>> GetProductSubcategory()
        {
            return await _context.ProductSubcategory.ToListAsync();
        }

        // GET: api/ProductSubcategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductSubcategory>> GetProductSubcategory(int id)
        {
            var productSubcategory = await _context.ProductSubcategory.FindAsync(id);

            if (productSubcategory == null)
            {
                return NotFound();
            }

            return productSubcategory;
        }

        // PUT: api/ProductSubcategories/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductSubcategory(int id, ProductSubcategory productSubcategory)
        {
            if (id != productSubcategory.ProductSubcategoryId)
            {
                return BadRequest();
            }

            _context.Entry(productSubcategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductSubcategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductSubcategories
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ProductSubcategory>> PostProductSubcategory(ProductSubcategory productSubcategory)
        {
            _context.ProductSubcategory.Add(productSubcategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductSubcategory", new { id = productSubcategory.ProductSubcategoryId }, productSubcategory);
        }

        // DELETE: api/ProductSubcategories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductSubcategory>> DeleteProductSubcategory(int id)
        {
            var productSubcategory = await _context.ProductSubcategory.FindAsync(id);
            if (productSubcategory == null)
            {
                return NotFound();
            }

            _context.ProductSubcategory.Remove(productSubcategory);
            await _context.SaveChangesAsync();

            return productSubcategory;
        }
        #endregion

        #region private
        private bool ProductSubcategoryExists(int id)
        {
            return _context.ProductSubcategory.Any(e => e.ProductSubcategoryId == id);
        }
        #endregion
    }
}
