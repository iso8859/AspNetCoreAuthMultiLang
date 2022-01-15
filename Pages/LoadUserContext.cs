using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

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

        bool bLogout;
        string login, url;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await _masp.TryLoginAsync();
            }
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(url))
            {
                await _masp.DoLoginAsync(login, url);
                login = null;
                url = null;
            }
            if (bLogout)
            {
                await _masp.DoLogoutAsync();
                bLogout= false;
            }
            if (_masp.IsAuthenticated())
            {
                await OnAfterRenderContextAsync(firstRender);
            }
        }

        // In your page override this method to be sure user is authenticated
        public virtual Task OnAfterRenderContextAsync(bool firstRender)
        {
            return Task.CompletedTask;
        }

        public void DoLogin(string login, string url)
        {
            this.login = login;
            this.url = url;
            StateHasChanged();
        }
        public void DoLogout()
        {
            bLogout = true;
            StateHasChanged();
        }
    }
}
