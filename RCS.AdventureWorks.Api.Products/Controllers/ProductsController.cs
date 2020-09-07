using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCS.AdventureWorks.Products;
using RCS.AdventureWorks.Products.Standard;
using System;
using System.Linq;
using System.Linq.Expressions;
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
        #region construction
        private readonly AdventureWorks2014Context dbContext;

        public ProductsController(AdventureWorks2014Context context)
        {
            dbContext = context;
        }
        #endregion

        // Note the implicit transformation from the entity names to the function names.

        #region API
        // Note this part of routing is to avoid ambiguity.
        [HttpGet("overview")]
        // Optional query parameters may be used. Names must be exact matches (case insensitive), order does not matter. Names are kept short.
        // Example: https://rcsworks.nl/ProductsApi/api/Products/overview?category=3&subcategory=21&word=yel
        public async Task<ActionResult<Dtos.ProductsOverviewList>> GetProduct(int? category, int? subcategory, string word)
        {
            var task = Task.Run(() =>
            {
                var listDto = GetProductsOverview(category, subcategory, word);

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
        // Note this part is very comparable to class ProductsService, but has minor naming differences in the Entity Framework model.

        private bool ProductExists(int id)
        {
            return dbContext.Product.Any(e => e.ProductId == id);
        }

        private static Expression<Func<Product, bool>> CategoryTest(int? productCategoryId)
        {
            return product =>
                product.ProductSubcategory != null &&
                product.ProductSubcategory.ProductCategoryId == productCategoryId;
        }

        private static Expression<Func<Product, bool>> SubcategoryTest(int? productSubcategoryId)
        {
            return product =>
                product.ProductSubcategoryId == productSubcategoryId;
        }

        private static Expression<Func<Product, bool>> StringTest(string searchString)
        {
            // Added IsNullOrEmpty as extra precaution because Contains returns true on an empty searchString.
            return product => 
                !string.IsNullOrEmpty(searchString) && 
                (product.Color.Contains(searchString) || product.Name.Contains(searchString));
        }

        private static Expression<Func<Product, bool>> ProductsFilterExpression(int? productCategoryId, int? productSubcategoryId, string searchString)
        {
            // Note that ProductCategory is reached through ProductSubcategory. 
            // - Product.ProductSubcategoryId -> ProductSubcategory
            // - ProductSubcategory.ProductCategoryId -> ProductCategory
            // Note that Product.ProductSubcategoryId is nullable. So Product may have no ProductSubcategory and thus no ProductCategory.
            // But for a ProductCategory to be applied on a Product, a ProductSubcategory has to be set too.
            // This actually occurs in the current DB and has to be tested for.

            var isCategoryFilter = Expressions.IsCategoryFilter(productCategoryId, productSubcategoryId, searchString);
            var isSubcategoryFilter = Expressions.IsSubcategoryFilter(productCategoryId, productSubcategoryId, searchString);
            var isCategoryAndStringFilter = Expressions.IsCategoryAndStringFilter(productCategoryId, productSubcategoryId, searchString);
            var isFullFilter = Expressions.IsFullFilter(productCategoryId, productSubcategoryId, searchString);
            var isStringFilter = Expressions.IsStringFilter(productCategoryId, productSubcategoryId, searchString);

            var categoryTest = CategoryTest(productCategoryId);
            var subcategoryTest = SubcategoryTest(productSubcategoryId);
            var stringTest = StringTest(searchString);

            // Need to Invoke on variables instead of function calls.
            // Both arguments need to be Expressions.
            Expression<Func<Product, bool>> categoryFilter = product =>
                isCategoryFilter.Invoke() &&
                categoryTest.Invoke(product);

            Expression<Func<Product, bool>> subCategoryFilter = product =>
                isSubcategoryFilter.Invoke() &&
                subcategoryTest.Invoke(product);

            Expression<Func<Product, bool>> categoryAndStringFilter = product =>
                isCategoryAndStringFilter.Invoke() &&
                categoryTest.Invoke(product) &&
                stringTest.Invoke(product);

            Expression<Func<Product, bool>> fullFilter = product =>
                isFullFilter.Invoke() &&
                categoryTest.Invoke(product) &&
                subcategoryTest.Invoke(product) &&
                stringTest.Invoke(product);

            Expression<Func<Product, bool>> stringFilter = product =>
                isStringFilter.Invoke() &&
                stringTest.Invoke(product);

            // The filters must be mutually exclusive.
            return product =>
                (fullFilter.Invoke(product) || subCategoryFilter.Invoke(product)) || categoryAndStringFilter.Invoke(product) || categoryFilter.Invoke(product) || stringFilter.Invoke(product);
        }

        private static Expression<Func<Product, DomainClasses.ProductsOverviewObject>> ProductsOverviewObjectExpression()
        {
            return product => new DomainClasses.ProductsOverviewObject()
            {
                Id = product.ProductId,
                Name = product.Name,
                Color = product.Color,
                ListPrice = product.ListPrice,

                Size = product.Size,
                SizeUnitMeasureCode = product.SizeUnitMeasureCode,

                WeightUnitMeasureCode = product.WeightUnitMeasureCode,
                // Note navigation properties are still applicable.
                ThumbNailPhoto = product.ProductProductPhotoes.FirstOrDefault().ProductPhoto.ThumbNailPhoto,

                ProductCategoryId = (product.ProductSubcategory != null) ? product.ProductSubcategory.ProductCategoryId : (int?)null,
                ProductCategory = (product.ProductSubcategory != null) ? product.ProductSubcategory.ProductCategory.Name : null,

                ProductSubcategoryId = (product.ProductSubcategory != null) ? product.ProductSubcategory.ProductSubcategoryId : (int?)null,
                ProductSubcategory = (product.ProductSubcategory != null) ? product.ProductSubcategory.Name : null
            };
        }

        private Dtos.ProductsOverviewList GetProductsOverview(int? productCategoryId, int? productSubcategoryId, string searchString)
        {
            // The expression is broken down using LINQKit, which extends with Invoke, Expand, AsExpandable.
            // For details and examples:
            // http://www.albahari.com/nutshell/linqkit.aspx
            // https://github.com/scottksmith95/LINQKit
            var productsFilterExpression = ProductsFilterExpression(productCategoryId, productSubcategoryId, searchString);
            var productsOverviewObjectExpression = ProductsOverviewObjectExpression();

            // Need to Expand on variables instead of function calls.
            IQueryable<DomainClasses.ProductsOverviewObject> query =
                dbContext.Product.AsExpandable().
                Where(productsFilterExpression.Expand()).
                Select(productsOverviewObjectExpression.Expand()).
                OrderBy(product => product.Name);

            var result = new Dtos.ProductsOverviewList();

            // Note that the query executes on ToList.
            result.AddRange(query.ToList());

            return result;
        }

        private DomainClasses.Product GetProductDetails(int productId)
        {
            var query =
                // Note this benefits from the joins already defined in the model.
                from product in dbContext.Product
                from productProductPhoto in product.ProductProductPhotoes
                from productModelProductDescriptionCulture in product.ProductModel.ProductModelProductDescriptionCulture
                where
                (
                    (product.ProductId == productId) &&

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
