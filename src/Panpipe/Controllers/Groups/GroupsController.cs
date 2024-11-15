using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panpipe.Domain.Group;
using Panpipe.Persistence;
using Panpipe.Persistence.Identity;

namespace Panpipe.Controllers.Groups;

[ApiController]
[Route("/api/v1/[controller]")]
[Authorize]
public class GroupsController(AppDbContext dbContext, UserManager<AppIdentityUser> userManager) : ControllerBase
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly UserManager<AppIdentityUser> _userManager = userManager;

    [HttpGet]
    [Route("{id:guid}")]
    [TranslateResultToActionResult]
    public async Task<Result<GetGroupResponse>> GetGroup([FromRoute] Guid id)
    {
        var group = await _dbContext.Groups
            .AsNoTracking()
            .Where(group => group.Id == id)
            .FirstOrDefaultAsync();

        if (group == null)
        {
            return Result.NotFound();
        }

        return Result.Success(new GetGroupResponse(
            group.Name, group.UserIds.Select(userId => new GetGroupResponseParticipant(userId)).ToList()
        ));
    }

    [HttpGet]
    [TranslateResultToActionResult]
    public async Task<Result<GetGroupsResponse>> GetGroups()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Result.Unauthorized("Cannot find authorized user by claim");
        }

        var groups = await _dbContext.Groups
            .AsNoTracking()
            .Where(group => group.UserIds.Contains(user.Id))
            .ToListAsync();

        return Result.Success(
            new GetGroupsResponse(groups.Select(group => new GetGroupsResponseGroup(group.Id, group.Name)).ToList())
        );
    }

    [HttpPost]
    [TranslateResultToActionResult]
    public async Task<Result<CreateGroupResponse>> CreateGroup([FromBody] CreateGroupRequest request)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Result.Unauthorized("Cannot find authorized user by claim");
        }

        var group = new Group(Guid.NewGuid(), request.Name, user.Id);

        _dbContext.Groups.Add(group);
        await _dbContext.SaveChangesAsync();

        return Result.Created(new CreateGroupResponse(group.Id));
    }
}
