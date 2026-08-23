using BBV.HR.Api.Extensions;
using BBV.HR.Application;
using BBV.HR.Infrastructure;

namespace BBV.HR.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();

        // Add Controllers & ProblemDetails / Exception Handling
        builder.Services.AddControllers();
        builder.Services.AddProblemDetailsAndExceptionHandler();

        // Register Swagger & API Explorer
        builder.Services.AddSwaggerDocumentation();

        // Register Application & Infrastructure Layer Dependencies
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);

        // Configure Authentication with OpenIddict Validation
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });

        // Configure Role-based Authorization Policies
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole(BBV.HR.Application.Common.Constants.Roles.Admin));
            options.AddPolicy("ManagerOrAdmin", policy =>
                policy.RequireRole(BBV.HR.Application.Common.Constants.Roles.Admin, BBV.HR.Application.Common.Constants.Roles.Manager));
            options.AddPolicy("EmployeeAccess", policy =>
                policy.RequireRole(
                    BBV.HR.Application.Common.Constants.Roles.Admin,
                    BBV.HR.Application.Common.Constants.Roles.Manager,
                    BBV.HR.Application.Common.Constants.Roles.Employee));
        });

        var app = builder.Build();

        // Seed Sample Data into PostgreSQL Database
        await app.SeedDatabaseAsync();

        // Use Standard ASP.NET Core Exception Handler (ProblemDetails RFC 7807)
        app.UseExceptionHandler();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BBV.HR API v1");
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapDefaultEndpoints();
        app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

        await app.RunAsync();
    }
}
