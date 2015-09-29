using System.Threading.Tasks;

namespace ProductsService
{
    public class ProductsService : IProductsService
    {
        async Task<ProductsOverviewListDto> IProductsService.GetProductsOverview()
        {
            var task = Task.Factory.StartNew(() =>
            {
                var listDto = ShoppingWrapper.Instance.GetProductsOverview();

                return listDto;
            });

            return await task;
        }

        async Task<ProductDetailsRowDto> IProductsService.GetProductDetails(int productId)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var rowDto = ShoppingWrapper.Instance.GetProductDetails(productId);

                return rowDto;
            });

            return await task;
        }

        async Task<ProductCategoryListDto> IProductsService.GetProductCategories()
        {
            var task = Task.Factory.StartNew(() =>
            {
                var listDto = ShoppingWrapper.Instance.GetProductCategories();

                return listDto;
            });

            return await task;
        }

        async Task<ProductSubcategoryListDto> IProductsService.GetProductSubcategories()
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
