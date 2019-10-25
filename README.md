
## AdventureWorks

#### Description
WCF database service tailored for my shop projects, working on the standard AdventureWorks database.

#### News
* Integrated with Azure Devops build and release pipelines.
* This data service  is no longer fully functional on Azure.

#### Purpose
* Explore various techniques based on C#, EF and WCF.
* Manage the code by Git and GitHub.
* Explore continuous integration by using combinations of Git, GitHub, TeamCity and Azure DevOps.
* Explore Scrum process management by integration with Jira and Azure DevOps.

#### Prerequisites
* The service assumes the presence of an AdventureWorks2014 database, to which a connection should be configured.
* Note that currently a self signed certifcate on the server is enough to let the https binding function and be accepted by an Android device.
* For the contents of this database I refer to http://msftdbprodsamples.codeplex.com/releases/view/125550

#### Notes
* Submitted to code analysis by [Better Code Hub](https://bettercodehub.com). Current score: [![BCH compliance](https://bettercodehub.com/edge/badge/a-einstein/AdventureWorks)](https://bettercodehub.com)  
* Connected to automated Azure Devops build and release pipelines. Current build status for the master branch: [![Build Status](https://dev.azure.com/RcsProjects/AdventureWorks/_apis/build/status/Build?branchName=master)](https://dev.azure.com/RcsProjects/AdventureWorks/_build/latest?definitionId=16&branchName=master)

#### Aspects
* C#.
* SQL Server.
* Use a Azure Database.
* Be deployed as an Azure service.
* Entity Framework.
* WCF + SSL.
* Asynchronisity.

#### Installation
The service cannot be installed plug & play, as the database is not included and configured to. But for demo purposes the following stubs are available. 
* One can download and install the **[latest ZIP](https://rcsadventureworac85.blob.core.windows.net/adventureworks-releases/latest/RCS.AdventureWorks.Services.Products.zip)** .
* The deployed service is up and running and can be **[referred to](https://rcs-adventureworksservices.azurewebsites.net/ProductsService.svc)**, but is not correctly configured.
