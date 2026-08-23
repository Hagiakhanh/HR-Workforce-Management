using Microsoft.AspNetCore.Mvc;

namespace BBV.HR.Api.Models;

/// <summary>
/// Model for form binding in Swagger UI for OpenIddict token endpoint requests.
/// </summary>
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
