using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace JwtAuthDemo.Helpers
{
    public class MyTokenOptions
    {
    }

    public class MyCustomTokenAuthOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScemeName = "bearer";
        public string CusTooken { get; set; }
    }

    public class MyCustomTokenAuthHandler : AuthenticationHandler<MyCustomTokenAuthOptions>
    {
        public MyCustomTokenAuthHandler(IOptionsMonitor<MyCustomTokenAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Context.Request.Headers.TryGetValue("Authorization", out StringValues token))
            {
                // 切割`Authorization`標頭的內容，這是為了切割Authorization的規格與內容
                var tokenInfo = token[0].Split(' ');
                // 切割後長度必須為2(因為依照格式來切割恰好會等於兩個，當然這邊可以自己決定)
                // 如果長度等於2且格式為`bearer`時處理Token
                if ((tokenInfo.Length == 2
                       && Options.CusTooken.Equals(tokenInfo[1], StringComparison.CurrentCultureIgnoreCase)
                     )
                   )
                {
                    var username = "hal";
                    var claims = new[] {
                                    new Claim(ClaimTypes.NameIdentifier, username),
                                    new Claim(ClaimTypes.Name, username),
                        // add other claims/roles as you like
                     };
                        var id = new ClaimsIdentity(claims, Scheme.Name);
                        var principal = new ClaimsPrincipal(id);
                        var ticket = new AuthenticationTicket(principal, Scheme.Name);
                        return Task.FromResult(AuthenticateResult.Success(ticket));
                }
            }

            return Task.FromResult(AuthenticateResult.Fail($"TokenError"));





            //if (!Request.Headers.ContainsKey(Options.TokenHeaderName))
            //    return Task.FromResult(AuthenticateResult.Fail($"Missing Header For Token: {Options.TokenHeaderName}"));

            // var token = Request.Headers[Options.TokenHeaderName];
            // get username from db or somewhere else accordining to this token

        }
    }


}
