using EntityFrameworkCore.UnitOfWork.Extensions;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Alan_WarrenDesafio1.Configurations;

public static class DataConfiguration
{
    public static IServiceCollection AddDataConfiguration(
        this IServiceCollection services,
        string connectionString,
        ServiceLifetime? lifetime = null
    )
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        var builder = new DbContextOptionsBuilder<DataContext>().UseMySql(
            connectionString,
            ServerVersion.Parse("8.0.29"),
            mySqlOptions =>
            {
                var assembly = typeof(DataContext).Assembly;
                var assemblyName = assembly.GetName();

                mySqlOptions.MigrationsAssembly(assemblyName.Name);
            }
        );

        var dbContext = (DataContext)Activator.CreateInstance(typeof(DataContext), builder.Options);
        services.AddTransient(_ => dbContext);

        services.AddUnitOfWork<DataContext>(lifetime ?? ServiceLifetime.Scoped);

        return services;
    }
}
