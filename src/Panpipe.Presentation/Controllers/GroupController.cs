using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Panpipe.Application.Commands.CreateGroup;
using Panpipe.Application.Queries.GetGroup;
using Panpipe.Presentation.Requests;
using Ardalis.Result;
using Panpipe.Presentation.Responses;

namespace Panpipe.Presentation.Controllers;

[ApiController]
[Route("/api/groups")]
public class GroupController: ControllerBase {
    private readonly IMediator _mediator;

    public GroupController(IMediator mediator) 
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<ActionResult<GetGroupResponse>> GetById(Guid id) 
    {
        var command = new GetGroupQuery(id);
        var group = await _mediator.Send(command);

        return group.Map(group => new GetGroupResponse(group.Name, group.UserIds)).ToActionResult(this);
    }

    [HttpPost]
    public async Task<ActionResult<CreateGroupResponse>> Create(CreateGroupRequest request) 
    {
        var userId = Guid.Empty;
        var command = new CreateGroupCommand(userId, request.Name);
        var result = await _mediator.Send(command);

        return result.Map(guid => new CreateGroupResponse(guid)).ToActionResult(this);
    }
}
