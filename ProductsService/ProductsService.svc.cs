using Common.DomainClasses;
using Common.Dtos;
using System.Threading.Tasks;

namespace ProductsService
{
    public class ProductsService : IProductsService
    {
        async Task<ProductsOverviewList> IProductsService.GetProductsOverviewBy(int productCategoryID, int productSubcategoryID, string productNameString)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var listDto = ShoppingWrapper.Instance.GetProductsOverview(productCategoryID, productSubcategoryID, productNameString);

                return listDto;
            });

            return await task;
        }

        async Task<Product> IProductsService.GetProductDetails(int productId)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var rowDto = ShoppingWrapper.Instance.GetProductDetails(productId);

                return rowDto;
            });

            return await task;
        }

        async Task<ProductCategoryList> IProductsService.GetProductCategories()
        {
            var task = Task.Factory.StartNew(() =>
            {
                var listDto = ShoppingWrapper.Instance.GetProductCategories();

                return listDto;
            });

            return await task;
        }

        async Task<ProductSubcategoryList> IProductsService.GetProductSubcategories()
        {
            var task = Task.Factory.StartNew(() =>
            {
                var listDto = ShoppingWrapper.Instance.GetProductSubcategories();

                return listDto;
            });

            return await task;
        }
    }
}
