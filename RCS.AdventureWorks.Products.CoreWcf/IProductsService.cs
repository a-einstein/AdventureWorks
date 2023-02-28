using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;

namespace RCS.AdventureWorks.Products.CoreWcf
{
    // Note This cant't be shared with plain WCF (through another project),
    // apparently because of the discrepancy between between dependencies of System.ServiceModel and CoreWCF.
    // Essentially it's a duplicate now.

    [ServiceContract]
    internal interface IProductsService
    {
        [OperationContract(AsyncPattern = true)]
        Task<ProductsOverviewList> GetProductsOverviewBy(int? productCategoryId, int? productSubcategoryId, string productNameString);

        [OperationContract(AsyncPattern = true)]
        Task<Product> GetProductDetails(int productId);

        [OperationContract(AsyncPattern = true)]
        Task<ProductCategoryList> GetProductCategories();

        [OperationContract(AsyncPattern = true)]
        Task<ProductSubcategoryList> GetProductSubcategories();
    }
}
