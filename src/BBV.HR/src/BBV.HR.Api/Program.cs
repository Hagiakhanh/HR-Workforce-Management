using BBV.HR.Api.Extensions;
using BBV.HR.Application;
using BBV.HR.Infrastructure;

namespace BBV.HR.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add Controllers & ProblemDetails / Exception Handling
        builder.Services.AddControllers();
        builder.Services.AddProblemDetailsAndExceptionHandler();

        // Register Swagger & API Explorer
        builder.Services.AddSwaggerDocumentation();

        // Register Application & Infrastructure Layer Dependencies
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);

        builder.Services.AddAuthorization();

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
        app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

        await app.RunAsync();
    }
}
