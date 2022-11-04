using Application.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace API.Tests
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(CustomerProfile).Assembly);
        }
    }
}
