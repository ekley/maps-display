using MapsDisplay.Client.Services;
using MapsDisplay.Components.Account;
using MapsDisplay.Features.LocalAuthority.Data;
using MapsDisplay.Features.LocalAuthority.Services.Implementations;
using MapsDisplay.Features.LocalAuthority.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Services.Interfaces;

namespace MapsDisplay.Configurations
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRazorComponents()
                    .AddInteractiveServerComponents()
                    .AddInteractiveWebAssemblyComponents();

            services.AddControllers();
            services.AddHttpClient();
            services.AddCascadingAuthenticationState();
            services.AddScoped<IdentityUserAccessor>();
            services.AddScoped<IdentityRedirectManager>();
            services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
            services.AddScoped<ILocalAuthorityApiService, LocalAuthorityApiService>();
            services.AddScoped<ILocalAuthorityService, LocalAuthorityService>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = configuration["Authentication:Microsoft:ClientId"];
                microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
                microsoftOptions.AuthorizationEndpoint = configuration["Authentication:Microsoft:AuthorizationEndpoint"];
                microsoftOptions.TokenEndpoint = configuration["Authentication:Microsoft:TokenEndpoint"];
            })
            .AddIdentityCookies();
        }

        public static void AddCustomDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();
        }

        public static void AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddSignInManager()
                    .AddDefaultTokenProviders();
        }

        public static void AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });
        }

        public static void AddEmailSender(this IServiceCollection services)
        {
            services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
        }
    }
}
