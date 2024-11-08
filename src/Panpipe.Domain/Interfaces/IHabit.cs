namespace Panpipe.Domain.Interfaces;

public interface IHabit
{
    public void AddEmptyMark(Guid markId, DateTimeOffset timestamp);
}