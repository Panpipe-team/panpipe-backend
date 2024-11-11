using Ardalis.Result;
using MediatR;
using Panpipe.Domain.Entities.HabitParamsAggregate;

namespace Panpipe.Application.Queries.GetHabitTemplates;

public record GetHabitTemplatesQuery(): IRequest<Result<List<HabitParams>>>;
