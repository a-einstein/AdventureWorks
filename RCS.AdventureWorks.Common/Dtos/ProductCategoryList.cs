using RCS.AdventureWorks.Common.DomainClasses;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RCS.AdventureWorks.Common.Dtos
{
    [CollectionDataContract]
    [DebuggerDisplay("Count = {Count}")]
    public partial class ProductCategoryList : List<ProductCategory>
    {
    }
}
