using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace server.Configuration
{
    public class ProfileService : IProfileService
    {
        //logger
        private readonly ILogger<ProfileService> _logger;

        //context (used for tracking ip)
        private readonly IHttpContextAccessor _httpContextAccessor;

        //services
        private ApplicationUserManager _userManager = null;
        private SignInManager<ApplicationUser> _signInManager = null;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;

        public ProfileService(ILogger<ProfileService> logger,
            IHttpContextAccessor httpContextAccessor,
            ApplicationUserManager userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
            _claimsFactory = claimsFactory;
        }

        /// <summary>
        /// Get user profile data
        /// </summary>
        /// <param name="context">Change IssuedClaims state by assigning user info claims to it</param>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //check if correct client is chosen for getting profile data
                if (context.Client.ClientId == "client")
                {
                    //get client ip address
                    var clientIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                    //get user id from claims
                    //var userIdClaim = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");
                    var userId = context.Subject.GetSubjectId();

                    //if user id is an actual guid
                    if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out var output))
                    {
                        //get user
                        var user = await _userManager.FindByIdAsync(userId);

                        //if the user exists, issue the claims
                        if (user != null)
                        {
                            //get all claims
                            var claims = GetClaims(user, clientIp);

                            //set claims to context
                            context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                            //context.IssuedClaims = claims2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// Check if user is active
        /// </summary>
        /// <param name="context">Used context to change IsActive state</param>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                //check if correct client is chosen for checking user state
                if (context.Client.ClientId == "client")
                {
                    //get user id from claims
                    var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "user_id");

                    if (!string.IsNullOrEmpty(userId?.Value) && long.Parse(userId.Value) > 0)
                    {
                        //get user
                        var user = await _userManager.FindByIdAsync(userId.Value);

                        //if the user exists
                        if (user != null)
                        {
                            if (user != null)
                            {
                                context.IsActive = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //log error
                _logger.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// Gets all user claims that should be included in token auth requests
        /// </summary>
        /// <param name="user">Identity Application User</param>
        /// <returns>Array of Claims</returns>
        private static Claim[] GetClaims(ApplicationUser user, string ip)
        {
            return new Claim[]
            {
                new Claim(JwtClaimTypes.Id, user.Id),
                new Claim(JwtClaimTypes.Name, user.UserName),
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim("ip_address", ip),

                //hard claim
                new Claim(JwtClaimTypes.Role, "User")
            };
        }
    }
}
