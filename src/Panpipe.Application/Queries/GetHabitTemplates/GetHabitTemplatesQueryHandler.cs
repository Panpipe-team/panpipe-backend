using Ardalis.Result;
using MediatR;
using Panpipe.Application.Interfaces;
using Panpipe.Application.Specifications;
using Panpipe.Domain.Entities.HabitParamsAggregate;

namespace Panpipe.Application.Queries.GetHabitTemplates;

public class GetHabitTemplatesQueryHandler : IRequestHandler<GetHabitTemplatesQuery, Result<List<HabitParams>>>
{
    private readonly IReadRepository<HabitParams> _habitParamsRepository;

    public GetHabitTemplatesQueryHandler(IReadRepository<HabitParams> habitParamsRepository)
    {
        _habitParamsRepository = habitParamsRepository;
    }
    
    public async Task<Result<List<HabitParams>>> Handle(GetHabitTemplatesQuery request, CancellationToken cancellationToken)
    {
        var spec = new HabitParamsPublicTemplatesSpecification();

        var result = await _habitParamsRepository.ListAsync(spec, cancellationToken);

        return Result.Success(result);
    }
}
