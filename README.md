# AspNetCoreAuthMultiLang
Template for Blazor ASP.NET 6 Core Server Side authentication from any database in a multi-language project.

Use JWT token https://jwt.io/

Add JWT token info in appsetings.json
"jwt": {
    "Secret": "this_is_my_secret :-)",
    "ExpireMinutes": 600
  },

Add Nuget packages
- Microsoft.IdentityModel.Tokens
- Microsoft.AspNetCore.Authentication.JwtBearer

For multi-language management install
https://marketplace.visualstudio.com/items?itemName=TomEnglert.ResXManager
Create a folder named Resources/Pages
Add resource file with exact same name as pages, for exemple Index.resx
Open in menu Tools/ResX Manager, add languages and create key/values pair

Add <CascadingAuthenticationState> in App.razor
https://docs.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-6.0

