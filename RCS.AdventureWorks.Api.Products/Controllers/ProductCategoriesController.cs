using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCS.AdventureWorks.Products.Standard;
using System.Linq;
using System.Threading.Tasks;
using Dtos = RCS.AdventureWorks.Common.Dtos;

namespace RCS.AdventureWorks.Api.Products.Controllers
{
    // Note the implicit transformation from [controller] to the class name without 'controller'.
    [Route("/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        #region construction
        // Note this is shared instead created in usings as in ProductsService.
        private readonly AdventureWorks2014Context dbContext;
        private readonly ContextExtension contextExtension;

        public ProductCategoriesController(AdventureWorks2014Context context)
        {
            dbContext = context;
            contextExtension = new ContextExtension(dbContext);
        }
        #endregion

        // Note the implicit transformation from the paths to the function names.
        #region API
        // GET: api/ProductCategories
        [HttpGet]
        public async Task<ActionResult<Dtos.ProductCategoryList>> GetProductCategory()
        {
            var task = Task.Run(() =>
            {
                var listDto = contextExtension.GetProductCategories();

                return listDto;
            });

            return await task.ConfigureAwait(false);
        }
        #endregion

        // TODO Currently not yet checked/corrected/removed. Made private.
        #region API not (yet) to be used.
        // GET: api/ProductCategories/5
        [HttpGet("{id}")]
        private async Task<ActionResult<ProductCategory>> GetProductCategory(int id)
        {
            var productCategory = await dbContext.ProductCategories.FindAsync(id);

            if (productCategory == null)
            {
                return NotFound();
            }

            return productCategory;
        }

        // PUT: api/ProductCategories/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        private async Task<IActionResult> PutProductCategory(int id, ProductCategory productCategory)
        {
            if (id != productCategory.ProductCategoryId)
            {
                return BadRequest();
            }

            dbContext.Entry(productCategory).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCategoryExists(id))
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

        // POST: api/ProductCategories
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        private async Task<ActionResult<ProductCategory>> PostProductCategory(ProductCategory productCategory)
        {
            dbContext.ProductCategories.Add(productCategory);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction("GetProductCategory", new { id = productCategory.ProductCategoryId }, productCategory);
        }

        // TODO Currently not yet checked/corrected/removed/disabled.
        // DELETE: api/ProductCategories/5
        [HttpDelete("{id}")]
        private async Task<ActionResult<ProductCategory>> DeleteProductCategory(int id)
        {
            var productCategory = await dbContext.ProductCategories.FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            dbContext.ProductCategories.Remove(productCategory);
            await dbContext.SaveChangesAsync();

            return productCategory;
        }
        #endregion

        // Note this part is based on the current WCF ProductsService, with at least the necessary name changes.
        // If possible the code should be shared (or the EF Core used by the service).
        // TODO Move this to the EF Core?
        #region private
        private bool ProductCategoryExists(int id)
        {
            return dbContext.ProductCategories.Any(e => e.ProductCategoryId == id);
        }
        #endregion
    }
}