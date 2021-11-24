# AspNetCoreAuthMultiLang
Template for Blazor ASP.NET 6 Core Server Side authentication from any database in a multi-language project.
Version 1.0

Objectives:
- Being DB agnostic
- Support multi language
- Support page reload

For multi-language management install Visual Studio Tool ResXManager
https://marketplace.visualstudio.com/items?itemName=TomEnglert.ResXManager
Create a folder named Resources/Pages
Add resource file with exact same name as pages, for exemple Index.resx
Open in menu Tools/ResX Manager, add languages and create key/values pair

Look at comments in Program.cs

Look at DBAbstraction and change it with your own code for authentication.

Display language mecanism is handle with a cookie. Because server side blazor app are Single Page App you can't change the cookie. The only way to change a cookie it is to do a page refresh. This is done in the AuthController.cs. But this mean you have to save the user id somewhere. I choose to save it in the browser's session storage. Also a great place to keep some other context.

TODO: Introduce JWT Bearer token and link it with Blazor user session for API calls and SignalR protection.