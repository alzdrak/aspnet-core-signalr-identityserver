using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace server.Configuration
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("ip", new[] { "ip_address" } )
        };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("server", "Server")
                {
                    ApiSecrets =
                    {
                        new Secret("ServerSecret".Sha256())
                    },
                    Description = "Server API.",
                    DisplayName = "Server API",
                    Enabled = true,
                    Scopes =
                    {
                        new Scope("ip_address")
                        {
                            UserClaims = { "ip_address" }
                        }
                    },
                    UserClaims =
                    {
                        JwtClaimTypes.Id,
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Email,
                        JwtClaimTypes.Role,
                        "ip_address",
                    }
                }
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                // resource owner password grant client
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "server",
                        "ip_address"
                    },
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:3000"
                    },
                    AllowOfflineAccess = true,
                    UpdateAccessTokenClaimsOnRefresh = true
                },
            };
        }
    }
}
