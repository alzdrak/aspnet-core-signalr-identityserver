using IdentityModel;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace server.Configuration
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        //context (used for tracking ip)
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<ApplicationUserManager> logger,
            IHttpContextAccessor httpContextAccessor/*, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory*/) :
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors,
                services, logger)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get all claims for users
        /// </summary>
        /// <param name="user">User Object</param>
        /// <returns></returns>
        public override async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            //get current user ip
            var clientIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            //get all current claims
            var claims = await base.GetClaimsAsync(user);

            //add new dynamic ip address claim (expires with the token)
            claims.Add(new Claim("ip_address", clientIp));

            //return all claims
            return claims;
        }
    }
}
