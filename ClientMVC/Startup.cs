using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ClientMVC
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
            services.AddControllersWithViews();

            services.AddHttpClient();
            services.AddHttpContextAccessor();

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            if (Debugger.IsAttached)
            {
                services.AddMvc(opts => opts.RequireHttpsPermanent = false);
                IdentityModelEventSource.ShowPII = true;
            }

            services.AddAuthorization();
            services.AddAuthentication(
                o =>
                {
                    o.DefaultScheme = "Coockies";
                    o.DefaultChallengeScheme = "oidc";
                }
                )
                .AddCookie("Coockies")
                .AddOpenIdConnect(
                    "oidc", o => {
                        if (Debugger.IsAttached)
                            o.RequireHttpsMetadata = false;

                        o.Authority = "https://localhost:5001";
                        o.ClientId = "ClientMVC";
                        o.ClientSecret = "61476915fc7f45c98ff2cd99f3c843b7";
                        o.ResponseType = "code";
                        o.Scope.Add("profile");
                        o.Scope.Add("openid");
                        o.SaveTokens = true;
                        o.GetClaimsFromUserInfoEndpoint = true;

                        o.TokenValidationParameters = new TokenValidationParameters()
                        {
                            NameClaimType = "name",
                            RoleClaimType = "role"
                        };
                    }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
