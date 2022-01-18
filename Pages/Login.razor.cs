using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AspNetCoreAuthMultiLang.Pages
{
    public partial class Login
    {
        string login = "admin", password = "admin";

        [Inject] NavigationManager i_navigationManager { get; set; }
        [Inject] IDBAbstraction i_db { get; set; }

        private async Task SubmitAsync()
        {
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
            {
                if (i_db.IsUserPasswordOk(login, password))
                {
                    await DoLoginAsync(login, i_navigationManager.Uri.Substring(i_navigationManager.BaseUri.Length-1));
                }
            }
        }
    }
}