using LOTS3.Models;
using LOTS3.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Mindscape.Raygun4Net.AspNetCore;
using System;
namespace LOTS3
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
            // Add services.AddRaygun(Configuration); to the ConfigureServices method
            services.AddRaygun(Configuration);
            services.AddMvc();
            //Authentication
            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options => Configuration.Bind("AzureAd", options));
            ////Authorization
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("PARCS_LOTS", p =>
            //    {
            //        p.RequireClaim("groups", "0a6d86be-5248-4db4-b732-f0a85360e530");
            //    });
            //});

            //services.AddControllersWithViews(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //});

            services.AddRazorPages().AddMvcOptions(options => {}).AddMicrosoftIdentityUI();

            services.AddDbContext<AppDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("PermitteeDBConnection")));
            //services.AddDbContext<AppDbContext>(options =>
            //{
            //    options.UseSqlServer(Configuration.GetConnectionString("PermitteeDBConnection"));
            //    options.EnableSensitiveDataLogging();
            //});

            //Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //Identity password complexity
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 2;
                //options.SignIn.RequireConfirmedEmail = true;
                //options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";\
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
            //.AddTokenProvider<CustomEmailConfirmationTokenProvider
            //    <ApplicationUser>>("CustomEmailConfirmation");

            // Changes token lifespan of all token types
            services.Configure<DataProtectionTokenProviderOptions>(o =>
                        o.TokenLifespan = TimeSpan.FromHours(5));

            //// Changes token lifespan of just the Email Confirmation Token type
            //services.Configure<CustomEmailConfirmationTokenProviderOptions>(o =>
            //            o.TokenLifespan = TimeSpan.FromDays(3));
            
            services.AddMvc(options =>{
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            //services.AddAuthentication()
            //    .AddGoogle(options =>
            //    {
            //        options.ClientId = "YOUR_GOOGLE_CLIENT_ID";
            //        options.ClientSecret = "YOUR_GOOGLE_CLIENT_SECRET";
            //        //options.CallbackPath = "";
            //    })
            //    .AddFacebook(options =>
            //    {
            //        options.AppId = "YOUR_FACEBOOK_APP_ID";
            //        options.AppSecret = "YOUR_FACEBOOK_APP_SECRET";
            //    });
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CreateRolePolicy",
                    //policy => policy.RequireClaim("Delete Role", "true"));
                    //.RequireClaim("Create Role"));
                    policy => policy.RequireAssertion(context =>
                    context.User.IsInRole("Administrator") &&
                    context.User.HasClaim(claim => claim.Type == "Create Role" && claim.Value == "true")
                    //||context.User.IsInRole("Super Administrator")
                    ));
                options.AddPolicy("DeleteRolePolicy",
                    //policy => policy.RequireClaim("Delete Role", "true"));
                    //.RequireClaim("Create Role"));
                    policy => policy.RequireAssertion(context =>
                    context.User.IsInRole("Administrator") &&
                    context.User.HasClaim(claim => claim.Type == "Delete Role" && claim.Value == "true")
                    //||context.User.IsInRole("Super Administrator")
                    ));
                options.AddPolicy("EditRolePolicy",
                    policy => policy.RequireAssertion(context =>
                    context.User.IsInRole("Administrator") &&
                    context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true")
                    //||context.User.IsInRole("Super Administrator")
                    ));
                //options.AddPolicy("EditRolePolicy",
                //    policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                //options.InvokeHandlersAfterFailure = false;
                options.AddPolicy("AdminRolePolicy",
                    policy => policy.RequireRole("Administrator"));
                options.FallbackPolicy = options.DefaultPolicy;
            });
            services.AddScoped<IPermitteeRepository, SQLPermitteeRepository>();
            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdministratorHandler>();
            services.AddSingleton<DataProtectionPurposeStrings>();
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHsts();
                app.UseRaygun();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseRaygun();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
