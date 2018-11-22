# Server

WebApi that hosts the hubs that the client connects to. Identity Server 4 is setup to be a resource owner type.

## How to run

You will need to scaffold asp.net core identity tables to your local database.
````
dotnet ef --verbose migrations add Initial
dotnet ef database update -c ApplicationDbContext
````

Runs the app in the development mode.<br>
````
dotnet run
````

Open [http://localhost:50000](http://localhost:50000) to view it in the browser.

The page will update and show all connected clients.<br>