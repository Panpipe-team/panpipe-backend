using Microsoft.EntityFrameworkCore;
using Panpipe.Domain.Habit;
using Panpipe.Domain.HabitParamsSet;
using Panpipe.Persistence;
using Quartz;

namespace Panpipe.Sheduler.Job;

public class SetMarkHabitsAsyncJob(ILogger<SetMarkHabitsAsyncJob> logger, AppDbContext dbContext) : IJob
{
    private ILogger<SetMarkHabitsAsyncJob> _logger = logger;
    
    private AppDbContext _appDbContext = dbContext;

    public async Task Execute(IJobExecutionContext context)
    {
        JobKey jobKey = context.JobDetail.Key;
        _logger.LogInformation("START JOB: {jobKey}", jobKey);

        try
        {
            await AddEmptyMarks();
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("FAILED JOB: {jobKey}, Message: {message}, StackTrace: {ex}", jobKey, e.Message, e);
        }
    }

    private async Task AddEmptyMarks()
    {
        var habitsParams = await _appDbContext.Habits
            .Include(x => x.Marks)
            .Join(
                _appDbContext.HabitParamsSets,
                habit => habit.ParamsSetId,
                paramsSet => paramsSet.Id,
                (habit, paramsSet) => new 
                {
                    habit,
                    paramsSet
                }
            )
            .ToListAsync();

        foreach (var habitParam in habitsParams)
        {
            HabitMark? lastMark = habitParam.habit.Marks.MaxBy(m => m.TimestampUtc);
            if (lastMark is null) 
                continue;
            
            List<DateTimeOffset> emptyMarksTimestamps = habitParam.paramsSet.CalculateTimestampsOfEmptyMarksForExistingHabitFromLastMarkTimestamp(lastMark.TimestampUtc);
            IEnumerable<HabitMark> marks = emptyMarksTimestamps.Select(timestamp => HabitMark.CreateEmpty(Guid.NewGuid(), timestamp));
            
            foreach (var emptyMark in marks)
            {
                habitParam.habit.AddEmptyMark(emptyMark);
            }
        }
    }
}