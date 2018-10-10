using System;
using System.Text;
using System.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace Musiquizza_React {
    
    [EnableCors("AllowAllOrigins")]
    [Route("api/Authentication")]
    
    public class AuthenticationController: Controller {

        [HttpGet]
        public IActionResult SignIn()
        {
            // Instruct the middleware corresponding to the requested external identity
            // provider to redirect the user agent to its own authorization endpoint.
            // Note: the authenticationScheme parameter must match the value configured in Startup.cs
            return Challenge(new AuthenticationProperties { RedirectUri = "/game", IsPersistent = true, AllowRefresh = true }, "Spotify");
        }

        [HttpGet("signout")]
        public IActionResult SignOut()
        {
            // Instruct the cookies middleware to delete the local cookie created
            // when the user agent is redirected from the external identity provider
            // after a successful authentication flow (e.g Google or Facebook).
            return SignOut(new AuthenticationProperties { RedirectUri = "/" },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [Authorize]
        [HttpGet("token")]
        public async Task<string> Token() {

            return await HttpContext.GetTokenAsync("Spotify", "access_token");

        }

    }
}