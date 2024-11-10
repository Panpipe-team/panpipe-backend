using Ardalis.Result;
using MediatR;
using Panpipe.Domain.Entities.HabitParamsAggregate;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Application.Queries.GetHabitTemplates;

public record GetHabitTemplatesQuery<T>(): IRequest<Result<List<HabitParams<T>>>> where T: IHabitResultType;
