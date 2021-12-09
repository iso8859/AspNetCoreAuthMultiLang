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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await _masp.TryLoginAsync();
            }
        }
    }
}
