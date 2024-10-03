using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Common.NotificationModels;
using CourseHub.Core.RequestDtos.Common.NotificationDtos;
using CourseHub.Core.RequestDtos.Course.InstructorDtos;
using CourseHub.Core.RequestDtos.Payment;

namespace CourseHub.UI.Services.Contracts.CommonServices;

public interface INotificationApiService
{
    Task<PagedResult<NotificationModel>> GetPaged(QueryNotificationDto dto, HttpContext context);

    Task<HttpResponseMessage> RequestInstructor(CreateInstructorDto dto, HttpContext context);
    Task<HttpResponseMessage> CreateWithdrawRequest(CreateWithdrawalDto dto, HttpContext context);
}
