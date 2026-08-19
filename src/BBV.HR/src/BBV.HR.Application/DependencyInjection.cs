using BBV.HR.Application.Interfaces.Services;
using BBV.HR.Application.Services;
using BBV.HR.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BBV.HR.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IProjectMemberService, ProjectMemberService>();
        services.AddScoped<IProjectEffortService, ProjectEffortService>();
        services.AddScoped<ICapabilityService, CapabilityService>();

        // Register FluentValidation Validators
        services.AddValidatorsFromAssemblyContaining<CreateProjectDtoValidator>();

        return services;
    }
}
