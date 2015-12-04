using Common.Dtos;
using ProductsService.ProductsModel;
using System;
using System.Linq;

namespace ProductsService
{
    // TODO This can probably be moved to the service.
    class ShoppingWrapper
    {
        private ShoppingWrapper()
        { }

        private static volatile ShoppingWrapper instance;
        private static object syncRoot = new Object();

        // Note this class is a singleton, implemented along the way (but not entirely) of https://msdn.microsoft.com/en-us/library/ff650316.aspx
        public static ShoppingWrapper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ShoppingWrapper();
                        }
                    }
                }

                return instance;
            }
        }

        // Choose for an int as this is the actual type of the Id.
        public int noId = -1;

        public ProductsOverviewList GetProductsOverview(int productCategoryID, int productSubcategoryID, string productNameString)
        {
            using (var entitiesContext = new Entities())
            {
                IQueryable<Common.DomainClasses.ProductsOverviewObject> query =
                    from product in entitiesContext.Products
                    from productProductPhotoes in product.ProductProductPhotoes
                    where
                    (
                        // No filters.
                        // TODO Forbid both here as in GUI until paged.
                        (productNameString == null) && (productSubcategoryID == noId) && (productCategoryID == noId) ||

                        // Category.
                        (productNameString == null) && (productSubcategoryID == noId) && (product.ProductSubcategory.ProductCategoryID == productCategoryID) ||

                        // Category && Subcategory.
                        (productNameString == null) && (product.ProductSubcategory.ProductCategoryID == productCategoryID) && (product.ProductSubcategory.ProductSubcategoryID == productSubcategoryID) ||

                        // Category && Subcategory && Name.
                        (product.ProductSubcategory.ProductCategoryID == productCategoryID) && (product.ProductSubcategory.ProductSubcategoryID == productSubcategoryID) && product.Name.Contains(productNameString) ||

                        // Category && Name.
                        (productSubcategoryID == noId) && (product.ProductSubcategory.ProductCategoryID == productCategoryID) && product.Name.Contains(productNameString) ||

                        // Name.
                        (productCategoryID == noId) && (productSubcategoryID == noId) && product.Name.Contains(productNameString)
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
                        ProductCategoryID = product.ProductSubcategory.ProductCategoryID,
                        ProductCategory = product.ProductSubcategory.ProductCategory.Name,
                        ProductSubcategoryID = product.ProductSubcategory.ProductSubcategoryID,
                        ProductSubcategory = product.ProductSubcategory.Name
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

        public Common.DomainClasses.Product GetProductDetails(int productID)
        {
            using (var entitiesContext = new Entities())
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
                        Color = product.Color,
                        ListPrice = product.ListPrice,
                        Size = product.Size,
                        SizeUnitMeasureCode = product.SizeUnitMeasureCode,
                        // TODO Change type.
                        //Weight = product.Weight,
                        WeightUnitMeasureCode = product.WeightUnitMeasureCode,
                        LargePhoto = productProductPhotoes.ProductPhoto.LargePhoto,
                        ProductCategoryID = product.ProductSubcategory.ProductCategoryID,
                        ProductCategory = product.ProductSubcategory.ProductCategory.Name,
                        ProductSubcategoryID = product.ProductSubcategory.ProductSubcategoryID,
                        ProductSubcategory = product.ProductSubcategory.Name,
                        ModelName = product.ProductModel.Name,
                        Description = productModelProductDescriptionCulture.ProductDescription.Description
                    };

                // Note that the query executes on the FirstOrDefault.
                var result = query.FirstOrDefault();

                return result;
            }
        }

        public ProductCategoryList GetProductCategories()
        {
            using (var entitiesContext = new Entities())
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

        public ProductSubcategoryList GetProductSubcategories()
        {
            using (var entitiesContext = new Entities())
            {
                IQueryable<Common.DomainClasses.ProductSubcategory> query =
                    from productSubcategory in entitiesContext.ProductSubcategories
                    orderby productSubcategory.Name
                    select new Common.DomainClasses.ProductSubcategory()
                    {
                        Id = productSubcategory.ProductSubcategoryID,
                        Name = productSubcategory.Name,
                        ProductCategoryID = productSubcategory.ProductCategoryID
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
    }
}
