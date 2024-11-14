using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panpipe.Domain.Group;
using Panpipe.Persistence;

namespace Panpipe.Controllers.Groups;

[ApiController]
[Route("/api/v1/[controller]")]
public class GroupsController(AppDbContext dbContext) : ControllerBase
{
    private readonly AppDbContext _dbContext = dbContext;

    [HttpGet]
    [Route("{id:guid}")]
    [TranslateResultToActionResult]
    public async Task<Result<GetGroupResponse>> GetGroup([FromRoute] Guid id)
    {
        var userId = Guid.Empty;

        var group = await _dbContext.Groups
            .AsNoTracking()
            .Where(group => group.Id == id)
            .FirstOrDefaultAsync();

        if (group == null)
        {
            return Result.NotFound();
        }

        return new GetGroupResponse(
            group.Name, group.UserIds.Select(userId => new GetGroupResponseParticipant(userId)).ToList()
        );
    }

    [HttpGet]
    public async Task<GetGroupsResponse> GetGroups()
    {
        var userId = Guid.Empty;

        var groups = await _dbContext.Groups
            .AsNoTracking()
            .Where(group => group.UserIds.Contains(userId))
            .ToListAsync();

        return new GetGroupsResponse(groups.Select(group => new GetGroupsResponseGroup(group.Id, group.Name)).ToList());
    }

    [HttpPost]
    public async Task<CreateGroupResponse> CreateGroup([FromBody] CreateGroupRequest request)
    {
        var userId = Guid.Empty;

        var group = new Group(Guid.NewGuid(), request.Name, userId);

        _dbContext.Groups.Add(group);
        await _dbContext.SaveChangesAsync();

        return new CreateGroupResponse(group.Id);
    }
}
