namespace RCS.AdventureWorks.Common.DomainClasses
{
    public interface IShoppingProduct
    {
        int? Id { get; set; }

        string Name { get; set; }

        string Size { get; set; }

        string SizeUnitMeasureCode { get; set; }

        string Color { get; set; }

        decimal ListPrice { get; set; }
    }
}
