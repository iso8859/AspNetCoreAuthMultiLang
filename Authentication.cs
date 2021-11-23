using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
        public myServerAuthenticationStateProvider(IConfiguration Configuration, 
            NavigationManager NavigationManager)
        {
            configuration = Configuration;
            navigationManager = NavigationManager;
        }

        public long m_userId = -1; // -1 = not authenticated
        public string m_sessionId;
        public string m_jwt;

        //public async Task SaveToSessionStorageAsync()
        //{
        //    var s = await GetAuthenticationStateAsync();
        //    if (s.User.Identity.IsAuthenticated)
        //        await sessionStorage.SetItemAsync("session", this.ToJson());
        //}

        //public async Task<bool> LoadFromSessionStorageAsync()
        //{
        //    string cookie = await sessionStorage.GetItemAsync<string>("session");
        //    if (!string.IsNullOrEmpty(cookie))
        //    {
        //        var tmp = BsonSerializer.Deserialize<gdServerAuthenticationStateProvider>(cookie);
        //        if (tmp != null)
        //        {
        //            m_userId = tmp.m_userId;
        //            m_sessionId = tmp.m_sessionId;
        //            m_jwt = tmp.m_jwt;
        //            var ua = await global.GetUserAccountsAsync();
        //            lock (ua)
        //            {
        //                if (ua.ContainsKey(m_userId))
        //                {
        //                    m_currentUser = ua[m_userId];
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    m_userId = -1;
        //    m_sessionId = null;
        //    m_jwt = null;
        //    m_currentUser = null;
        //    return false;
        //}


        public bool IsAuthenticated()
        {
            return m_userId != -1;
        }

        //public async Task<bool> AutoLoginAsync()
        //{
        //    var s = await GetAuthenticationStateAsync();
        //    if (!s.User.Identity.IsAuthenticated)
        //    {
        //        if (await LoadFromSessionStorageAsync())
        //        {
        //            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        public string ApplyCulture(string culture, string redirectionUri) // fr-FR for example
        {
            var query = $"?culture={System.Net.WebUtility.UrlEncode(culture)}&" + $"redirectionUri={System.Net.WebUtility.UrlEncode(redirectionUri)}";
            navigationManager.NavigateTo("/api/v1/auth/setCulture" + query, forceLoad: true);
            return culture;
        }

        public async Task DoLoginAsync(long userId)
        {
            m_userId = userId;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public Task DoLogoutAsync()
        {
            m_userId = -1;
            // await sessionStorage.RemoveItemAsync("session");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return Task.CompletedTask;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            AuthenticationState result = null;
            if (IsAuthenticated())
            {
                result = new AuthenticationState(
                    new ClaimsPrincipal(
                        new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, m_userId.ToString()) }, "appName"
                )));
            }
            else
                result = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())); // Not authenticated
            return Task.FromResult(result);
        }
    }
}
