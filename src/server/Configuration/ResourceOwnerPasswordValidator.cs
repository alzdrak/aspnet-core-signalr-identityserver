using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
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

        //services
        private UserManager<ApplicationUser> _userManager = null;
        private SignInManager<ApplicationUser> _signInManager = null;

        //signalr hubs
        private readonly IHubContext<ChatHub> _chatHub;

        public ResourceOwnerPasswordValidator(ILogger<ProfileService> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IHubContext<ChatHub> chatHub)
        {
            _logger = logger;

            _userManager = userManager;
            _signInManager = signInManager;
            _chatHub = chatHub;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                //specific client trying to validate
                string client_id = context.Request.Raw["client_id"];

                //depending on client
                if (client_id == "client")
                {
                    //check user credentials 
                    var result = await _signInManager.PasswordSignInAsync(
                        context.UserName, context.Password, false, false);

                    //see if user was successfully logged in
                    if (result.Succeeded)
                    {
                        //get the user from database
                        var user = await _userManager.FindByNameAsync(context.UserName);

                        //return user data
                        context.Result = new GrantValidationResult(
                            subject: user.Id,
                            authenticationMethod: "custom",
                            claims: GetClaims(user));

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

