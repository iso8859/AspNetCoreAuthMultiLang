using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Sockets;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCoreAuthMultiLang
{
    public class myServerAuthenticationStateProvider : AuthenticationStateProvider
    {

        public readonly IConfiguration configuration;
        public readonly NavigationManager navigationManager;
        public readonly ProtectedSessionStorage protectedSessionStore;
        public readonly IHttpContextAccessor httpContextAccessor;
        public myServerAuthenticationStateProvider(IConfiguration Configuration, 
            NavigationManager NavigationManager,
            ProtectedSessionStorage ProtectedSessionStore,
            IHttpContextAccessor HttpContextAccessor)
        {
            configuration = Configuration;
            navigationManager = NavigationManager;
            protectedSessionStore = ProtectedSessionStore;
            httpContextAccessor = HttpContextAccessor;
        }

        public string m_login = null;
        public string m_culture = "fr-FR";

        public bool IsAuthenticated()
        {
            return m_login!=null;
        }

        public async Task TryLoginAsync()
        {
            if (!IsAuthenticated())
            {
                var result = await protectedSessionStore.GetAsync<string>("login");
                if (result.Success)
                    await DoLoginAsync(result.Value, null);
            }
        }

        public void ApplyCulture(string culture, string redirectionUri) // fr-FR for example
        {
            var query = $"?culture={System.Net.WebUtility.UrlEncode(culture)}&" + $"redirectionUri={System.Net.WebUtility.UrlEncode(redirectionUri)}";
            navigationManager.NavigateTo("/api/v1/auth/setCulture" + query, forceLoad: true);
        }

        public async Task DoLoginAsync(string login, string path)
        {
            m_login = login;
            await protectedSessionStore.SetAsync("login", login);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            if (!string.IsNullOrEmpty(path))
                ApplyCulture(m_culture, path);
        }

        public async Task DoLogoutAsync()
        {
            m_login=null;
            await protectedSessionStore.DeleteAsync("login");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            AuthenticationState result = null;
            if (IsAuthenticated())
            {
                result = new AuthenticationState(
                    new ClaimsPrincipal(
                        new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, m_login) }, "appName"
                )));
            }
            else
                result = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())); // Not authenticated
            return Task.FromResult(result);
        }
    }
}
