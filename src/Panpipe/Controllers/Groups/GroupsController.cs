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
[Route("/api/v1.1/[controller]")]
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
        const string ReplacementForNullFullname = "";

        var group = await _dbContext.Groups
            .AsNoTracking()
            .Where(group => group.Id == id)
            .FirstOrDefaultAsync();

        if (group == null)
        {
            return Result.NotFound();
        }

        var participants = new List<GetGroupResponseParticipant> ();

        foreach (var userId in group.UserIds)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                return Result.CriticalError(
                    $"Group with id {id} has participant with userId {userId}, " +
                    "but user with this is cannot be found"
                );
            }

            participants.Add(new GetGroupResponseParticipant(userId, user.FullName ?? ReplacementForNullFullname));
        }

        return Result.Success(new GetGroupResponse(group.Name, participants));
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

        var group = new Group(
            Guid.NewGuid(), 
            request.Name, 
            user.Id, 
            request.Participants.Select(participant => participant.UserId).ToList()
        );

        _dbContext.Groups.Add(group);
        await _dbContext.SaveChangesAsync();

        return Result.Created(new CreateGroupResponse(group.Id));
    }

    [HttpPost]
    [Route("{groupId:guid}/participants")]
    [TranslateResultToActionResult]
    public async Task<Result> CreateGroupParticipant(
        [FromRoute] Guid groupId, [FromBody] CreateGroupParticipantRequest request
    )
    {
        // FAKED
        return Result.Success();
    }

    [HttpDelete]
    [Route("{groupId:guid}/participants")]
    [TranslateResultToActionResult]
    public async Task<Result> ExitGroup([FromRoute] Guid groupId)
    {
        // FAKED
        return Result.Success();
    }

    
}
