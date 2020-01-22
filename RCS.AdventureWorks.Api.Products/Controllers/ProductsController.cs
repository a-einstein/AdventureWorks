using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCS.AdventureWorks.Products.Standard;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainClasses = RCS.AdventureWorks.Common.DomainClasses;
using Dtos = RCS.AdventureWorks.Common.Dtos;

namespace RCS.AdventureWorks.Api.Products.Controllers
{
    // Note the implicit transformation from [controller] to the class name without 'controller'.
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        #region construction
        private readonly AdventureWorks2014Context dbContext;

        public ProductsController(AdventureWorks2014Context context)
        {
            dbContext = context;
        }
        #endregion

        // Note the implicit transformation from the paths to the function names.
        #region API
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            return await dbContext.Product.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DomainClasses.Product>> GetProduct(int id)
        {
            var task = Task.Run(() =>
            {
                var rowDto = GetProductDetails(id);

                return rowDto;
            });

            return await task.ConfigureAwait(false);
        }
        #endregion

        // TODO Currently not yet checked/corrected/removed. Made private.
        #region API not (yet) to be used.
        // PUT: api/Products/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        private async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            dbContext.Entry(product).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        private async Task<ActionResult<Product>> PostProduct(Product product)
        {
            dbContext.Product.Add(product);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        private async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await dbContext.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            dbContext.Product.Remove(product);
            await dbContext.SaveChangesAsync();

            return product;
        }
        #endregion

        #region private
        private bool ProductExists(int id)
        {
            return dbContext.Product.Any(e => e.ProductId == id);
        }

        private DomainClasses.Product GetProductDetails(int productID)
        {
            IQueryable<DomainClasses.Product> query =
                // Note this benefits from the joins already defined in the model.
                from product in dbContext.Product
                from productProductPhoto in product.ProductProductPhoto
                from productModelProductDescriptionCulture in product.ProductModel.ProductModelProductDescriptionCulture
                where
                (
                    (product.ProductId == productID) &&

                    // TODO Should this be used by &&?
                    (productModelProductDescriptionCulture.CultureId == "en") // HACK
                )
                select new DomainClasses.Product()
                {
                    Id = product.ProductId,
                    Name = product.Name,
                    ProductNumber = product.ProductNumber,
                    Color = product.Color,
                    ListPrice = product.ListPrice,

                    Size = product.Size,
                    SizeUnitMeasureCode = product.SizeUnitMeasureCode,

                    Weight = product.Weight,
                    WeightUnitMeasureCode = product.WeightUnitMeasureCode,

                    LargePhoto = productProductPhoto.ProductPhoto.LargePhoto,

                    ProductCategoryId = product.ProductSubcategory.ProductCategoryId,
                    ProductCategory = product.ProductSubcategory.ProductCategory.Name,

                    ProductSubcategoryId = product.ProductSubcategory.ProductSubcategoryId,
                    ProductSubcategory = product.ProductSubcategory.Name,

                    ModelName = product.ProductModel.Name,
                    Description = productModelProductDescriptionCulture.ProductDescription.Description
                };

            // Note that the query executes on the FirstOrDefault.
            var result = query.FirstOrDefault();

            return result;
        }

        #endregion
    }
}
