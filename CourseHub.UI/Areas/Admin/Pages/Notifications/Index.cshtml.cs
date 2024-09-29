using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Common.NotificationModels;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.Common.NotificationDtos;
using CourseHub.UI.Helpers.AppStart;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.CommonServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Areas.Admin.Pages.Notifications;

public class IndexModel : PageModel
{
    private readonly INotificationApiService _notificationApiService;

    public PagedResult<NotificationModel> Data { get; set; }

    public string NotificationRequestPath { get; set; }



    public IndexModel(INotificationApiService notificationApiService)
    {
        _notificationApiService = notificationApiService;
        NotificationRequestPath = Configurer.GetApiClientOptions().ApiServerPath + "/api/notifications";
    }

    public async Task OnGet()
    {
        //...
        QueryNotificationDto dto = new() { PageSize = 20 };
        var result = await _notificationApiService.GetPaged(dto, HttpContext);

        if (result is null)
            Data = PagedResult<NotificationModel>.GetEmpty();
        else
            Data = result;
    }
}
