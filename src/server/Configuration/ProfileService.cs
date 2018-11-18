using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
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

        //services
        private UserManager<ApplicationUser> _userManager = null;
        private SignInManager<ApplicationUser> _signInManager = null;

        public ProfileService(ILogger<ProfileService> logger, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;

            _userManager = userManager;
            _signInManager = signInManager;
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
                    //get user id from claims
                    var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");

                    //if user id is an actual id greater than 0
                    if (!string.IsNullOrEmpty(userId?.Value) && long.Parse(userId.Value) > 0)
                    {
                        //get user
                        var user = await _userManager.FindByIdAsync(userId.Value);

                        //if the user exists, issue the claims
                        if (user != null)
                        {
                            var claims = GetClaims(user);

                            //set claims to context
                            context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
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
        private static Claim[] GetClaims(ApplicationUser user)
        {
            return new Claim[]
            {
                new Claim(JwtClaimTypes.Id, user.Id),
                new Claim(JwtClaimTypes.Name, user.UserName),
                new Claim(JwtClaimTypes.Email, user.Email),

                //hard claim
                new Claim(JwtClaimTypes.Role, "User")
            };
        }
    }
}
