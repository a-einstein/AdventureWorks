using System.Diagnostics;

namespace ProductsService
{
    partial class ProductsDataSet
    {
        [DebuggerDisplay("{ProductID}, {Name}")]
        public partial class ProductsOverviewRow { }

        [DebuggerDisplay("{ProductID}, {Name}")]
        public partial class ProductDetailsRow { }

        [DebuggerDisplay("{ProductCategoryID}, {Name}")]
        public partial class ProductCategoriesRow { }

        [DebuggerDisplay("{ProductCategoryID}, {ProductSubcategoryID}, {Name}")]
        public partial class ProductSubcategoriesRow { }
    }
}
