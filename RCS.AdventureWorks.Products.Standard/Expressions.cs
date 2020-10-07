using LinqKit;
using RCS.AdventureWorks.Products.Standard.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using DomainClasses = RCS.AdventureWorks.Common.DomainClasses;

namespace RCS.AdventureWorks.Products.Standard
{
    /// <summary>
    ///  Share code as far as possible, because of subtle Entity Framework model differences.
    /// </summary>
    public class Expressions
    {
        public static Expression<Func<bool>> IsCategoryFilter(int? productCategoryId, int? productSubcategoryId, string searchString)
        {
            return () =>
                productCategoryId.HasValue &&
                !productSubcategoryId.HasValue &&
                string.IsNullOrEmpty(searchString);
        }

        public static Expression<Func<bool>> IsSubcategoryFilter(int? productCategoryId, int? productSubcategoryId, string searchString)
        {
            return () =>
                productCategoryId.HasValue &&
                productSubcategoryId.HasValue &&
                string.IsNullOrEmpty(searchString);
        }

        public static Expression<Func<bool>> IsCategoryAndStringFilter(int? productCategoryId, int? productSubcategoryId, string searchString)
        {
            return () =>
                productCategoryId.HasValue &&
                !productSubcategoryId.HasValue &&
                !string.IsNullOrEmpty(searchString);
        }

        public static Expression<Func<bool>> IsFullFilter(int? productCategoryId, int? productSubcategoryId, string searchString)
        {
            return () =>
                productCategoryId.HasValue &&
                productSubcategoryId.HasValue &&
                !string.IsNullOrEmpty(searchString);
        }

        public static Expression<Func<bool>> IsStringFilter(int? productCategoryId, int? productSubcategoryId, string searchString)
        {
            return () =>
                !productCategoryId.HasValue &&
                !productSubcategoryId.HasValue &&
                !string.IsNullOrEmpty(searchString);
        }

        #region Private ProductsOverviewList
        // Note this part is literally equal as in class ProductsController.
        // It should be shared. 
        // Problem is that they are based on different frameworks, like for DbContext, Expression, ...
        // And based on different data models (which effectively could be the same).

        public static Expression<Func<Product, bool>> CategoryTest(int? productCategoryId)
        {
            return product =>
                product.ProductSubcategory != null &&
                product.ProductSubcategory.ProductCategoryId == productCategoryId;
        }

        public static Expression<Func<Product, bool>> SubcategoryTest(int? productSubcategoryId)
        {
            return product =>
                product.ProductSubcategoryId == productSubcategoryId;
        }

        public static Expression<Func<Product, bool>> StringTest(string searchString)
        {
            // Added IsNullOrEmpty as extra precaution because Contains returns true on an empty searchString.
            return product =>
                !string.IsNullOrEmpty(searchString) &&
                (product.Color.Contains(searchString) || product.Name.Contains(searchString));
        }

        public static Expression<Func<Product, bool>> ProductsFilterExpression(int? productCategoryId, int? productSubcategoryId, string searchString)
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

        public static Expression<Func<Product, DomainClasses.ProductsOverviewObject>> ProductsOverviewObjectExpression()
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
        #endregion
    }
}
