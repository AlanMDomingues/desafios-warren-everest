using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ApplyMigrations(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            using var provider = services.BuildServiceProvider();
            provider.ApplyMigrations();
        }
    }
}
