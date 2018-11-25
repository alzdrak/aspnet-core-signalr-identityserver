using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using server.Hubs;
using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace server.Configuration
{
    public class ResourceOwnerPasswordValidator: IResourceOwnerPasswordValidator
    {
        //logger
        private readonly ILogger<ProfileService> _logger;

        //context (used for tracking ip)
        private readonly IHttpContextAccessor _httpContextAccessor;

        //services
        private ApplicationUserManager _userManager = null;
        private SignInManager<ApplicationUser> _signInManager = null;

        //signalr hubs
        private readonly IHubContext<ChatHub> _chatHub;

        public ResourceOwnerPasswordValidator(ILogger<ProfileService> logger,
            IHttpContextAccessor httpContextAccessor,
            ApplicationUserManager userManager,
            SignInManager<ApplicationUser> signInManager,
            IHubContext<ChatHub> chatHub)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
            _chatHub = chatHub;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                //specific client trying to validate
                string clientId = context.Request.Raw["client_id"];

                //depending on client
                if (clientId == "client")
                {
                    //check user credentials 
                    var result = await _signInManager.PasswordSignInAsync(
                        context.UserName, context.Password, false, false);

                    //get client ip address
                    var clientIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                    //see if user was successfully logged in
                    if (result.Succeeded)
                    {
                        //get the user from database
                        var user = await _userManager.FindByNameAsync(context.UserName);

                        //return user data
                        context.Result = new GrantValidationResult(
                            subject: user.Id,
                            authenticationMethod: "custom",
                            claims: GetClaims(user, clientIp));

                        //notify all other clients of new user
                        await _chatHub.Clients.All.SendAsync("NewUser", user.UserName);

                        return;
                    }
                    else
                    {
                        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password.");
                    }
                }
            }
            catch (Exception ex)
            {
                //log error on username
                _logger.LogError(ex, ex.Message, context.UserName);

                //return
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Error occured during sign in. Please try again.");
            }
        }

        /// <summary>
        /// Gets all significant user claims that should be included
        /// </summary>
        /// <param name="user">Identity User</param>
        /// <returns>Array of User Claims</returns>
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

