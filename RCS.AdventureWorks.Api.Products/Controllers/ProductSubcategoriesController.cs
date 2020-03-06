using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCS.AdventureWorks.Products.Standard;
using System.Linq;
using System.Threading.Tasks;
using DomainClasses = RCS.AdventureWorks.Common.DomainClasses;
using Dtos = RCS.AdventureWorks.Common.Dtos;

namespace RCS.AdventureWorks.Api.Products.Controllers
{
    // Note the implicit transformation from [controller] to the class name without 'controller'.
    [Route("/[controller]")]
    [ApiController]
    public class ProductSubcategoriesController : ControllerBase
    {
        #region construction
        // Note this is shared instead created in usings as in ProductsService.
        private readonly AdventureWorks2014Context dbContext;

        public ProductSubcategoriesController(AdventureWorks2014Context context)
        {
            dbContext = context;
        }
        #endregion

        // Note the implicit transformation from the paths to the function names.
        #region API
        // GET: api/ProductSubcategories
        [HttpGet]
        public async Task<ActionResult<Dtos.ProductSubcategoryList>> GetProductSubcategory()
        {
            var task = Task.Run(() =>
            {
                var listDto = GetProductSubcategories();

                return listDto;
            });

            return await task.ConfigureAwait(false);
        }
        #endregion

        // TODO Currently not yet checked/corrected/removed. Made private.
        #region API not (yet) to be used.
        // GET: api/ProductSubcategories/5
        [HttpGet("{id}")]
        private async Task<ActionResult<ProductSubcategory>> GetProductSubcategory(int id)
        {
            var productSubcategory = await dbContext.ProductSubcategory.FindAsync(id);

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
        private async Task<IActionResult> PutProductSubcategory(int id, ProductSubcategory productSubcategory)
        {
            if (id != productSubcategory.ProductSubcategoryId)
            {
                return BadRequest();
            }

            dbContext.Entry(productSubcategory).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
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
        private async Task<ActionResult<ProductSubcategory>> PostProductSubcategory(ProductSubcategory productSubcategory)
        {
            dbContext.ProductSubcategory.Add(productSubcategory);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction("GetProductSubcategory", new { id = productSubcategory.ProductSubcategoryId }, productSubcategory);
        }

        // DELETE: api/ProductSubcategories/5
        [HttpDelete("{id}")]
        private async Task<ActionResult<ProductSubcategory>> DeleteProductSubcategory(int id)
        {
            var productSubcategory = await dbContext.ProductSubcategory.FindAsync(id);
            if (productSubcategory == null)
            {
                return NotFound();
            }

            dbContext.ProductSubcategory.Remove(productSubcategory);
            await dbContext.SaveChangesAsync();

            return productSubcategory;
        }
        #endregion

        #region private
        private bool ProductSubcategoryExists(int id)
        {
            return dbContext.ProductSubcategory.Any(e => e.ProductSubcategoryId == id);
        }

        private Dtos.ProductSubcategoryList GetProductSubcategories()
        {
            var query =
                from productSubcategory in dbContext.ProductSubcategory
                orderby productSubcategory.Name
                select new DomainClasses.ProductSubcategory()
                {
                    Id = productSubcategory.ProductSubcategoryId,
                    Name = productSubcategory.Name,
                    ProductCategoryId = productSubcategory.ProductCategoryId
                };

            var result = new Dtos.ProductSubcategoryList();

            // Note that the query executes on the ToList.
            result.AddRange(query.ToList());

            return result;
        }
        #endregion
    }
}
