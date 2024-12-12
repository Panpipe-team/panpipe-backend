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
    private readonly AppDbContext _appDbContext = dbContext;
    private readonly UserManager<AppIdentityUser> _userManager = userManager;

    [HttpGet]
    [Route("{id:guid}")]
    [TranslateResultToActionResult]
    public async Task<Result<GetGroupResponse>> GetGroup([FromRoute] Guid id)
    {
        var group = await _appDbContext.Groups
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
                    "but user with this id cannot be found"
                );
            }

            var userName = user.UserName;

            if (userName is null)
            {
                return Result.CriticalError(
                    $"User with id {userId} does not have login"
                );
            }

            participants.Add(new GetGroupResponseParticipant(userId, userName, user.FullName));
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

        var groups = await _appDbContext.Groups
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

        var participantIds = request.Participants.Select(participant => participant.UserId).ToList();

        foreach (var participantId in participantIds)
        {
            var participant = await _userManager.FindByIdAsync(participantId.ToString());
            if (participant is null)
            {
                return Result.Invalid(new ValidationError($"User with id {participantId} cannot be found"));
            }
        }

        var group = new Group(
            Guid.NewGuid(), 
            request.Name, 
            user.Id, 
            participantIds
        );

        _appDbContext.Groups.Add(group);
        await _appDbContext.SaveChangesAsync();

        return Result.Created(new CreateGroupResponse(group.Id));
    }

    [HttpPost]
    [Route("{groupId:guid}/participants")]
    [TranslateResultToActionResult]
    public async Task<Result> CreateGroupParticipant(
        [FromRoute] Guid groupId, [FromBody] CreateGroupParticipantRequest request
    )
    {
        var group = await _appDbContext.Groups
            .Where(group => group.Id == groupId)
            .FirstOrDefaultAsync();
        
        if (group is null)
        {
            return Result.Invalid(new ValidationError($"Group with id {groupId} was not found"));
        }

        var userId = request.UserId;

        if (group.UserIds.Contains(userId))
        {
            return Result.Invalid(new ValidationError(
                $"User with id {userId} is already participant of group with id {groupId}"
            ));
        }

        group.AddUserId(userId);

        await _appDbContext.SaveChangesAsync();
        
        return Result.Success();
    }

    [HttpDelete]
    [Route("{groupId:guid}/participants")]
    [TranslateResultToActionResult]
    public async Task<Result> ExitGroup([FromRoute] Guid groupId)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Result.Unauthorized("Cannot find authorized user by claim");
        }

        var group = await _appDbContext.Groups
            .Where(group => group.Id == groupId)
            .FirstOrDefaultAsync();
        
        if (group is null)
        {
            return Result.Invalid(new ValidationError($"Group with id {groupId} was not found"));
        }

        group.RemoveUserId(user.Id);

        await _appDbContext.SaveChangesAsync();

        return Result.Success();
    }

    
}
