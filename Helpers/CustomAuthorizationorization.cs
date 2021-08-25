using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
 

namespace JwtAuthDemo.Helpers
{

    public class CustomAuthorizationorization : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                if (context.HttpContext.User.Identity.Name == "halkang") //黑名單
                {
                    context.HttpContext.Response.WriteAsync($"{context.HttpContext.User.Identity.Name}是黑名單 \r\n");
                }

            }
        }

    }

}
