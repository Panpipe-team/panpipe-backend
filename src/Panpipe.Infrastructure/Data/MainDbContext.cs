using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Panpipe.Domain.Entities;

namespace Panpipe.Infrastructure.Data.Migrations;

public class MainDbContext : IdentityDbContext<Account, AccountRole, Guid>
{
    public MainDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
        
    }
}