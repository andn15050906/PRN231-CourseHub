using AutoMapper;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Interfaces.Repositories;

namespace CourseHub.Core.Services.Domain;

public abstract class DomainService
{
    protected readonly IUnitOfWork _uow;
    protected readonly IMapper _mapper;
    protected readonly IAppLogger _logger;

    internal DomainService(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger logger)
    {
        _uow = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    protected static ServiceResult Ok() => new(200);
    protected static ServiceResult<T> Ok<T>(T data) => new(200, data);

    protected static ServiceResult Created() => new(201);
    protected static ServiceResult<T> Created<T>(T data) => new(201, data);

    protected static ServiceResult BadRequest() => new(400);
    protected static ServiceResult BadRequest(string message) => new(400, message);
    protected static ServiceResult<T> BadRequest<T>(string message) => new(400, message);

    protected static ServiceResult Unauthorized() => new(401);
    protected static ServiceResult Unauthorized(string message) => new(401, message);
    protected static ServiceResult<T> Unauthorized<T>() => new(401);
    protected static ServiceResult<T> Unauthorized<T>(string message) => new(401, message);

    protected static ServiceResult Forbidden() => new(403);
    protected static ServiceResult<T> Forbidden<T>() => new(403);
    protected static ServiceResult<T> Forbidden<T>(string message) => new(403, message);

    protected static ServiceResult<T> NotFound<T>() => new(404);

    protected static ServiceResult Conflict(string message) => new(409, message);
    protected static ServiceResult<T> Conflict<T>(string message) => new(409, message);

    protected static ServiceResult ServerError() => new(500);
    protected static ServiceResult<T> ServerError<T>() => new(500);
    protected static ServiceResult<T> ServerError<T>(string message) => new(500, message);



    // Necessary explicit methods for PagedList and IEnumerable

    protected static ServiceResult<PagedResult<T>> ToQueryResult<T>(PagedResult<T> pagedList)
    {
        if (pagedList is null || pagedList.TotalCount == 0)
            return NotFound<PagedResult<T>>();
        return Ok(pagedList);
    }

    protected static ServiceResult<IEnumerable<T>> ToQueryResult<T>(IEnumerable<T> values)
    {
        if (values is null || !values.Any())
            return NotFound<IEnumerable<T>>();
        return Ok(values);
    }

    protected static ServiceResult<T> ToQueryResult<T>(T? value)
    {
        return value is null ? NotFound<T>() : Ok(value);
    }
}
