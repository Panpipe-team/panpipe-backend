using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Panpipe.Persistence.Data;
using Panpipe.Persistence.Identity;

namespace Panpipe.Persistence;

public static class Dependencies
{
    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(c => c.UseInMemoryDatabase("Panpipe"));
        services.AddDbContext<AppIdentityDbContext>(c => c.UseInMemoryDatabase("Panpipe"));
    }
}
