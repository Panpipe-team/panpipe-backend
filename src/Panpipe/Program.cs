using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Panpipe.Persistence;
using Panpipe.Persistence.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<AppDbContext>(
    options => 
    {
        options.UseNpgsql("Host=db;Database=panpipe;Username=panpipe;Password=panpipe");
        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
        }
    } 
);

builder.Services.AddDbContextPool<AppIdentityDbContext>(
    options => options.UseNpgsql("Host=db-identity;Database=panpipe-identity;Username=panpipe;Password=panpipe")
);

builder.Services.AddIdentity<AppIdentityUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppIdentityDbContext>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddCors();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Bug fix: API Endpoint with AuthorizeAttribute returns 404 on Unauthorized
// because trying to redirect to somewhere /account/login
// See more: https://github.com/dotnet/aspnetcore/issues/9039
builder.Services.ConfigureApplicationCookie(options =>{
    options.Events.OnRedirectToAccessDenied = context => {
             context.Response.StatusCode = 403;
             return Task.CompletedTask;
    };
    options.Events.OnRedirectToLogin = context => {
             context.Response.StatusCode = 401;
             return Task.CompletedTask;
    };
});

var app = builder.Build();

await SeedAppDatabase(app);
SeedIdentityDatabase(app);

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(builder => builder.AllowAnyOrigin());

app.UseSwagger(c => c.RouteTemplate = "api" + c.RouteTemplate);
app.UseSwaggerUI(c => c.RoutePrefix = "api/swagger");

app.MapControllers();

app.Run();

static async Task SeedAppDatabase(WebApplication app)
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

        throw;
    }
}

static void SeedIdentityDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var scopedProvider = scope.ServiceProvider;
    var appIdentityDbContext = scopedProvider.GetRequiredService<AppIdentityDbContext>();
    appIdentityDbContext.Database.EnsureDeleted();
    appIdentityDbContext.Database.EnsureCreated();
}
