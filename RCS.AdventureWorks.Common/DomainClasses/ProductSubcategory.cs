using System.Diagnostics;
using System.Runtime.Serialization;

namespace RCS.AdventureWorks.Common.DomainClasses
{
    [DataContract]
    // Note this is not implemented in Mono.
    [DebuggerDisplay("{ProductCategoryId}, {Id.HasValue ? Id.Value : 0}, {Name}")]
    public partial class ProductSubcategory : DomainClass
    {
        // This is kept non nullable as there is no sense at all in having a Subcategory without a Category.
        [DataMember]
        public int ProductCategoryId { get; set; }
    }
}
