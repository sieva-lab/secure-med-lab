using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace SecureMed.SharedKernel.Modules;

public static class ModuleExtensions
{
    private static readonly IModule[] s_modules = DiscoverModules();

    public static WebApplicationBuilder AddModules(this WebApplicationBuilder services)
    {
        foreach (var module in s_modules)
        {
            module.AddModule(services);
        }

        return services;
    }

    public static WebApplication UseModules(this WebApplication app)
    {
        foreach (var module in s_modules)
        {
            module.UseModule(app);
        }

        return app;
    }

    private static IModule[] DiscoverModules()
    {
        // Scannen we alleen geladen assemblies die beginnen met SecureMed
        var modules = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName != null && a.FullName.StartsWith("SecureMed.", StringComparison.Ordinal))
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            // Filter op unieke naam zodat we nooit dezelfde module 2x laden
            .GroupBy(t => t.FullName)
            .Select(g => g.First())
            .Select(t => (IModule)Activator.CreateInstance(t)!)
            .ToArray();

        return modules;
    }
}
