﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using server.Configuration;
using server.Data;
using server.Hubs;
using server.Models;

namespace server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //database connection
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //aspnet identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                //.AddUserStore<UserStore<ApplicationUser>()
                //.AddRoleStore<IRoleStore<>()
                .AddUserManager<ApplicationUserManager>()
                //.AddRoleManager<RoleManager<>()
                .AddDefaultTokenProviders();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<ProfileService>();
                //.AddTestUsers(Config.GetUsers());

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(x =>
                {
                    x.Authority = Configuration.GetSection("IdentityServerSettings:Host").Value;
                    //x.AllowedScopes = new List<string> { "Api" };
                    x.ApiSecret = "ServerSecret";
                    x.ApiName = "server";
                    x.SupportedTokens = SupportedTokens.Both;
                    x.RequireHttpsMetadata = false;
                    x.RoleClaimType = "role";

                    //change this to true for SLL
                    x.RequireHttpsMetadata = false;
                });

            //httpcontext
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<ApplicationUserManager>();

            //extra auth policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SameTokenIp", policy => policy.Requirements.Add(new SameTokenIpRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, SameTokenIpHandler>();


            //resource owner setup for identity server
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();


            //add signalr
            services.AddSignalR(config => config.EnableDetailedErrors = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            //include signalr routes
            app.UseSignalR(routers => routers.MapHub<ChatHub>("/hubs/chat"));

            //app.UseAuthentication();
            app.UseIdentityServer();

            app.UseMvc();
        }
    }
}
