using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace AspNetCoreAuthMultiLang.Pages
{
    public class LoadUserContext : ComponentBase
    {
        [Inject] AuthenticationStateProvider _asp { get; set; }
        public MyServerAuthenticationStateProvider _masp { get; set;}
        protected override void OnInitialized()
        {
            _masp = (MyServerAuthenticationStateProvider)_asp;
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                await _masp.TryLoginAsync();
            if (_masp.IsAuthenticated())
                await OnAfterRenderContextAsync(firstRender);
        }

        // In your page override this method to be sure user is authenticated
        public virtual Task OnAfterRenderContextAsync(bool firstRender)
        {
            return Task.CompletedTask;
        }

        public Task DoLoginAsync(string login, string url) => _masp.DoLoginAsync(login, url);
        public Task DoLogoutAsync() => _masp.DoLogoutAsync();

        public List<string> GetRoles(ClaimsPrincipal user)
        {
            var userIdentity = (ClaimsIdentity)user.Identity;
            var claims = userIdentity.Claims;
            return claims.Where(c => c.Type == ClaimTypes.Role).Select(_ => _.Value).ToList();
        }
    }
}
