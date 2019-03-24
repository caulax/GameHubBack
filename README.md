# GameHub.Backend

This folder contains backend code of GameHub project.

## Structure

* `Startup.cs` - configuration of services, database and their lifetime
* `MappingProfile.cs` - mapping of data structures used for easier conversion between parsed data and database entities
* `GameHubContext.cs` - database configuration and lists for accessing database entities
* `Hubs\GameHub.cs` - communication with clients
* `Entities\` - database entities
* `Services\` - business logic
  * `GameService.cs` - service for game scenario files importing and parsing
  * `GameSessionService\GameController.cs` - service for active games managing
* `Models\` - models for data parsing and transfering
* `Controllers\` - controllers for web interface
* `Views\` - views for web interface

## Development

1) Install [Visual Studio Community 2017 or newer](https://visualstudio.microsoft.com/vs/community/) with the following workloads:
   - ASP.NET and web development
   - .NET Core cross-platform development
2) Open solution and launch Packet Manager Console, run the following command:
   - `Update-Database`
3) On first launch you will be asked to choose Windows Firewall settings. Choose the following:
   - [x] Allow access in home networks
   - [x] Allow access in public networks