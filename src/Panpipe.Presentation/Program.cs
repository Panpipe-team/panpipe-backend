using Panpipe.Application.Commands.CreateGroup;
using Panpipe.Application.Interfaces;
using Panpipe.Persistence.Data;
using Panpipe.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Panpipe.Persistence.Identity;

var builder = WebApplication.CreateBuilder(args);

Dependencies.ConfigureServices(builder.Configuration, builder.Services);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateGroupCommandHandler).Assembly));

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));

builder.Services.AddIdentity<AppIdentityUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppIdentityDbContext>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
