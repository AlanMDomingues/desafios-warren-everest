using Application.Profiles;
using EntityFrameworkCore.UnitOfWork.Extensions;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests
{
    public class Startup
    {
        private static readonly DbContextOptions<DataContext> _contextOptions =
           new DbContextOptionsBuilder<DataContext>()
               .UseInMemoryDatabase("DataContextTest")
               .Options;

        private static DataContext _context;

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(CustomerProfile).Assembly);
            _context = new DataContext(_contextOptions);

            _context.Database.EnsureCreated();

            services.AddSingleton(_context);
            services.AddUnitOfWork<DataContext>(ServiceLifetime.Singleton);
        }

        //private static void DeleteAll<T>(DbContext context)
        //    where T : class
        //{
        //    foreach (var p in context.Set<T>())
        //    {
        //        context.Entry(p).State = EntityState.Deleted;
        //    }
        //}
    }
}
