using System;
using System.Linq.Expressions;

namespace RCS.AdventureWorks.Products
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
    }
}
