using CourseHub.Core.Helpers.Messaging.Messages;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.RequestDtos.Payment.BillDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.AppStart;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.CourseServices;
using CourseHub.UI.Services.Contracts.PaymentServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.Payment;

public class IndexModel : PageModel
{
    public CourseModel Course { get; set; }

    public async Task<IActionResult> OnGet(
        [FromQuery] Guid courseId, [FromQuery] bool? failed,
        [FromServices] ICourseApiService courseApiService)
    {
        var client = await HttpContext.GetClientData();
        if (client is null)
            return Redirect(Global.PAGE_SIGNIN);

        var result = await courseApiService.GetAsync(courseId);
        if (result is null)
            return Redirect(Global.PAGE_404);

        TempData[Global.DATA_USE_BACKGROUND] = true;
        Course = result;

        if (failed == true)
		{
			TempData[Global.ALERT_MESSAGE] = "Transaction cancelled!";
			TempData[Global.ALERT_STATUS] = false;
		}
        return Page();
    }

    public async Task<IActionResult> OnPost([FromServices] IPaymentApiService paymentApiService, [FromQuery] Guid courseId)
    {
        if (courseId == default)
            return Redirect(Global.PAGE_404);

        CreateBillDto dto = new()
        {
            Action = PaymentDomainMessages.ACTION_PAY_COURSE,
            Note = courseId.ToString(),
            Gateway = PaymentDomainMessages.GATEWAY_VNPAY,
        };

        var response = await paymentApiService.GetUrl(dto, HttpContext);
        if (response.IsSuccessStatusCode)
            return Redirect(await response.Content.ReadAsStringAsync());

        return Page();
    }
}
