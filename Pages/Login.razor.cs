using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AspNetCoreAuthMultiLang.Pages
{
    public partial class Login
    {
        string login = "admin", password = "admin";

        [Inject] NavigationManager i_navigationManager { get; set; }
        [Inject] DBAbstraction i_db { get; set; }

        private async Task Submit()
        {
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
            {
                if (i_db.IsUserPasswordOk(login, password))
                {
                    // auth.m_jwt = await SessionToken.CreateAsync(i_db, i_config, login, "User", i_db.GetUserId(login));
                    await _masp.DoLoginAsync(login, i_navigationManager.Uri.Substring(i_navigationManager.BaseUri.Length-1));
                }
            }
        }
    }
}