namespace Panpipe.Domain.Entities;

public class Result
{
    public Result() { }

    public Result(Guid id, DateTime date, string? value = null)
    {
        Id = id;
        Date = date;
        Value = value;
    }
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string? Value { get; set; }
}