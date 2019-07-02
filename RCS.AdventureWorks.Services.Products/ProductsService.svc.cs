using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using System.Linq;
using System.Threading.Tasks;

namespace RCS.AdventureWorks.Services.Products
{
    public class ProductsService : IProductsService
    {
        #region Public

        async Task<ProductsOverviewList> IProductsService.GetProductsOverviewBy(int? productCategoryID, int? productSubcategoryID, string productNameString)
        {
            var task = Task.Run(() =>
            {
                var listDto = GetProductsOverview(productCategoryID, productSubcategoryID, productNameString);

                return listDto;
            });

            return await task.ConfigureAwait(false);
        }

        async Task<Product> IProductsService.GetProductDetails(int productId)
        {
            var task = Task.Run(() =>
            {
                var rowDto = GetProductDetails(productId);

                return rowDto;
            });

            return await task.ConfigureAwait(false);
        }

        async Task<ProductCategoryList> IProductsService.GetProductCategories()
        {
            var task = Task.Run(() =>
            {
                var listDto = GetProductCategories();

                return listDto;
            });

            return await task.ConfigureAwait(false);
        }

        async Task<ProductSubcategoryList> IProductsService.GetProductSubcategories()
        {
            var task = Task.Run(() =>
            {
                var listDto = GetProductSubcategories();

                return listDto;
            });

            return await task.ConfigureAwait(false);
        }

        #endregion

        #region Private
        // TODO Maybe change into universal filter descriptors.
        private ProductsOverviewList GetProductsOverview(int? productCategoryId, int? productSubcategoryId, string searchString)
        {
            using (var entitiesContext = new ProductsModel.Entities())
            {
                IQueryable<ProductsOverviewObject> query =
                    from product in entitiesContext.Products
                    from productProductPhotoes in product.ProductProductPhotoes
                    
                    // Note that ProductCategory is reached through ProductSubcategory. 
                    // - Product.ProductSubcategoryId -> ProductSubcategory
                    // - ProductSubcategory.ProductCategoryId -> ProductCategory
                    // Note that Product.ProductSubcategoryID is nullable. So Product mag have no ProductSubcategory and thus ProductCategory.
                    // This actually occurs in the current DB and has to be tested for.
                    
                    // Note one cannot use functions like Expression<Func<ProductsModel.Product, bool>> FunctionName(parameters) lifted outside.
                    // Note one cannot us 'null propagating operators' like '?.' to simplify.
                    
                    // Do not use at least until paged.
                    // Preferably have this visually disabled in GUI too.
                    let noFilter =
                        (searchString == null) && (!productSubcategoryId.HasValue) && (!productCategoryId.HasValue)

                    let categoryFilterable =
                        (searchString == null) && (!productSubcategoryId.HasValue) && (product.ProductSubcategory != null) && (product.ProductSubcategory.ProductCategoryID == productCategoryId)

                    let categoryAndSubcategoryFilterable =
                        (searchString == null) && (product.ProductSubcategory != null) && (product.ProductSubcategory.ProductCategoryID == productCategoryId) && (product.ProductSubcategory.ProductSubcategoryID == productSubcategoryId)

                    let categoryAndSubcategoryAndStringFilterable =
                        (product.ProductSubcategory != null) && (product.ProductSubcategory.ProductCategoryID == productCategoryId) && (product.ProductSubcategory.ProductSubcategoryID == productSubcategoryId) && (product.Color.Contains(searchString) || product.Name.Contains(searchString))

                    let categoryAndStringFilterable =
                        (!productSubcategoryId.HasValue) && (product.ProductSubcategory != null) && (product.ProductSubcategory.ProductCategoryID == productCategoryId) && (product.Color.Contains(searchString) || product.Name.Contains(searchString))

                    let stringFilterable =
                        (!productCategoryId.HasValue) && (!productSubcategoryId.HasValue) && (product.Color.Contains(searchString) || product.Name.Contains(searchString))

                    where categoryFilterable || categoryAndSubcategoryFilterable || categoryAndSubcategoryAndStringFilterable || categoryAndStringFilterable || stringFilterable

                    orderby product.Name

                    select new ProductsOverviewObject()
                    {
                        Id = product.ProductID,
                        Name = product.Name,
                        Color = product.Color,
                        ListPrice = product.ListPrice,

                        Size = product.Size,
                        SizeUnitMeasureCode = product.SizeUnitMeasureCode,

                        WeightUnitMeasureCode = product.WeightUnitMeasureCode,
                        ThumbNailPhoto = productProductPhotoes.ProductPhoto.ThumbNailPhoto,

                        ProductCategoryId = (product.ProductSubcategory != null) ? product.ProductSubcategory.ProductCategoryID : (int?)null,
                        // Dont use fix for IDE0031 (yet). Check: https://github.com/dotnet/roslyn/issues/17623
                        ProductCategory = (product.ProductSubcategory != null) ? product.ProductSubcategory.ProductCategory.Name : null,

                        ProductSubcategoryId = (product.ProductSubcategory != null) ? product.ProductSubcategory.ProductSubcategoryID : (int?)null,
                        // Dont use fix for IDE0031 (yet). Check: https://github.com/dotnet/roslyn/issues/17623
                        ProductSubcategory = (product.ProductSubcategory != null) ? product.ProductSubcategory.Name : null
                    };

                var result = new ProductsOverviewList();

                // Note that the query executes on ToList.
                foreach (var item in query.ToList())
                {
                    result.Add(item);
                }

                return result;
            }
        }

        private Product GetProductDetails(int productID)
        {
            using (var entitiesContext = new ProductsModel.Entities())
            {
                IQueryable<Product> query =
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
                    select new Product()
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
                IQueryable<ProductCategory> query =
                    from productCategory in entitiesContext.ProductCategories
                    orderby productCategory.Name
                    select new ProductCategory()
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
                IQueryable<ProductSubcategory> query =
                    from productSubcategory in entitiesContext.ProductSubcategories
                    orderby productSubcategory.Name
                    select new ProductSubcategory()
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
