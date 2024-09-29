using CourseHub.Core.Entities.CommonDomain.Enums;
using CourseHub.Core.Entities.SocialDomain;
using CourseHub.Core.Models.Common.NotificationModels;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.Common.NotificationDtos;
using CourseHub.UI.Services.Contracts.CommonServices;
using CourseHub.UI.Services.Contracts.CourseServices;
using CourseHub.UI.Services.Contracts.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace CourseHub.UI.Areas.Admin.Pages.Courses;

public class IndexModel : PageModel
{
    public List<NotificationModel> Notifications { get; set; }
    public Dictionary<Guid, UserOverviewModel> RelatedUsers { get; set; }
    public Dictionary<Guid, CourseOverviewModel> Notification_Course { get; set; }
    public Dictionary<Guid, string> Notification_Message { get; set; }



    public async Task OnGet(
        [FromServices] INotificationApiService notificationApiService,
        [FromServices] IUserApiService userApiService,
        [FromServices] ICourseApiService courseApiService)
    {
        var notificationQuery = new QueryNotificationDto
        {
            Type = NotificationType.ReportCourse
        };
        var notificationResponse = await notificationApiService.GetPaged(notificationQuery, HttpContext);
        Notifications = notificationResponse.Items;



        var userResponseTask = userApiService.GetOverviewAsync(Notifications.Select(_ => _.CreatorId));

        List<Guid> courseIds = new();
        Notification_Message = new();
        Notification_Course = new();
        foreach (var item in Notifications)
        {
            var message = JsonSerializer.Deserialize<CreateCourseReportDto>(item.Message);
            if (message is null)
                continue;
            Notification_Message.Add(item.Id, message.Message);
            courseIds.Add(message.Course);
            Notification_Course.Add(item.Id, new CourseOverviewModel { Id = message.Course });
        }
        var courseTask = courseApiService.GetMultipleAsync(courseIds);

        await Task.WhenAll(userResponseTask, courseTask);

        RelatedUsers = userResponseTask.Result.ToDictionary(_ => _.Id);
        foreach (var course in courseTask.Result)
        {
            var key = Notification_Course.FirstOrDefault(_ => _.Value.Id == course.Id).Key;
            Notification_Course[key] = course;
        };
    }
}
