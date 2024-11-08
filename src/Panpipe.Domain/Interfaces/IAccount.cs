namespace Panpipe.Domain.Interfaces;

public interface IAccount {
    Guid Id { get; }
    string Login { get; }
}