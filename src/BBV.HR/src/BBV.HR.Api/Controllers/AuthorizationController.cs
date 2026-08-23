using System.Security.Claims;
using BBV.HR.Application.Interfaces.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace BBV.HR.Api.Controllers;

[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictScopeManager _scopeManager;
    private readonly IAuthService _authService;

    public AuthorizationController(
        IOpenIddictApplicationManager applicationManager,
        IOpenIddictScopeManager scopeManager,
        IAuthService authService)
    {
        _applicationManager = applicationManager;
        _scopeManager = scopeManager;
        _authService = authService;
    }

    [HttpPost("~/connect/token")]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    [Consumes("application/x-www-form-urlencoded")]
    [Produces("application/json")]
    public async Task<IActionResult> Exchange([FromForm] OpenIddictTokenRequestModel? requestModel = null)
    {
        var request = HttpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (request.IsClientCredentialsGrantType())
        {
            return await HandleClientCredentialsFlow(request);
        }

        if (request.IsPasswordGrantType())
        {
            return await HandlePasswordFlow(request);
        }

        if (request.IsAuthorizationCodeGrantType() || request.IsRefreshTokenGrantType())
        {
            return await HandleCodeOrRefreshTokenFlow();
        }

        return BadRequest(new OpenIddictResponse
        {
            Error = Errors.UnsupportedGrantType,
            ErrorDescription = "The specified grant type is not supported."
        });
    }

    private async Task<IActionResult> HandleClientCredentialsFlow(OpenIddictRequest request)
    {
        var application = await _applicationManager.FindByClientIdAsync(request.ClientId!)
            ?? throw new InvalidOperationException("The application details cannot be found in the database.");

        var identity = new ClaimsIdentity(
            authenticationType: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            nameType: Claims.Name,
            roleType: Claims.Role);

        identity.SetClaim(Claims.Subject, await _applicationManager.GetClientIdAsync(application));
        identity.SetClaim(Claims.Name, await _applicationManager.GetDisplayNameAsync(application));

        identity.SetDestinations(claim => [Destinations.AccessToken]);

        var principal = new ClaimsPrincipal(identity);
        principal.SetScopes(request.GetScopes());
        principal.SetResources("bbv_hr_resource");

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private async Task<IActionResult> HandlePasswordFlow(OpenIddictRequest request)
    {
        var employee = await _authService.ValidateCredentialsAsync(request.Username!, request.Password!);
        if (employee is null)
        {
            var properties = new AuthenticationProperties(new Dictionary<string, string?>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The username or password is invalid."
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var principal = CreatePrincipalForEmployee(employee, request.GetScopes());
        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private async Task<IActionResult> HandleCodeOrRefreshTokenFlow()
    {
        var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        if (!result.Succeeded || result.Principal is null)
        {
            return Forbid(new AuthenticationProperties(new Dictionary<string, string?>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The token is no longer valid."
            }), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var subject = result.Principal.GetClaim(Claims.Subject);
        if (Guid.TryParse(subject, out var employeeId))
        {
            var employee = await _authService.GetEmployeeByIdAsync(employeeId);
            if (employee is null)
            {
                return Forbid(new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer active."
                }), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            var principal = CreatePrincipalForEmployee(employee, result.Principal.GetScopes());
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        return SignIn(result.Principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [HttpGet("~/connect/userinfo")]
    [HttpPost("~/connect/userinfo")]
    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme + "," + "OpenIddict.Validation.AspNetCore")]
    [Produces("application/json")]
    public async Task<IActionResult> Userinfo()
    {
        var subject = User.GetClaim(Claims.Subject);
        if (string.IsNullOrEmpty(subject))
        {
            return Challenge(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        if (Guid.TryParse(subject, out var employeeId))
        {
            var employee = await _authService.GetEmployeeByIdAsync(employeeId);
            if (employee is null)
            {
                return NotFound();
            }

            return Ok(new Dictionary<string, object>
            {
                [Claims.Subject] = employee.Id.ToString(),
                [Claims.Email] = employee.Email,
                [Claims.Name] = $"{employee.FirstName} {employee.LastName}".Trim(),
                [Claims.GivenName] = employee.FirstName,
                [Claims.FamilyName] = employee.LastName,
                [Claims.Role] = employee.Role,
                ["employee_code"] = employee.EmployeeCode
            });
        }

        return Ok(new Dictionary<string, object>
        {
            [Claims.Subject] = subject,
            [Claims.Name] = User.GetClaim(Claims.Name) ?? subject
        });
    }

    private static ClaimsPrincipal CreatePrincipalForEmployee(BBV.HR.Application.Entities.Employee employee, IEnumerable<string> scopes)
    {
        var identity = new ClaimsIdentity(
            authenticationType: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            nameType: Claims.Name,
            roleType: Claims.Role);

        identity.SetClaim(Claims.Subject, employee.Id.ToString());
        identity.SetClaim(Claims.Email, employee.Email);
        identity.SetClaim(Claims.Name, $"{employee.FirstName} {employee.LastName}".Trim());
        identity.SetClaim(Claims.GivenName, employee.FirstName);
        identity.SetClaim(Claims.FamilyName, employee.LastName);
        identity.SetClaim(Claims.Role, employee.Role);
        identity.SetClaim("employee_code", employee.EmployeeCode);

        // Set destinations: which claims are included in access token vs id token
        identity.SetDestinations(claim => claim.Type switch
        {
            Claims.Subject or Claims.Name or Claims.Role or Claims.Email or "employee_code" =>
                [Destinations.AccessToken, Destinations.IdentityToken],
            _ => [Destinations.AccessToken]
        });

        var principal = new ClaimsPrincipal(identity);
        principal.SetScopes(scopes);
        principal.SetResources("bbv_hr_resource");

        return principal;
    }
}

public class OpenIddictTokenRequestModel
{
    [FromForm(Name = "grant_type")]
    public string GrantType { get; set; } = "password";

    [FromForm(Name = "username")]
    public string? Username { get; set; } = "admin@bbv.vn";

    [FromForm(Name = "password")]
    public string? Password { get; set; } = "Password123!";

    [FromForm(Name = "scope")]
    public string? Scope { get; set; } = "openid profile email roles api";

    [FromForm(Name = "client_id")]
    public string? ClientId { get; set; } = "bbv-hr-spa";

    [FromForm(Name = "client_secret")]
    public string? ClientSecret { get; set; }

    [FromForm(Name = "refresh_token")]
    public string? RefreshToken { get; set; }

    [FromForm(Name = "code")]
    public string? Code { get; set; }
}
