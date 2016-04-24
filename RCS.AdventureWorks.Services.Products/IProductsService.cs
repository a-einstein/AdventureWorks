using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using System.ServiceModel;
using System.Threading.Tasks;

namespace  RCS.AdventureWorks.Services.Products
{
    [ServiceContract]
    interface IProductsService
    {
        [OperationContract(AsyncPattern = true)]
        Task<ProductsOverviewList> GetProductsOverviewBy(int? productCategoryID, int? productSubcategoryID, string productNameString);

        [OperationContract(AsyncPattern = true)]
        Task<Product> GetProductDetails(int productId);

        [OperationContract(AsyncPattern = true)]
        Task<ProductCategoryList> GetProductCategories();

        [OperationContract(AsyncPattern = true)]
        Task<ProductSubcategoryList> GetProductSubcategories();
    }
}
