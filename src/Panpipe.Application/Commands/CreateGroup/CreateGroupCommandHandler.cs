using Ardalis.Result;
using MediatR;
using Panpipe.Application.Interfaces;
using Panpipe.Domain.Entities;

namespace Panpipe.Application.Commands.CreateGroup;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Result<Guid>> 
{
    private readonly IRepository<Group> _groupRepository;

    public CreateGroupCommandHandler(IRepository<Group> groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<Result<Guid>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var guid = Guid.NewGuid();
        var group = new Group(guid, request.Name, request.UserId);
        await _groupRepository.AddAsync(group, cancellationToken);
        await _groupRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(guid);
    }
}
