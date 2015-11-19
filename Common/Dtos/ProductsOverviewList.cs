using Common.DomainClasses;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Common.Dtos
{
    [CollectionDataContract]
    [DebuggerDisplay("Count = {Count}")]
    public partial class ProductsOverviewList : List<ProductsOverviewObject>
    {
    }
}
