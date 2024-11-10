using Ardalis.Result;
using MediatR;
using Panpipe.Application.Interfaces;
using Panpipe.Application.Specifications;
using Panpipe.Domain.Entities.HabitParamsAggregate;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Application.Queries.GetHabitTemplates;

public class GetHabitTemplatesQueryHandler<T> : IRequestHandler<GetHabitTemplatesQuery<T>, Result<List<HabitParams<T>>>>
    where T : IHabitResultType
{
    private readonly IReadRepository<HabitParams<T>> _habitParamsRepository;

    public GetHabitTemplatesQueryHandler(IReadRepository<HabitParams<T>> habitParamsRepository)
    {
        _habitParamsRepository = habitParamsRepository;
    }
    
    public async Task<Result<List<HabitParams<T>>>> Handle(GetHabitTemplatesQuery<T> request, CancellationToken cancellationToken)
    {
        var spec = new HabitParamsPublicTemplatesSpecification<T>();

        var result = await _habitParamsRepository.ListAsync(spec, cancellationToken);

        return Result.Success(result);
    }
}
