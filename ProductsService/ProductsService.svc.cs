using Common.DomainClasses;
using Common.Dtos;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsService
{
    public class ProductsService : IProductsService
    {
        #region Public

        async Task<ProductsOverviewList> IProductsService.GetProductsOverviewBy(int productCategoryID, int productSubcategoryID, string productNameString)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var listDto = GetProductsOverview(productCategoryID, productSubcategoryID, productNameString);

                return listDto;
            });

            return await task;
        }

        async Task<Product> IProductsService.GetProductDetails(int productId)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var rowDto = GetProductDetails(productId);

                return rowDto;
            });

            return await task;
        }

        async Task<ProductCategoryList> IProductsService.GetProductCategories()
        {
            var task = Task.Factory.StartNew(() =>
            {
                var listDto = GetProductCategories();

                return listDto;
            });

            return await task;
        }

        async Task<ProductSubcategoryList> IProductsService.GetProductSubcategories()
        {
            var task = Task.Factory.StartNew(() =>
            {
                var listDto = GetProductSubcategories();

                return listDto;
            });

            return await task;
        }

        #endregion

        #region Private

        // Choose for an int as this is the actual type of the Id.
        private const int noId = -1;

        private ProductsOverviewList GetProductsOverview(int productCategoryId, int productSubcategoryId, string productNameString)
        {
            using (var entitiesContext = new ProductsModel.Entities())
            {
                IQueryable<Common.DomainClasses.ProductsOverviewObject> query =
                    from product in entitiesContext.Products
                    from productProductPhotoes in product.ProductProductPhotoes
                    where
                    (
                        // Note that ProductCategory is reached through ProductSubcategory.
                        // Note that product.ProductSubcategoryID is nullable strangely, so there might be no product.ProductSubcategory.
                        // This junk actually exists in the current DB and has to be tested for.

                        // No filters.
                        // TODO Forbid both here as in GUI until paged.
                        (productNameString == null) && (productSubcategoryId == noId) && (productCategoryId == noId) ||

                        // Category.
                        (productNameString == null) && (productSubcategoryId == noId) && (product.ProductSubcategory != null) && (product.ProductSubcategory.ProductCategoryID == productCategoryId) ||

                        // Category && Subcategory.
                        (productNameString == null) && (product.ProductSubcategory != null) && (product.ProductSubcategory.ProductCategoryID == productCategoryId) && (product.ProductSubcategory.ProductSubcategoryID == productSubcategoryId) ||

                        // Category && Subcategory && Name.
                        (product.ProductSubcategory != null) && (product.ProductSubcategory.ProductCategoryID == productCategoryId) && (product.ProductSubcategory.ProductSubcategoryID == productSubcategoryId) && product.Name.Contains(productNameString) ||

                        // Category && Name.
                        (productSubcategoryId == noId) && (product.ProductSubcategory != null) && (product.ProductSubcategory.ProductCategoryID == productCategoryId) && product.Name.Contains(productNameString) ||

                        // Name.
                        (productCategoryId == noId) && (productSubcategoryId == noId) && product.Name.Contains(productNameString)
                    )
                    orderby product.Name
                    select new Common.DomainClasses.ProductsOverviewObject()
                    {
                        Id = product.ProductID,
                        Name = product.Name,
                        Color = product.Color,
                        ListPrice = product.ListPrice,
                        Size = product.Size,
                        SizeUnitMeasureCode = product.SizeUnitMeasureCode,
                        WeightUnitMeasureCode = product.WeightUnitMeasureCode,
                        ThumbNailPhoto = productProductPhotoes.ProductPhoto.ThumbNailPhoto,
                        ProductCategoryId = (product.ProductSubcategory != null) ? product.ProductSubcategory.ProductCategoryID : noId,
                        ProductCategory = (product.ProductSubcategory != null) ? product.ProductSubcategory.ProductCategory.Name : null,
                        ProductSubcategoryId = (product.ProductSubcategory != null) ? product.ProductSubcategory.ProductSubcategoryID : noId,
                        ProductSubcategory = (product.ProductSubcategory != null) ? product.ProductSubcategory.Name : null
                    };

                var result = new ProductsOverviewList();

                // Note that the query executes on the ToList.
                foreach (var item in query.ToList())
                {
                    result.Add(item);
                }

                return result;
            }
        }

        private Common.DomainClasses.Product GetProductDetails(int productID)
        {
            using (var entitiesContext = new ProductsModel.Entities())
            {
                IQueryable<Common.DomainClasses.Product> query =
                    // Note this benefits from the joins already defined in the model.
                    from product in entitiesContext.Products
                    from productProductPhotoes in product.ProductProductPhotoes
                    from productModelProductDescriptionCulture in product.ProductModel.ProductModelProductDescriptionCultures
                    where
                    (
                        (product.ProductID == productID) &&

                        // TODO Should this be used by &&?
                        (productModelProductDescriptionCulture.CultureID == "en") // HACK
                    )
                    select new Common.DomainClasses.Product()
                    {
                        Id = product.ProductID,
                        Name = product.Name,
                        ProductNumber = product.ProductNumber,
                        Color = product.Color,
                        ListPrice = product.ListPrice,
                        Size = product.Size,
                        SizeUnitMeasureCode = product.SizeUnitMeasureCode,
                        Weight = product.Weight,
                        WeightUnitMeasureCode = product.WeightUnitMeasureCode,
                        LargePhoto = productProductPhotoes.ProductPhoto.LargePhoto,
                        ProductCategoryId = product.ProductSubcategory.ProductCategoryID,
                        ProductCategory = product.ProductSubcategory.ProductCategory.Name,
                        ProductSubcategoryId = product.ProductSubcategory.ProductSubcategoryID,
                        ProductSubcategory = product.ProductSubcategory.Name,
                        ModelName = product.ProductModel.Name,
                        Description = productModelProductDescriptionCulture.ProductDescription.Description
                    };

                // Note that the query executes on the FirstOrDefault.
                var result = query.FirstOrDefault();

                return result;
            }
        }

        private ProductCategoryList GetProductCategories()
        {
            using (var entitiesContext = new ProductsModel.Entities())
            {
                IQueryable<Common.DomainClasses.ProductCategory> query =
                    from productCategory in entitiesContext.ProductCategories
                    orderby productCategory.Name
                    select new Common.DomainClasses.ProductCategory()
                    {
                        Id = productCategory.ProductCategoryID,
                        Name = productCategory.Name
                    };

                var result = new ProductCategoryList();

                // Note that the query executes on the ToList.
                foreach (var item in query.ToList())
                {
                    result.Add(item);
                }

                return result;
            }
        }

        private ProductSubcategoryList GetProductSubcategories()
        {
            using (var entitiesContext = new ProductsModel.Entities())
            {
                IQueryable<Common.DomainClasses.ProductSubcategory> query =
                    from productSubcategory in entitiesContext.ProductSubcategories
                    orderby productSubcategory.Name
                    select new Common.DomainClasses.ProductSubcategory()
                    {
                        Id = productSubcategory.ProductSubcategoryID,
                        Name = productSubcategory.Name,
                        ProductCategoryId = productSubcategory.ProductCategoryID
                    };

                var result = new ProductSubcategoryList();

                // Note that the query executes on the ToList.
                foreach (var item in query.ToList())
                {
                    result.Add(item);
                }

                return result;
            }
        }

        #endregion
    }
}
