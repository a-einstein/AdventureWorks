using RCS.AdventureWorks.Products.Standard;
using System.Threading.Tasks;
using DomainClasses = RCS.AdventureWorks.Common.DomainClasses;
using Dtos = RCS.AdventureWorks.Common.Dtos;

namespace RCS.AdventureWorks.Services.Products
{
    public class ProductsService : IProductsService
    {
        // Note that for this to use contextExtension, it needed a couple of of references to Core libraries too.
        // Could the project files also be converted to the new format, to make it more implicit and compact? See a remark elsewehere.

        #region construction
        /*
         NOTE THAT THE Entities CONTEXT CURRENTLY IS NO LONGER USED!
         THIS DbContext IS SHARED WITH the web API.
        */
        private readonly AdventureWorksContext dbContext;
        private readonly ContextExtension contextExtension;

        public ProductsService()
        {
            // Note that the API service gets an instance by a (singleton) injection.
            // This service currently has multiple instances each with their own dbContext instance.
            // Addvantage is that it avoid configuring problems with multiple threads of AdventureWorksContext. See notes there.
            dbContext = new AdventureWorksContext();

            contextExtension = new ContextExtension(dbContext);
        }
        #endregion


        #region Public
        async Task<Dtos.ProductsOverviewList> IProductsService.GetProductsOverviewBy(int? productCategoryId, int? productSubcategoryId, string productNameString)
        {
            var task = Task.Run(() =>
            {
                var listDto = contextExtension.GetProductsOverview(productCategoryId, productSubcategoryId, productNameString);

                return listDto;
            });

            return await task.ConfigureAwait(false);
        }

        async Task<DomainClasses.Product> IProductsService.GetProductDetails(int productId)
        {
            var task = Task.Run(() =>
            {
                var rowDto = contextExtension.GetProductDetails(productId);

                return rowDto;
            });

            return await task.ConfigureAwait(false);
        }

        /*
        https://www.google.com/search?as_q=%22type+initializer+for%22+%22+threw+an+exception%22+static+FileNotFoundException&as_epq=&as_oq=&as_eq=&as_nlo=&as_nhi=&lr=&cr=&as_qdr=y&as_sitesearch=&as_occt=any&safe=images&as_filetype=&tbs=
        https://stackoverflow.com/questions/60042860/system-typeinitializationexception-the-type-initializer-for-bid-threw-an-exc
         */
        async Task<Dtos.ProductCategoryList> IProductsService.GetProductCategories()
        {
            var task = Task.Run(() =>
            {
                var listDto = contextExtension.GetProductCategories();

                return listDto;
            });

            return await task.ConfigureAwait(false);
        }

        async Task<Dtos.ProductSubcategoryList> IProductsService.GetProductSubcategories()
        {
            var task = Task.Run(() =>
            {
                var listDto = contextExtension.GetProductSubcategories();

                return listDto;
            });

            return await task.ConfigureAwait(false);
        }
        #endregion
    }
}
