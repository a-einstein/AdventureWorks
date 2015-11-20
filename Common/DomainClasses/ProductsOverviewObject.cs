﻿using System.Runtime.Serialization;

namespace Common.DomainClasses
{
    [DataContract]
    public partial class ProductsOverviewObject : DomainClass, IShoppingProduct
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
}
