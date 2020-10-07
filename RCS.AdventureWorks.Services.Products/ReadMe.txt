An attempt has been made to convert the .csproj file to the new SDK format.
This would make it a lot more compact.
It might also prevent unexpected references to like Core libraries.
Those cause runtime errors when left out.

This has largely succedeeded. Some more parts could probably still be reduced.
Check out the latest stash.

But effectively it does not run as intended with currently 2 problems.
- The library is not recognized to be run in IISExpress. 
  Adding a launchSettings.json only partially helped. Note the schema definition on the web.
  Note that file can also be edited through the project properties as the debug part. 
  It lacks some properties there that are present in the file. 
  The IIS Express profile lacks a lot of the (default) iisSettings properties that can be seen at the API project.
  Apparently they are not recognized. Maybe still because of lacking SDK support.
- Trying to publish to IIS fails with an exception about profiles, though they are present, 
  just like in the other projects (and even still present in the project file). 
  This may have the same cause.

  Apparently this is still not feasible (yet) as mentioned in the extensive guide below.
  Hopefully this will be supported in the future. 
  But that future seems dark for WCF and WPF  support in .net Core or .net 5.
  https://www.michaeltaylorp3.net/migrating-to-sdk-project-format/