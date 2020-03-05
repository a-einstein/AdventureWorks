﻿using LinqKit;
using RCS.AdventureWorks.Products;
using RCS.AdventureWorks.Services.Products.ProductsModel;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DomainClasses = RCS.AdventureWorks.Common.DomainClasses;
using Dtos = RCS.AdventureWorks.Common.Dtos;

namespace RCS.AdventureWorks.Services.Products
{
    public class ProductsService : IProductsService
    {
        #region Public
        async Task<Dtos.ProductsOverviewList> IProductsService.GetProductsOverviewBy(int? productCategoryId, int? productSubcategoryId, string productNameString)
        {
            var task = Task.Run(() =>
            {
                var listDto = GetProductsOverview(productCategoryId, productSubcategoryId, productNameString);

                return listDto;
            });

            return await task.ConfigureAwait(false);
        }

        async Task<DomainClasses.Product> IProductsService.GetProductDetails(int productId)
        {
            var task = Task.Run(() =>
            {
                var rowDto = GetProductDetails(productId);

                return rowDto;
            });

            return await task.ConfigureAwait(false);
        }

        async Task<Dtos.ProductCategoryList> IProductsService.GetProductCategories()
        {
            var task = Task.Run(() =>
            {
                var listDto = GetProductCategories();

                return listDto;
            });

            return await task.ConfigureAwait(false);
        }

        async Task<Dtos.ProductSubcategoryList> IProductsService.GetProductSubcategories()
        {
            var task = Task.Run(() =>
            {
                var listDto = GetProductSubcategories();

                return listDto;
            });

            return await task.ConfigureAwait(false);
        }
        #endregion

        #region Private ProductsOverviewList
        private static Expression<Func<Product, bool>> CategoryTest(int? productCategoryId)
        {
            return product =>
                product.ProductSubcategory != null &&
                product.ProductSubcategory.ProductCategoryID == productCategoryId;
        }

        private static Expression<Func<Product, bool>> SubcategoryTest(int? productSubcategoryId)
        {
            return product =>
                product.ProductSubcategoryID == productSubcategoryId;
        }

        private static Expression<Func<Product, bool>> StringTest(string searchString)
        {
            return product =>
               product.Color.Contains(searchString) || product.Name.Contains(searchString);
        }

        private static Expression<Func<Product, bool>> ProductsFilterExpression(int? productCategoryId, int? productSubcategoryId, string searchString)
        {
            // Note that ProductCategory is reached through ProductSubcategory. 
            // - Product.ProductSubcategoryId -> ProductSubcategory
            // - ProductSubcategory.ProductCategoryId -> ProductCategory
            // Note that Product.ProductSubcategoryID is nullable. So Product may have no ProductSubcategory and thus no ProductCategory.
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

        private static Expression<Func<Product, DomainClasses.ProductsOverviewObject>> ProductsOverviewObjectExpression()
        {
            return product => new DomainClasses.ProductsOverviewObject()
            {
                Id = product.ProductID,
                Name = product.Name,
                Color = product.Color,
                ListPrice = product.ListPrice,

                Size = product.Size,
                SizeUnitMeasureCode = product.SizeUnitMeasureCode,

                WeightUnitMeasureCode = product.WeightUnitMeasureCode,
                // Note navigation properties are still applicable.
                ThumbNailPhoto = product.ProductProductPhotoes.FirstOrDefault().ProductPhoto.ThumbNailPhoto,

                ProductCategoryId = (product.ProductSubcategory != null) ? product.ProductSubcategory.ProductCategoryID : (int?)null,
                ProductCategory = (product.ProductSubcategory != null) ? product.ProductSubcategory.ProductCategory.Name : null,

                ProductSubcategoryId = (product.ProductSubcategory != null) ? product.ProductSubcategory.ProductSubcategoryID : (int?)null,
                ProductSubcategory = (product.ProductSubcategory != null) ? product.ProductSubcategory.Name : null
            };
        }

        // TODO Maybe change into universal filter descriptors.
        private Dtos.ProductsOverviewList GetProductsOverview(int? productCategoryId, int? productSubcategoryId, string searchString)
        {
            using (var entitiesContext = new Entities())
            {
                // The expression is broken down using LINQKit, which extends with Invoke, Expand, AsExpandable.
                // For details and examples:
                // http://www.albahari.com/nutshell/linqkit.aspx
                // https://github.com/scottksmith95/LINQKit
                var productsFilterExpression = ProductsFilterExpression(productCategoryId, productSubcategoryId, searchString);
                var productsOverviewObjectExpression = ProductsOverviewObjectExpression();

                // Need to Expand on variables instead of function calls.
                IQueryable<DomainClasses.ProductsOverviewObject> query =
                    entitiesContext.Products.AsExpandable().
                    Where(productsFilterExpression.Expand()).
                    Select(productsOverviewObjectExpression.Expand()).
                    OrderBy(product => product.Name);

                var result = new Dtos.ProductsOverviewList();

                // Note that the query executes on ToList.
                result.AddRange(query.ToList());

                return result;
            }
        }
        #endregion

        #region Private ProductDetails
        private DomainClasses.Product GetProductDetails(int productId)
        {
            using (var entitiesContext = new Entities())
            {
                IQueryable<DomainClasses.Product> query =
                    // Note this benefits from the joins already defined in the model.
                    from product in entitiesContext.Products
                    from productProductPhoto in product.ProductProductPhotoes
                    from productModelProductDescriptionCulture in product.ProductModel.ProductModelProductDescriptionCultures
                    where
                    (
                        (product.ProductID == productId) &&

                        // TODO Should this be used by &&?
                        (productModelProductDescriptionCulture.CultureID == "en") // HACK
                    )
                    select new DomainClasses.Product()
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

                        LargePhoto = productProductPhoto.ProductPhoto.LargePhoto,

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
        #endregion

        #region Private Categories
        private Dtos.ProductCategoryList GetProductCategories()
        {
            using (var entitiesContext = new Entities())
            {
                IQueryable<DomainClasses.ProductCategory> query =
                    from productCategory in entitiesContext.ProductCategories
                    orderby productCategory.Name
                    select new DomainClasses.ProductCategory()
                    {
                        Id = productCategory.ProductCategoryID,
                        Name = productCategory.Name
                    };

                var result = new Dtos.ProductCategoryList();

                // Note that the query executes on the ToList.
                result.AddRange(query.ToList());

                return result;
            }
        }

        private Dtos.ProductSubcategoryList GetProductSubcategories()
        {
            using (var entitiesContext = new Entities())
            {
                IQueryable<DomainClasses.ProductSubcategory> query =
                    from productSubcategory in entitiesContext.ProductSubcategories
                    orderby productSubcategory.Name
                    select new DomainClasses.ProductSubcategory()
                    {
                        Id = productSubcategory.ProductSubcategoryID,
                        Name = productSubcategory.Name,
                        ProductCategoryId = productSubcategory.ProductCategoryID
                    };

                var result = new Dtos.ProductSubcategoryList();

                // Note that the query executes on the ToList.
                result.AddRange(query.ToList());

                return result;
            }
        }
        #endregion
    }
}
