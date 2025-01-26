using Infinit.Assessment.Services.Contracts;
using Infinit.Assessment.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Infinit.Assessment.Services;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IGithubService, GithubService>();
        services.AddScoped<IAnalysisService, AnalysisService>();
    }
}
