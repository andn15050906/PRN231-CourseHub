using CourseHub.Core.Entities.CommonDomain.Enums;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Common.NotificationModels;
using CourseHub.Core.Models.Course.InstructorModels;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.Common.NotificationDtos;
using CourseHub.Core.RequestDtos.Payment;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.CommonServices;
using CourseHub.UI.Services.Contracts.CourseServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace CourseHub.UI.Pages.Instructor;

public class WithdrawModel : PageModel
{
    public UserFullModel Client { get; set; }
    public PagedResult<NotificationModel> Notifications { get; set; }
    public Dictionary<Guid, CreateWithdrawalDto> Notification_Withdrawal { get; set; }
    


    [BindProperty]
    public long Balance { get; set; }

    [BindProperty]
    public CreateWithdrawalDto Dto { get; set; }



    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        Client = context.HttpContext.GetClientData().Result;
        if (Client is null)
            context.Result = Redirect(Global.PAGE_SIGNIN);
        else if (Client.Role != Core.Entities.UserDomain.Enums.Role.Instructor)
            context.Result = Redirect(Global.PAGE_403);
    }



    public async Task<IActionResult> OnGet(
        [FromServices] IInstructorApiService instructorApiService,
        [FromServices] INotificationApiService notificationApiService)
    {
        var client = await HttpContext.GetClientData();
        if (client is null)
            return Redirect(Global.PAGE_SIGNIN);

        var instructor = await instructorApiService.GetByUserId(client.Id);
        if (instructor is null)
            return Redirect(Global.PAGE_404);



        QueryNotificationDto query = new()
        {
            CreatorId = client.Id,
            Type = NotificationType.RequestWithdrawal
        };
        Notifications = await notificationApiService.GetPaged(query, HttpContext);
        Notification_Withdrawal = new();
        foreach (var notification in Notifications.Items)
        {
            var message = JsonSerializer.Deserialize<CreateWithdrawalDto>(notification.Message);
            Notification_Withdrawal.Add(notification.Id, message);
        }



        Balance = instructor.Balance;
        TempData[Global.DATA_USE_BACKGROUND] = true;
        return Page();
    }

    public async Task<IActionResult> OnPost([FromServices] INotificationApiService notificationApiService)
    {
        if (Dto.Amount > Balance)
        {
            TempData[Global.ALERT_STATUS] = false;
            TempData[Global.ALERT_MESSAGE] = "You can't withdraw more than your balance!";
            //TempData[Global.DATA_USE_BACKGROUND] = true;
            return Redirect(Request.Path);
        }

        if (!ModelState.IsValid)
        {
            TempData[Global.ALERT_STATUS] = false;
            TempData[Global.ALERT_MESSAGE] = "Invalid data!";
            //TempData[Global.DATA_USE_BACKGROUND] = true;
            return Redirect(Request.Path);
        }

        var response = await notificationApiService.CreateWithdrawRequest(Dto, HttpContext);
        if (response.IsSuccessStatusCode)
        {
            TempData[Global.ALERT_STATUS] = response.IsSuccessStatusCode;
            TempData[Global.ALERT_MESSAGE] = response.IsSuccessStatusCode ?
                "Requested successfully." :
                $"Cannot request for withdrawal!";
        }
        //TempData[Global.DATA_USE_BACKGROUND] = true;
        return Redirect(Request.Path);
    }
}
