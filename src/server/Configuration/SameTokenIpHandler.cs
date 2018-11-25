using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Configuration
{
    /// <summary>
    /// Checks that the Token Ip matches current client Ip. Increased security for JWT.
    /// </summary>
    public class SameTokenIpHandler : AuthorizationHandler<SameTokenIpRequirement>
    {
        //context (used for ip)
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SameTokenIpHandler(IHttpContextAccessor httpContextAccessor) : base()
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SameTokenIpRequirement requirement)
        {
            //get accessor client ip
            var clientIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            //token issued ip
            var tokenIp = context.User.FindFirst("ip_address")?.Value;

            //if tokenIp matches current clientIp
            if (clientIp.Equals(tokenIp))
                context.Succeed(requirement);
            else
                context.Fail();

            return Task.CompletedTask;
        }
    }

    public class SameTokenIpRequirement : IAuthorizationRequirement {
        public SameTokenIpRequirement()  { }
    }
}
