using System.Reflection;
using Application.ServiceLifetimes;
using Domain.Extensions;
using Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Infrastructure.Extensions;

/// <summary>
/// Contains extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Scans the specified assemblies for all implementations of <see cref="IServiceInstaller"/> 
    /// and executes their <see cref="IServiceInstaller.Install"/> method, passing the provided 
    /// <see cref="IServiceCollection"/> and <see cref="IConfiguration"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register the services into.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> to provide configuration values.</param>
    /// <param name="assemblies">One or more assemblies to scan for <see cref="IServiceInstaller"/> implementations.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance to allow chaining.</returns>
    public static IServiceCollection InstallServicesFromAssemblies(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies
    )
    {
        IEnumerable<IServiceInstaller> serviceInstallers = InstanceFactory.CreateFromAssemblies<IServiceInstaller>(
            assemblies
        );
            
        serviceInstallers.ForEach(serviceInstaller => serviceInstaller.Install(services, configuration));
        
        return services;
    }

    /// <summary>
    /// Scans the specified assemblies for all implementations of <see cref="IModuleInstaller"/> 
    /// and executes their <see cref="IModuleInstaller.Install"/> method, passing the provided 
    /// <see cref="IServiceCollection"/> and <see cref="IConfiguration"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register the modules into.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> to provide configuration values.</param>
    /// <param name="assemblies">One or more assemblies to scan for <see cref="IModuleInstaller"/> implementations.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance to allow chaining.</returns>
    public static IServiceCollection InstallModulesFromAssemblies(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies
    )
    {
        IEnumerable<IModuleInstaller> modules = InstanceFactory.CreateFromAssemblies<IModuleInstaller>(
            assemblies
        );
            
        modules.ForEach(moduleInstaller => moduleInstaller.Install(services, configuration));

        return services;
    }
    
    /// <summary>
    /// Scans the specified assembly for types implementing <see cref="ITransient"/> and
    /// registers them as all their matching interfaces (by name) with a transient lifetime.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="assembly">The assembly to scan for transient services.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
    public static IServiceCollection AddTransientAsMatchingInterfaces(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        services.Scan(scan =>
            scan.FromAssemblies(assembly)
                .AddClasses(filter => filter.AssignableTo<ITransient>(), false)
                .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                .AsMatchingInterface()
                .WithTransientLifetime()
        );

        return services;
    }
    
    /// <summary>
    /// Scans the specified assembly for types implementing <see cref="IScoped"/> and
    /// registers them as all their matching interfaces (by name) with a scoped lifetime.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="assembly">The assembly to scan for scoped services.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
    public static IServiceCollection AddScopedAsMatchingInterfaces(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        services.Scan(scan =>
            scan.FromAssemblies(assembly)
                .AddClasses(filter => filter.AssignableTo<IScoped>(), false)
                .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                .AsMatchingInterface()
                .WithScopedLifetime()
        );

        return services;
    }

    /// <summary>
    /// Scans the specified assembly for types implementing <see cref="ISingleton"/> and
    /// registers them as all their matching interfaces (by name) with a singleton lifetime.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="assembly">The assembly to scan for singleton services.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
    public static IServiceCollection AddSingletonAsMatchingInterfaces(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        services.Scan(scan =>
            scan.FromAssemblies(assembly)
                .AddClasses(filter => filter.AssignableTo<ISingleton>(), false)
                .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                .AsMatchingInterface()
                .WithSingletonLifetime()
        );
        
        return services;
    }
}
