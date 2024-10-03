using CourseHub.Core.Entities.CommonDomain.Enums;
using CourseHub.Core.Entities.UserDomain.Enums;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Common.NotificationModels;
using CourseHub.Core.Models.Payment;
using CourseHub.Core.RequestDtos.Common.NotificationDtos;
using CourseHub.Core.RequestDtos.Payment;
using CourseHub.Core.RequestDtos.Payment.BillDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.CommonServices;
using CourseHub.UI.Services.Contracts.PaymentServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace CourseHub.UI.Areas.Admin.Pages.Payment;

public class IndexModel : PageModel
{
    public PagedResult<NotificationModel> Notifications { get; set; }
    public PagedResult<BillModel> Bills { get; set; }
    public Dictionary<Guid, CreateWithdrawalDto> Notification_Withdrawal { get; set; }



    public async Task<IActionResult> OnGet(
        [FromServices] INotificationApiService notificationApiService,
        [FromServices] IPaymentApiService paymentApiService)
    {
        var client = await HttpContext.GetClientData();
        if (client is null)
            return Redirect(Global.PAGE_SIGNIN);
        if (client.Role != Role.SysAdmin)
            return Redirect(Global.PAGE_403);



        QueryNotificationDto notificationQuery = new()
        {
            Type = NotificationType.RequestWithdrawal
        };

        QueryBillDto billQuery = new();

        var notificationTask = notificationApiService.GetPaged(notificationQuery, HttpContext);
        var billTask = paymentApiService.GetPagedAsync(billQuery, HttpContext);

        Notifications = notificationTask.Result;
        Bills = billTask.Result;



        Notification_Withdrawal = new();
        foreach (var notification in Notifications.Items)
        {
            var message = JsonSerializer.Deserialize<CreateWithdrawalDto>(notification.Message);
            Notification_Withdrawal.Add(notification.Id, message);
        }



        return Page();
    }
}
