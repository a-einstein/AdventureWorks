using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ProductsService
{
    [ServiceContract]
    public interface IProductsService
    {
        [OperationContract(AsyncPattern = true)]
        Task<ProductsOverviewListDto> GetProductsOverview();

        [OperationContract(AsyncPattern = true)]
        Task<ProductDetailsRowDto> GetProductDetails(int productId);

        [OperationContract(AsyncPattern = true)]
        Task<ProductCategoryListDto> GetProductCategories();

        [OperationContract(AsyncPattern = true)]
        Task<ProductSubcategoryListDto> GetProductSubcategories();
    }

    [DataContract]
    [DebuggerDisplay("{ProductID}, {Name}")]
    public partial class ProductsOverviewRowDto
    {
        [DataMember]
        public int ProductID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public decimal ListPrice { get; set; }

        [DataMember]
        public string Size { get; set; }

        [DataMember]
        public string SizeUnitMeasureCode { get; set; }

        [DataMember]
        public string WeightUnitMeasureCode { get; set; }

        [DataMember]
        public byte[] ThumbNailPhoto { get; set; }

        [DataMember]
        public int ProductCategoryID { get; set; }

        [DataMember]
        public string ProductCategory { get; set; }

        [DataMember]
        public string ProductSubcategory { get; set; }

        [DataMember]
        public int ProductSubcategoryID { get; set; }
    }

    [CollectionDataContract]
    public partial class ProductsOverviewListDto : List<ProductsOverviewRowDto>
    {
    }

    [DataContract]
    [DebuggerDisplay("{ProductID}, {Name}")]
    public partial class ProductDetailsRowDto
    {
        [DataMember]
        public int ProductID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public decimal ListPrice { get; set; }

        [DataMember]
        public string Size { get; set; }

        [DataMember]
        public string SizeUnitMeasureCode { get; set; }

        [DataMember]
        public decimal Weight { get; set; }

        [DataMember]
        public string WeightUnitMeasureCode { get; set; }

        [DataMember]
        public byte[] LargePhoto { get; set; }

        [DataMember]
        public int ProductCategoryID { get; set; }

        [DataMember]
        public string ProductCategory { get; set; }

        [DataMember]
        public int ProductSubcategoryID { get; set; }

        [DataMember]
        public string ProductSubcategory { get; set; }

        [DataMember]
        public string ModelName { get; set; }

        [DataMember]
        public string Description { get; set; }
    }

    [DataContract]
    [DebuggerDisplay("{ProductCategoryID}, {Name}")]
    public partial class ProductCategoryRowDto
    {
        [DataMember]
        public int ProductCategoryID { get; set; }

        [DataMember]
        public string Name { get; set; }
    }

    [CollectionDataContract]
    public partial class ProductCategoryListDto : List<ProductCategoryRowDto>
    {
    }

    [DataContract]
    [DebuggerDisplay("{ProductCategoryID}, {ProductSubcategoryID}, {Name}")]
    public partial class ProductSubcategoryRowDto
    {
        [DataMember]
        public int ProductSubcategoryID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int ProductCategoryID { get; set; }
    }

    [CollectionDataContract]
    public partial class ProductSubcategoryListDto : List<ProductSubcategoryRowDto>
    {
    }
}
