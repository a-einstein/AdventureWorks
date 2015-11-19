using System.Diagnostics;
using System.Runtime.Serialization;

namespace Common.DomainClasses
{
    [DataContract]
    [DebuggerDisplay("{ProductCategoryID}, {ProductSubcategoryID}, {Name}")]
    public partial class ProductSubcategory : DomainClass
    {
        [DataMember]
        public int ProductCategoryID { get; set; }
    }
}
