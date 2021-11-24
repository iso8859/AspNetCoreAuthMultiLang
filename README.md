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

TODO: Introduce JWT Bearer token and link it with Blazor user session for API calls and SignalR protection.