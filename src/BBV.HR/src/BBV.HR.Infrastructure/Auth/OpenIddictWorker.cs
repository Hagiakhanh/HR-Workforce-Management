using BBV.HR.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace BBV.HR.Infrastructure.Auth;

public class OpenIddictWorker : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OpenIddictWorker> _logger;

    public OpenIddictWorker(IServiceProvider serviceProvider, ILogger<OpenIddictWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

        // 1. Seed Scopes
        if (await scopeManager.FindByNameAsync("api", cancellationToken) is null)
        {
            await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = "api",
                DisplayName = "BBV HR Core API Scope",
                Resources = { "bbv_hr_resource" }
            }, cancellationToken);
        }

        if (await scopeManager.FindByNameAsync(Scopes.Roles, cancellationToken) is null)
        {
            await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = Scopes.Roles,
                DisplayName = "Roles Scope",
                Resources = { "bbv_hr_resource" }
            }, cancellationToken);
        }

        // 2. Seed SPA Client (Authorization Code + PKCE)
        var spaDescriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = "bbv-hr-spa",
            ClientType = ClientTypes.Public,
            DisplayName = "BBV HR Single Page Application",
            RedirectUris =
            {
                new Uri("http://localhost:3000/callback"),
                new Uri("https://localhost:3000/callback"),
                new Uri("http://localhost:5173/callback"),
                new Uri("https://localhost:5173/callback"),
                new Uri("https://oauth.pstmn.io/v1/callback")
            },
            PostLogoutRedirectUris =
            {
                new Uri("http://localhost:3000/"),
                new Uri("https://localhost:3000/"),
                new Uri("http://localhost:5173/"),
                new Uri("https://localhost:5173/")
            },
            Permissions =
            {
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Token,
                Permissions.Endpoints.EndSession,
                Permissions.Endpoints.Revocation,
                Permissions.GrantTypes.Password,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.ResponseTypes.Code,
                Permissions.Scopes.Email,
                Permissions.Scopes.Profile,
                Permissions.Scopes.Roles,
                Permissions.Prefixes.Scope + "api"
            },
            Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            }
        };

        var spaClient = await manager.FindByClientIdAsync("bbv-hr-spa", cancellationToken);
        if (spaClient is null)
        {
            await manager.CreateAsync(spaDescriptor, cancellationToken);
            _logger.LogInformation("OpenIddict client 'bbv-hr-spa' registered.");
        }
        else
        {
            await manager.UpdateAsync(spaClient, spaDescriptor, cancellationToken);
            _logger.LogInformation("OpenIddict client 'bbv-hr-spa' updated.");
        }

        // 3. Seed Service Client (Client Credentials)
        var serviceClient = await manager.FindByClientIdAsync("bbv-hr-service", cancellationToken);
        if (serviceClient is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "bbv-hr-service",
                ClientSecret = "bbv-service-secret-key-12345",
                ClientType = ClientTypes.Confidential,
                DisplayName = "BBV HR Backend Service Client",
                Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.ClientCredentials,
                    Permissions.Prefixes.Scope + "api"
                }
            }, cancellationToken);
            _logger.LogInformation("OpenIddict client 'bbv-hr-service' registered.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
