using AspNetCoreAuthMultiLang.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

// For authentication
builder.Services.AddAuthorizationCore();
// Here create an instance of the DB layer you want to use
builder.Services.AddSingleton<AspNetCoreAuthMultiLang.DBAbstraction>(new AspNetCoreAuthMultiLang.DBMemory());
// Here we specify our version of AuthenticationStateProvider
builder.Services.AddScoped<AuthenticationStateProvider, AspNetCoreAuthMultiLang.MyServerAuthenticationStateProvider>();
builder.Services.AddHttpContextAccessor();

#region Localization
builder.Services.AddLocalization(option => option.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>()
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("fr-FR")
                };
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

#region Localisation
app.UseRequestLocalization(app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value);
#endregion

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
// For authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapControllers();
app.MapFallbackToPage("/_Host");

app.Run();
