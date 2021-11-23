using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using AspNetCoreAuthMultiLang;
using AspNetCoreAuthMultiLang.Shared;

namespace AspNetCoreAuthMultiLang.Pages
{
    public partial class Login
    {
        string login, password;
        string authState;
        protected override void OnInitialized()
        {
            myServerAuthenticationStateProvider asp = (myServerAuthenticationStateProvider)i_authenticationStateProvider;
            if (asp.IsAuthenticated())
                authState = "You are user " + asp.m_userId;
            else
                authState = "You are not authenticated";
        }

        private async Task Submit()
        {
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
            {
                if (i_db.IsUserPasswordOk(login, password))
                {
                    var t = await SessionToken.CreateAsync(i_db, i_config, login, "User", i_db.GetUserId(login));
                    myServerAuthenticationStateProvider asp = (myServerAuthenticationStateProvider)i_authenticationStateProvider;
                    await asp.DoLoginAsync(i_db.GetUserId(login));
                    // asp.ApplyCulture("fr-FR", "/");
                }
            }
        }
    }
}