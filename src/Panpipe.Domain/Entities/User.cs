namespace Panpipe.Domain.Entities;

public class User
{
    public User(Guid id, string email)
    {
        Id = id;
        Email = email;
    }

    public Guid Id { get; set; }

    public string? Name { get; set; }
    public string Email { get; set; }
}