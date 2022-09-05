using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Data.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void ApplyMigrations(this IServiceProvider provider)
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            try
            {
                Console.WriteLine("Tentando aplicar Migrations");
                var dbContext = provider.GetService<DataContext>();
                dbContext.Database.Migrate();
                Console.WriteLine("Migrations aplicada com sucesso");
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                if (ex.InnerException != null)
                {
                    message = ex.InnerException.Message;
                }

                Console.WriteLine("Erro ao aplicar migrations no banco de dados. Erro: {0}", message);
            }
        }
    }
}
