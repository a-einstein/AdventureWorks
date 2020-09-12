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
    public class ProductsController : ControllerBase
    {
        // TODO This could be shared.
        #region construction
        private readonly AdventureWorks2014Context dbContext;
        private readonly ContextExtension contextExtension;

        public ProductsController(AdventureWorks2014Context context)
        {
            dbContext = context;
            contextExtension = new ContextExtension(dbContext);
        }
        #endregion

        // Note the implicit transformation from the entity names to the function names.

        #region API
        // TODO This may even coincide with the ProductsService part, with some renaming.

        // Note this part of routing is to avoid ambiguity.
        [HttpGet("overview")]
        // Optional query parameters may be used. Names must be exact matches (case insensitive), order does not matter. Names are kept short.
        // Example: https://rcsworks.nl/ProductsApi/api/Products/overview?category=3&subcategory=21&word=yel
        public async Task<ActionResult<Dtos.ProductsOverviewList>> GetProduct(int? category, int? subcategory, string word)
        {
            var task = Task.Run(() =>
            {
                var listDto = contextExtension.GetProductsOverview(category, subcategory, word);

                return listDto;
            });

            return await task.ConfigureAwait(false);
        }

        // Note this part of routing is to avoid ambiguity.
        [HttpGet("details")]
        // Example: https://localhost:44372/api/Products/details?id=883
        public async Task<ActionResult<DomainClasses.Product>> GetProduct(int id)
        {
            var task = Task.Run(() =>
            {
                var rowDto = contextExtension.GetProductDetails(id);

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
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        private async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();

            return product;
        }
        #endregion

        #region private
        private bool ProductExists(int id)
        {
            return dbContext.Products.Any(e => e.ProductId == id);
        }
        #endregion
    }
}