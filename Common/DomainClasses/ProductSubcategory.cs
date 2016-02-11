using System.Diagnostics;
using System.Runtime.Serialization;

namespace Common.DomainClasses
{
    [DataContract]
    [DebuggerDisplay("{ProductCategoryId}, {Id}, {Name}")]
    public partial class ProductSubcategory : DomainClass
    {
        // This is kept non nullable as there is no sense at all in having a Subcategory without a Category.
        [DataMember]
        public int ProductCategoryId { get; set; }
    }
}
