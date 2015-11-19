using System.Diagnostics;
using System.Runtime.Serialization;

namespace Common.DomainClasses
{
    [DataContract]
    [DebuggerDisplay("{ProductID}, {Name}")]
    public partial class Product : DomainClass, IShoppingProduct
    {
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
}
