﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCS.AdventureWorks.Products.Standard;
using RCS.AdventureWorks.Products.Standard.Model;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly AdventureWorksContext dbContext;
        private readonly ContextExtension contextExtension;

        public ProductSubcategoriesController(AdventureWorksContext context)
        {
            dbContext = context;
            contextExtension = new ContextExtension(dbContext);
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
                var listDto = contextExtension.GetProductSubcategories();

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
            var productSubcategory = await dbContext.ProductSubcategories.FindAsync(id);

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
            dbContext.ProductSubcategories.Add(productSubcategory);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction("GetProductSubcategory", new { id = productSubcategory.ProductSubcategoryId }, productSubcategory);
        }

        // DELETE: api/ProductSubcategories/5
        [HttpDelete("{id}")]
        private async Task<ActionResult<ProductSubcategory>> DeleteProductSubcategory(int id)
        {
            var productSubcategory = await dbContext.ProductSubcategories.FindAsync(id);
            if (productSubcategory == null)
            {
                return NotFound();
            }

            dbContext.ProductSubcategories.Remove(productSubcategory);
            await dbContext.SaveChangesAsync();

            return productSubcategory;
        }
        #endregion

        #region private
        private bool ProductSubcategoryExists(int id)
        {
            return dbContext.ProductSubcategories.Any(e => e.ProductSubcategoryId == id);
        }
        #endregion
    }
}
