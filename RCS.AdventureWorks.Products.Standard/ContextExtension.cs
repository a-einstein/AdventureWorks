using LinqKit;
using System;
using System.Linq;
using DomainClasses = RCS.AdventureWorks.Common.DomainClasses;
using Dtos = RCS.AdventureWorks.Common.Dtos;

namespace RCS.AdventureWorks.Products.Standard
{
    // TODO Make this partial of AdventureWorksContext, or an extension? Move there? 
    // Combine with Expressions?
    public class ContextExtension
    {
        #region construction
        private readonly AdventureWorksContext dbContext;

        public ContextExtension(AdventureWorksContext dbContext)
        {
            /*
            TODO >>>> Problem from WPF "DbContext instance cannot be used inside OnConfiguring"
            Seems no way to await config.

            By the way: should not create in instance for the API as that has injection! How to combine with that? 
            Note this code is still in the Controllers with their own injected instance(s) of DbContext.
            Those should be passed. 

            Maybe injection can be done in ProductsService too, but that might demand quite some changes.
            Or put ProductsService on Standard/Core. But that does not support WCF as far as I know.

            Or can this all be reversed, making Standard depend on ProductsService, instead of the other way around?
            */

            this.dbContext = dbContext;
        }
        #endregion

        #region Private ProductsOverviewList
        // TODO Maybe change into universal filter descriptors.
        public Dtos.ProductsOverviewList GetProductsOverview(int? productCategoryId, int? productSubcategoryId, string searchString)
        {
            // The expression is broken down using LINQKit, which extends with Invoke, Expand, AsExpandable.
            // For details and examples:
            // http://www.albahari.com/nutshell/linqkit.aspx
            // https://github.com/scottksmith95/LINQKit
            var productsFilterExpression = Expressions.ProductsFilterExpression(productCategoryId, productSubcategoryId, searchString);
            var productsOverviewObjectExpression = Expressions.ProductsOverviewObjectExpression();

            IQueryable<DomainClasses.ProductsOverviewObject> query =
                dbContext.Products.
                Where(productsFilterExpression.Expand()).
                Select(productsOverviewObjectExpression.Expand()).
                OrderBy(product => product.Name);

            var result = new Dtos.ProductsOverviewList();

            try
            {
                // Note that the query executes on ToList.
                // Note application pool (currently) can't be ApplicationPoolIdentity.
                result.AddRange(query.ToList());

                return result;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        #endregion

        #region Private ProductDetails
        public DomainClasses.Product GetProductDetails(int productId)
        {
            var query =
                // Note this benefits from the joins already defined in the model.
                from product in dbContext.Products
                from productProductPhoto in product.ProductProductPhotoes
                from productModelProductDescriptionCulture in product.ProductModel.ProductModelProductDescriptionCultures
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

            try
            {
                // Note that the query executes on the FirstOrDefault.
                // Note application pool (currently) can't be ApplicationPoolIdentity.
                var result = query.FirstOrDefault();

                return result;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        #endregion

        #region Private Categories
        public Dtos.ProductCategoryList GetProductCategories()
        {
            var query =
                from productCategory in dbContext.ProductCategories
                orderby productCategory.Name
                select new DomainClasses.ProductCategory()
                {
                    Id = productCategory.ProductCategoryId,
                    Name = productCategory.Name
                };

            var result = new Dtos.ProductCategoryList();

            try
            {
                // Note that the query executes on the ToList.
                // Note application pool (currently) can't be ApplicationPoolIdentity.
                result.AddRange(query.ToList());

                return result;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public Dtos.ProductSubcategoryList GetProductSubcategories()
        {
            var query =
                from productSubcategory in dbContext.ProductSubcategories
                orderby productSubcategory.Name
                select new DomainClasses.ProductSubcategory()
                {
                    Id = productSubcategory.ProductSubcategoryId,
                    Name = productSubcategory.Name,
                    ProductCategoryId = productSubcategory.ProductCategoryId
                };

            var result = new Dtos.ProductSubcategoryList();

            try
            {
                // Note that the query executes on the ToList.
                // Note application pool (currently) can't be ApplicationPoolIdentity.
                result.AddRange(query.ToList());

                return result;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        #endregion
    }
}
