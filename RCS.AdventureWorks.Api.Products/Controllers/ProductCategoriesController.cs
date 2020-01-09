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
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        #region construction
        // Note this is shared instead created in usings as in ProductsService.
        private readonly AdventureWorks2014Context dbContext;

        public ProductCategoriesController(AdventureWorks2014Context context)
        {
            dbContext = context;
        }
        #endregion

        // Note the implicit transformation from the paths to the function names.
        #region API
        // GET: api/ProductCategories
        [HttpGet]
        public async Task<ActionResult<Dtos.ProductCategoryList>> GetProductCategory()
        {
            //return await dbContext.ProductCategory.ToListAsync();

            // TODO make async.
            return GetProductCategories();
        }
        #endregion

        // TODO Currently not yet checked/corrected/removed. Made private.
        #region API not (yet) to be used.
        // GET: api/ProductCategories/5
        [HttpGet("{id}")]
        private async Task<ActionResult<ProductCategory>> GetProductCategory(int id)
        {
            var productCategory = await dbContext.ProductCategory.FindAsync(id);

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
            dbContext.ProductCategory.Add(productCategory);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction("GetProductCategory", new { id = productCategory.ProductCategoryId }, productCategory);
        }

        // TODO Currently not yet checked/corrected/removed/disabled.
        // DELETE: api/ProductCategories/5
        [HttpDelete("{id}")]
        private async Task<ActionResult<ProductCategory>> DeleteProductCategory(int id)
        {
            var productCategory = await dbContext.ProductCategory.FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            dbContext.ProductCategory.Remove(productCategory);
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
            return dbContext.ProductCategory.Any(e => e.ProductCategoryId == id);
        }

        private Dtos.ProductCategoryList GetProductCategories()
        {
            IQueryable<DomainClasses.ProductCategory> query =
                from productCategory in dbContext.ProductCategory
                orderby productCategory.Name
                select new DomainClasses.ProductCategory()
                {
                    Id = productCategory.ProductCategoryId,
                    Name = productCategory.Name
                };

            var result = new Dtos.ProductCategoryList();

            // Note that the query executes on the ToList.
            foreach (var item in query.ToList())
            {
                result.Add(item);
            }

            return result;
        }

        private Dtos.ProductSubcategoryList GetProductSubcategories()
        {
            IQueryable<DomainClasses.ProductSubcategory> query =
                from productSubcategory in dbContext.ProductCategory
                orderby productSubcategory.Name
                select new DomainClasses.ProductSubcategory()
                {
                    Id = productSubcategory.ProductCategoryId,
                    Name = productSubcategory.Name,
                    ProductCategoryId = productSubcategory.ProductCategoryId
                };

            var result = new Dtos.ProductSubcategoryList();

            // Note that the query executes on the ToList.
            foreach (var item in query.ToList())
            {
                result.Add(item);
            }

            return result;
        }
        #endregion
    }
}
