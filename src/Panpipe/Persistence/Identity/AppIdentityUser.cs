using Microsoft.AspNetCore.Identity;

namespace Panpipe.Persistence.Identity;

public class AppIdentityUser: IdentityUser<Guid>
{
    public string FullName { get; set; } = "";

    public AppIdentityUser(): base() {}

    public AppIdentityUser(string username, string fullname) : base(username)
    {
        FullName = fullname;
    }
}
