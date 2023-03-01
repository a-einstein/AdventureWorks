
## AdventureWorks

#### Summary
Data services tailored for my shop projects, working on the standard AdventureWorks database.

#### Purpose
* Explore coding techniques based on C#, EF, WCF, CoreWcf and Web API.
* Explore continuous integration by using combination of Git, GitHub, and Azure DevOps.

#### Context
* Connected to Azure Devops self hosted build pipeline. Current build status for master branch: ![](https://dev.azure.com/RcsProjects/AdventureWorks/_apis/build/status/Build?branchName=master)
* Azure DevOps release pipeline to GitHub.
* See **[Release Notes](ReleaseNotes.md)** for the latest developments.

#### Project aspects
* C#.
* SQL Server.
* Entity Framework.
* LINQ.
* LinqKit.
* WCF service on .Net Framework 4.8.
* CoreWcf service on .Net 7.
* Web API service on .Net Standard 2.0 & .Net 7.
* SSL certification.
* Asynchronisity.

#### Prerequisites
* The service assumes the presence of an AdventureWorks2019 database, to which a connection should be configured.
* For the contents of this database I refer to the **[AdventureWorks page](https://docs.microsoft.com/en-us/sql/samples/adventureworks-install-configure)**.

#### Installation
* This service is mainly intended to be installed on my domain and used by my clients. Running and access is on request.
* The service cannot be installed plug & play, as the database is not included and configured to.
* Download the appropriate .zip file under Assets at the **[releases page](https://github.com/a-einstein/AdventureWorks/releases).**
* Extract if needed.
* Install in IIS.
