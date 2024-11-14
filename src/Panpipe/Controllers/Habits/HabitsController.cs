using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panpipe.Controllers.Habits.Helpers;
using Panpipe.Persistence;

namespace Panpipe.Controllers.Habits;

[ApiController]
[Route("/api/v1/[controller]")]
public class HabitsController(AppDbContext appDbContext): ControllerBase
{
    private readonly AppDbContext _appDbContext = appDbContext;

    [HttpGet]
    [Route("templates")]
    public async Task<GetPublicTemplatesResponse> GetPublicTemplates()
    {
        var templates = await _appDbContext.HabitParamsSets
            .AsNoTracking()
            .Where(x => x.IsPublicTemplate)
            .Include(x => x.Goal)
            .ToListAsync();
        
        return new GetPublicTemplatesResponse(templates.Select(template => 
            new GetPublicTemplatesResponseTemplate(
                template.Id, 
                template.Name, 
                template.Frequency.ToReadableString(), 
                template.Goal.ToReadableString(), 
                template.ResultType.ToString()
            )
        ).ToList());
    }
}
