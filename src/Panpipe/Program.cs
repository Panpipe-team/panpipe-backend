using Microsoft.EntityFrameworkCore;
using Panpipe.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<AppDbContext>(
    options => options
        .UseNpgsql("Host=db;Database=panpipe;Username=panpipe;Password=panpipe")
        .EnableSensitiveDataLogging()
);

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await SeedDatabase(app);

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();

static async Task SeedDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var scopedProvider = scope.ServiceProvider;
    try
    {
        var appDbContext = scopedProvider.GetRequiredService<AppDbContext>();
        appDbContext.Database.EnsureDeleted();
        appDbContext.Database.EnsureCreated();
        await AppDbContextSeed.SeedAsync(appDbContext);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred seeding the DB.");
    }
}
