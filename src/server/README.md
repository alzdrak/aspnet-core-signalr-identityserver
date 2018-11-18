# Server

WebApi that hosts the hubs that the client connects to. Identity Server 4 is setup to be a resource owner type.

## How to run

````
dotnet ef --verbose migrations add Initial
dotnet ef database update -c ApplicationDbContext
dotnet run
````