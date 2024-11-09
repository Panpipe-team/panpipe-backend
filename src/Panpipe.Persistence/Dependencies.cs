using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Panpipe.Persistence.Data;

namespace Panpipe.Persistence;

public static class Dependencies
{
    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(c => c.UseInMemoryDatabase("Panpipe"));
    }
}
