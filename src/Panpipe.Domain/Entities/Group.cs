namespace Panpipe.Domain.Entities;

public class Group
{
    public Group() { }
    public Group(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public virtual ICollection<Account> Members { get; set; } = new List<Account>();

    public virtual ICollection<Habit> Habits { get; set; } = new List<Habit>();
}