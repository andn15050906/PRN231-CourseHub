using Microsoft.AspNetCore.Mvc;

namespace CourseHub.Core.Helpers.Messaging;

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; init; }

    public ServiceResult(short status) : base(status) { }

    public ServiceResult(short status, T data) : base(status)
    {
        Status = status;
        Data = data;
    }

    public ServiceResult(short status, string? message) : base(status, message)
    {
        Status = status;
        Message = message;
    }



    public override IActionResult AsResponse()
    {
        return new JsonResult(Data != null ? Data : Message) { StatusCode = Status };
    }
}