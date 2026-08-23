using BBV.HR.Application.Interfaces.Repositories;
using BBV.HR.Application.Interfaces.Services;
using BBV.HR.Infrastructure.Auth;
using BBV.HR.Infrastructure.Data;
using BBV.HR.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BBV.HR.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options
                .UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null);
                })
                .UseSnakeCaseNamingConvention();

            options.UseOpenIddict();
        });

        // Register OpenIddict
        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                       .UseDbContext<ApplicationDbContext>();
            })
            .AddServer(options =>
            {
                options.SetTokenEndpointUris("/connect/token")
                       .SetAuthorizationEndpointUris("/connect/authorize")
                       .SetUserInfoEndpointUris("/connect/userinfo");

                options.AllowClientCredentialsFlow()
                       .AllowPasswordFlow()
                       .AllowAuthorizationCodeFlow()
                       .RequireProofKeyForCodeExchange()
                       .AllowRefreshTokenFlow();

                // Allow password flow without requiring a client_id
                options.AcceptAnonymousClients();

                // Register supported scopes
                options.RegisterScopes(
                    OpenIddict.Abstractions.OpenIddictConstants.Scopes.OpenId,
                    OpenIddict.Abstractions.OpenIddictConstants.Scopes.Profile,
                    OpenIddict.Abstractions.OpenIddictConstants.Scopes.Email,
                    OpenIddict.Abstractions.OpenIddictConstants.Scopes.Roles,
                    OpenIddict.Abstractions.OpenIddictConstants.Scopes.OfflineAccess,
                    "api");

                // Format as standard JWT (unencrypted payload)
                options.DisableAccessTokenEncryption();

                // Development signing & encryption certs
                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                options.UseAspNetCore()
                       .EnableTokenEndpointPassthrough()
                       .EnableAuthorizationEndpointPassthrough()
                       .EnableUserInfoEndpointPassthrough()
                       .DisableTransportSecurityRequirement();
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });

        // Register Auth Service
        services.AddScoped<IAuthService, AuthService>();

        // Register Background Worker to Seed OpenIddict Clients
        services.AddHostedService<OpenIddictWorker>();

        // Register Repositories
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();
        services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ICapabilityRepository, CapabilityRepository>();
        services.AddScoped<IEmployeeCapabilityRepository, EmployeeCapabilityRepository>();

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
