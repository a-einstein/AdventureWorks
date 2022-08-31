The entity classes here are generated following:
https://docs.microsoft.com/en-gb/ef/efcore-and-ef6/porting/port-edmx

With the following command & output in the Package Manager Console, with RCS.AdventureWorks.Api.Products as startup project:

PM> Scaffold-DbContext "<connectionString>" Microsoft.EntityFrameworkCore.SqlServer -Force -Project "RCS.AdventureWorks.Products.Standard" -OutputDir "Model"
Build started...
Build succeeded.
Could not find type mapping for column 'HumanResources.Employee.OrganizationNode' with data type 'hierarchyid'. Skipping column.
The column 'HumanResources.Employee.SalariedFlag' would normally be mapped to a non-nullable bool property, but it has a default constraint. Such a column is mapped to a nullable bool property to allow a difference between setting the property to false and invoking the default constraint. See https://go.microsoft.com/fwlink/?linkid=851278 for details.
The column 'HumanResources.Employee.CurrentFlag' would normally be mapped to a non-nullable bool property, but it has a default constraint. Such a column is mapped to a nullable bool property to allow a difference between setting the property to false and invoking the default constraint. See https://go.microsoft.com/fwlink/?linkid=851278 for details.
Unable to scaffold the index 'IX_Employee_OrganizationLevel_OrganizationNode'. The following columns could not be scaffolded: OrganizationNode.
Unable to scaffold the index 'IX_Employee_OrganizationNode'. The following columns could not be scaffolded: OrganizationNode.
Could not find type mapping for column 'Person.Address.SpatialLocation' with data type 'geography'. Skipping column.
The column 'Person.StateProvince.IsOnlyStateProvinceFlag' would normally be mapped to a non-nullable bool property, but it has a default constraint. Such a column is mapped to a nullable bool property to allow a difference between setting the property to false and invoking the default constraint. See https://go.microsoft.com/fwlink/?linkid=851278 for details.
Could not find type mapping for column 'Production.Document.DocumentNode' with data type 'hierarchyid'. Skipping column.
Could not scaffold the primary key for 'Production.Document'. The following columns in the primary key could not be scaffolded: DocumentNode.
Unable to generate entity type for table 'Production.Document'.
The column 'Production.Product.MakeFlag' would normally be mapped to a non-nullable bool property, but it has a default constraint. Such a column is mapped to a nullable bool property to allow a difference between setting the property to false and invoking the default constraint. See https://go.microsoft.com/fwlink/?linkid=851278 for details.
The column 'Production.Product.FinishedGoodsFlag' would normally be mapped to a non-nullable bool property, but it has a default constraint. Such a column is mapped to a nullable bool property to allow a difference between setting the property to false and invoking the default constraint. See https://go.microsoft.com/fwlink/?linkid=851278 for details.
Could not find type mapping for column 'Production.ProductDocument.DocumentNode' with data type 'hierarchyid'. Skipping column.
Could not scaffold the primary key for 'Production.ProductDocument'. The following columns in the primary key could not be scaffolded: DocumentNode.
Unable to generate entity type for table 'Production.ProductDocument'.
The column 'Purchasing.Vendor.PreferredVendorStatus' would normally be mapped to a non-nullable bool property, but it has a default constraint. Such a column is mapped to a nullable bool property to allow a difference between setting the property to false and invoking the default constraint. See https://go.microsoft.com/fwlink/?linkid=851278 for details.
The column 'Purchasing.Vendor.ActiveFlag' would normally be mapped to a non-nullable bool property, but it has a default constraint. Such a column is mapped to a nullable bool property to allow a difference between setting the property to false and invoking the default constraint. See https://go.microsoft.com/fwlink/?linkid=851278 for details.
The column 'Sales.SalesOrderHeader.OnlineOrderFlag' would normally be mapped to a non-nullable bool property, but it has a default constraint. Such a column is mapped to a nullable bool property to allow a difference between setting the property to false and invoking the default constraint. See https://go.microsoft.com/fwlink/?linkid=851278 for details.

1. Intially they are AS GENERATED.
- On the location they were put.
- Apparently for the whole Database, with far more entities and properties than I use.
- Also the definition of the individual entities are more extended compared to the EDMX versions.

2. It has been standardized on plurals with the EDMX version for the relevant parts.

3. Unused entities, properties and related code have been removed.
