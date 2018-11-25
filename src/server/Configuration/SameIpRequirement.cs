using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Configuration
{
    public class SameIpHandler : AuthorizationHandler<SameIpRequirement>
    {
        //context (used for ip)
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SameIpHandler(IHttpContextAccessor httpContextAccessor) : base()
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SameIpRequirement requirement)
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

    public class SameIpRequirement : IAuthorizationRequirement {
        public SameIpRequirement()  { }
    }
}
