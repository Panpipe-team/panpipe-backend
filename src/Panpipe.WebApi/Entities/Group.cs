namespace Panpipe.WebApi.Entities;

public class Group
{
    public Group() { }
    
    public Group(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    public Guid Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<User> Members { get; set; } = new List<User>();

    public virtual ICollection<Habit> Habits { get; set; } = new List<Habit>();
}