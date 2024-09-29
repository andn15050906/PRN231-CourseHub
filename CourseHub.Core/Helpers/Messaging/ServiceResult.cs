using Microsoft.AspNetCore.Mvc;

namespace CourseHub.Core.Helpers.Messaging;

public class ServiceResult
{
    public short Status { get; init; }
    public string? Message { get; init; }
    public bool IsSuccessful { get => Status < 300; }

    public ServiceResult(short status)
    {
        Status = status;
    }

    public ServiceResult(short status, string? message)
    {
        Status = status;
        Message = message;
    }



    public virtual IActionResult AsResponse()
    {
        return new JsonResult(Message) { StatusCode = Status };
    }
}
