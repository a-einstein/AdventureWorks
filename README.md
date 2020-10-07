
## AdventureWorks

#### Description
Both Web API and WCF database service tailored for my shop projects, working on the standard AdventureWorks database.

#### News
* The database is updated to AdventureWorks2019.
* Release is now to GitHub with an automatic change summary.
* Added Web API service as alternative to WCF.
* A certified domain is now available for these services to run on. On request.
* Integrated with Azure Devops build and release pipelines.

#### Purpose
* Explore various techniques based on C#, EF, WCF and Web API.
* Manage the code by Git and GitHub.
* Explore continuous integration by using combinations of Git, GitHub, TeamCity and Azure DevOps.
* Explore Scrum process management by integration with Jira and Azure DevOps.

#### Prerequisites
* The service assumes the presence of an AdventureWorks2019 database, to which a connection should be configured.
* For the contents of this database I refer to the **[AdventureWorks page](https://docs.microsoft.com/en-us/sql/samples/adventureworks-install-configure)**.

#### Notes
* Submitted to code analysis by [Better Code Hub](https://bettercodehub.com). Current score: [![BCH compliance](https://bettercodehub.com/edge/badge/a-einstein/AdventureWorks)](https://bettercodehub.com)  
* Connected to automated Azure Devops build and release pipelines. Current build status for the master branch: [![Build Status](https://dev.azure.com/RcsProjects/AdventureWorks/_apis/build/status/Build?branchName=master)](https://dev.azure.com/RcsProjects/AdventureWorks/_build/latest?definitionId=16&branchName=master)

#### Aspects
* C#.
* SQL Server.
* Entity Framework.
* LINQ.
* LinqKit.
* WCF service on .Net Framework.
* Web API service on .Net Standard & .Net Core.
* SSL certification.
* Asynchronisity.
* Has been deployed as an Azure service with an Azure Database.

#### Installation
* This service is mainly intended to be installed on my domain and used by my clients. Running and access is on request.
* The service cannot be installed plug & play, as the database is not included and configured to.
* Download the appropriate .zip file under Assets at the **[releases page](https://github.com/a-einstein/AdventureWorks/releases).**
* Extract if needed.
* Install in IIS.
