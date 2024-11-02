using CourseHub.Core.Entities.CommonDomain.Enums;
using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Models.Common.NotificationModels;
using CourseHub.Core.Models.Social;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.Common.NotificationDtos;
using CourseHub.UI.Services.Contracts.CommonServices;
using CourseHub.UI.Services.Contracts.SocialServices;
using CourseHub.UI.Services.Contracts.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace CourseHub.UI.Areas.Admin.Pages.Groups;

public class ReportedListModel : PageModel
{
    public List<NotificationModel> Notifications { get; set; }
    public Dictionary<Guid, UserOverviewModel> RelatedUsers { get; set; }
    public Dictionary<Guid, ConversationModel> Notification_Conversation { get; set; }
    public Dictionary<Guid, string> Notification_Message { get; set; }



    public async Task OnGet(
        [FromServices] INotificationApiService notificationApiService,
        [FromServices] IUserApiService userApiService,
        [FromServices] IConversationApiService conversationApiService)
    {
        var notificationQuery = new QueryNotificationDto
        {
            Type = NotificationType.ReportGroup
        };
        var notificationResponse = await notificationApiService.GetPaged(notificationQuery, HttpContext);
        Notifications = notificationResponse.Items;



        var userResponseTask = userApiService.GetOverviewAsync(Notifications.Select(_ => _.CreatorId));

        List <Guid> conversationIds = new();
        Notification_Message = new();
        Notification_Conversation = new();
        foreach (var item in Notifications)
        {
            var message = JsonSerializer.Deserialize<CreateConversationReportDto>(item.Message);
            if (message is null)
                continue;
            Notification_Message.Add(item.Id, message.Message);
            conversationIds.Add(message.Conversation);
            Notification_Conversation.Add(item.Id, new ConversationModel { Id = message.Conversation });
        }

        var conversationTask = conversationApiService.GetMultipleAsync(conversationIds, HttpContext);

        await Task.WhenAll(userResponseTask, conversationTask);

        RelatedUsers = userResponseTask.Result.ToDictionary(_ => _.Id);

        foreach (var item in conversationTask.Result)
        {
            var key = Notification_Conversation.FirstOrDefault(_ => _.Value.Id == item.Id).Key;
            Notification_Conversation[key] = item;
        };
    }
}
