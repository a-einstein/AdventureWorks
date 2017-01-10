using RCS.AdventureWorks.Common.DomainClasses;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RCS.AdventureWorks.Common.Dtos
{
    [CollectionDataContract]
    // Note this is not implemented in Mono.
    [DebuggerDisplay("Count = {Count}")]
    public partial class ProductSubcategoryList : List<ProductSubcategory>
    {
    }
}
