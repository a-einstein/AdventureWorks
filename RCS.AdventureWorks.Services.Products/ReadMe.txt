Currently the .csproj file has been converted to the new SDK format, with some side effects.

- The library is not recognized to be run in IISExpress. 
  Adding a launchSettings.json only partially helped. Note the schema definition on the web.
  Note that file can also be edited through the project properties as the debug part. 
  It lacks some properties there that are present in the file. 
  The IIS Express profile lacks a lot of the (default) iisSettings properties that can be seen at the API project.
  Apparently they are not recognized. Maybe still because of lacking SDK support.

- Publishing to the IIS directory has been impaired, which has to be corrected on the following points. 
  Either by hand when publishing locally, or scripted in Azure DevOps to create a usable package.
  - Move files ProductsService.svc and Web.config from the bin to the root directory.
  - Remove the generated element system.webServer from Web.config.

Apparently this is still problematic as mentioned in the extensive guide below.
Hopefully this will be supported in the future. 
But that future seems dark for WCF and WPF  support in .net Core or .net 5.
https://www.michaeltaylorp3.net/migrating-to-sdk-project-format/

A possible next step maybe applying Core WCF.
https://github.com/CoreWCF