using ProductsService.ProductsDataSetTableAdapters;
using System;
using ProductCategoriesDataTable = ProductsService.ProductsDataSet.ProductCategoriesDataTable;
using ProductCategoriesRow = ProductsService.ProductsDataSet.ProductCategoriesRow;
using ProductDetailsRow = ProductsService.ProductsDataSet.ProductDetailsRow;
using ProductsOverviewRow = ProductsService.ProductsDataSet.ProductsOverviewRow;
using ProductSubcategoriesDataTable = ProductsService.ProductsDataSet.ProductSubcategoriesDataTable;
using ProductSubcategoriesRow = ProductsService.ProductsDataSet.ProductSubcategoriesRow;

namespace ProductsService
{
    // TODO Maybe put this functionality into the partial sub classes of ProductsDataSet. But ProductsDataSet could not be a singleton .
    // Other option: make properties here on wrapper sub classes which are instantiated with a single dataset. The constructor should be restricted some way.
    class ShoppingWrapper
    {
        private ShoppingWrapper()
        {
            productsDataSet = new ProductsDataSet();
        }

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
                            instance = new ShoppingWrapper();
                    }
                }

                return instance;
            }
        }

        // Choose for an int as this is the actual type of the Id.
        public int NoId { get { return -1; } }

        private ProductsDataSet productsDataSet;

        // TODO filter.
        public ProductsOverviewListDto GetProductsOverview()
        {
            FillProductsTable();

            return Convert(productsDataSet.ProductsOverview);
        }

        private void FillProductsTable()
        {
            var productsTableAdapter = new ProductsOverviewTableAdapter() { ClearBeforeFill = true };

            // Note this currently takes in all of the table data. Of course this should be prefiltered and/or paged in a realistic situation. 
            productsTableAdapter.Fill(productsDataSet.ProductsOverview);
        }

        private static ProductsOverviewListDto Convert(ProductsDataSet.ProductsOverviewDataTable table)
        {
            var listDto = new ProductsOverviewListDto();

            foreach (var tableRow in table.Rows)
            {
                var row = tableRow as ProductsOverviewRow;

                var rowDto = new ProductsOverviewRowDto()
                {
                    ProductID = row.ProductID,
                    Name = row.Name,
                    Color = row.Color,
                    ListPrice = row.ListPrice,
                    Size = row.Size,
                    SizeUnitMeasureCode = row.SizeUnitMeasureCode,
                    WeightUnitMeasureCode = row.WeightUnitMeasureCode,
                    ThumbNailPhoto = row.ThumbNailPhoto,
                    ProductCategoryID = row.ProductCategoryID,
                    ProductCategory = row.ProductCategory,
                    ProductSubcategory = row.ProductSubcategory,
                    ProductSubcategoryID = row.ProductSubcategoryID
                };

                listDto.Add(rowDto);
            }

            return listDto;
        }

        // TODO Check filter types.
        public ProductDetailsRowDto GetProductDetails(int productID)
        {
            ProductDetailsRow details = GetProductDetailsBy(productID);

            return Convert(details);
        }

        private static ProductDetailsRow GetProductDetailsBy(int productID)
        {
            var productTableAdapter = new ProductDetailsTableAdapter() { ClearBeforeFill = true };

            // Note this always tries to retrieve a record from the DB.
            var productDetailsTable = productTableAdapter.GetDataBy(productID);

            var details = productDetailsTable.FindByProductID(productID);

            return details;
        }

        private static ProductDetailsRowDto Convert(ProductDetailsRow row)
        {
            var rowDto = new ProductDetailsRowDto()
            {
                ProductID = row.ProductID,
                Name = row.Name,
                Color = row.Color,
                ListPrice = row.ListPrice,
                Size = row.Size,
                SizeUnitMeasureCode = row.SizeUnitMeasureCode,
                Weight = row.Weight,
                WeightUnitMeasureCode = row.WeightUnitMeasureCode,
                LargePhoto = row.LargePhoto,
                ProductCategoryID = row.ProductCategoryID,
                ProductCategory = row.ProductCategory,
                ProductSubcategoryID = row.ProductSubcategoryID,
                ProductSubcategory = row.ProductSubcategory,
                ModelName = row.ModelName,
                Description = row.Description
            };

            return rowDto;
        }

        public ProductCategoryListDto GetProductCategories()
        {
            FillProductCategoriesTable();

            return Convert(productsDataSet.ProductCategories);
        }

        private void FillProductCategoriesTable()
        {
            var categoriesTableAdapter = new ProductCategoriesTableAdapter() { ClearBeforeFill = true };

            // Note this only retrieves the data once, whereas it would probably retrieve it every time in a realistic situation.
            categoriesTableAdapter.Fill(productsDataSet.ProductCategories);
        }

        private static ProductCategoryListDto Convert(ProductCategoriesDataTable table)
        {
            var listDto = new ProductCategoryListDto();

            foreach (var tableRow in table.Rows)
            {
                var row = tableRow as ProductCategoriesRow;

                var rowDto = new ProductCategoryRowDto()
                {
                    ProductCategoryID = row.ProductCategoryID,
                    Name = row.Name
                };

                listDto.Add(rowDto);
            }

            return listDto;
        }

        public ProductSubcategoryListDto GetProductSubcategories()
        {
            FillProductSubcategoriesTable();

            return Convert(productsDataSet.ProductSubcategories);
        }

        private void FillProductSubcategoriesTable()
        {
            var subcategoriesTableAdapter = new ProductSubcategoriesTableAdapter() { ClearBeforeFill = true };

            subcategoriesTableAdapter.Fill(productsDataSet.ProductSubcategories);
        }

        private static ProductSubcategoryListDto Convert(ProductSubcategoriesDataTable table)
        {
            var listDto = new ProductSubcategoryListDto();

            foreach (var tableRow in table.Rows)
            {
                var row = tableRow as ProductSubcategoriesRow;

                var rowDto = new ProductSubcategoryRowDto()
                {
                    ProductSubcategoryID = row.ProductSubcategoryID,
                    Name = row.Name,
                    ProductCategoryID = row.ProductCategoryID
                };

                listDto.Add(rowDto);
            }

            return listDto;
        }
    }
}
