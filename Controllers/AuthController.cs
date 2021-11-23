using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAuthMultiLang.Controllers
{
    [Route("/api/v1/auth")]
    public class AuthController : Controller
    {
        /// <summary>
        /// Set current user culture
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="redirectionUri"></param>
        /// <returns></returns>
        [HttpGet("setCulture")]
        public IActionResult SetCulture([FromQuery] string culture, [FromQuery] string redirectionUri)
        {
            if (culture != null)
            {
                HttpContext.Response.Cookies.Append(
                    Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.DefaultCookieName,
                    Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.MakeCookieValue(
                        new Microsoft.AspNetCore.Localization.RequestCulture(culture)));
            }
            return LocalRedirect(redirectionUri);
        }
    }
}
