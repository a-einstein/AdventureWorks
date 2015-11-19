using System.Diagnostics;
using System.Runtime.Serialization;

namespace Common.DomainClasses
{
    [DataContract]
    [DebuggerDisplay("{ProductCategoryID}, {Name}")]
    public partial class ProductCategory : DomainClass
    {
    }
}
