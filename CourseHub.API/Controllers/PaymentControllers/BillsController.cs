using CourseHub.API.Controllers.Shared;
using CourseHub.API.Helpers.Cookie;
using CourseHub.API.Services.AppInfo;
using CourseHub.API.Services.External.Payment;
using CourseHub.Core.Entities.UserDomain.Enums;
using CourseHub.Core.Helpers.Business;
using CourseHub.Core.Helpers.Messaging.Messages;
using CourseHub.Core.Helpers.Text;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.RequestDtos.Payment.BillDtos;
using CourseHub.Core.Services.Domain.CourseServices.Contracts;
using CourseHub.Core.Services.Domain.PaymentServices.Contracts;
using CourseHub.Core.Services.Domain.PaymentServices.TempModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CourseHub.API.Controllers.PaymentControllers;

public class BillsController : BaseController
{
    private readonly IAppLogger _logger;

    public BillsController(IAppLogger logger)
    {
        _logger = logger;
    }

    [HttpGet("RedirectLink")]
    [Authorize]
    public async Task<IActionResult> GetRedirectLink(
        [FromQuery] CreateBillDto dto,
        [FromServices] ICourseService courseService, [FromServices] IOptions<AppInfoOptions> appInfo)
    {
        var client = HttpContext.GetClientId();

        int amount = 0;
        string orderInfo = string.Empty;

        switch (dto.Action)
        {
            case PaymentDomainMessages.ACTION_PAY_COURSE:
                if (!Guid.TryParse(dto.Note, out var courseId))
                    return BadRequest(PaymentDomainMessages.INVALID_NOTE);
                var courseResult = await courseService.GetMinAsync(courseId);
                if (!courseResult.IsSuccessful)
                    return BadRequest(PaymentDomainMessages.INVALID_NOTE);

                var course = courseResult.Data!;
                amount = CourseBusinessHelper.GetPostDiscount(course.Price, course.Discount, course.DiscountExpiry);
                orderInfo = $"{client}'s payment for course #{courseId}";
                break;
            default:
                return BadRequest(PaymentDomainMessages.INVALID_ACTION);
        }

        var request = new VNPayHelper.VNPayRequest
        {
            vnp_Amount = amount,
            vnp_OrderInfo = orderInfo,
            vnp_ReturnUrl = $"{appInfo.Value.MainBackendApp}/api/bills"
            //vnp_IpAddr = HttpContext.GetClientIPAddress()
        };
        string url = new VNPayHelper().BuildPaymentUrl(request);

        return Ok(url);
    }



    [HttpGet]
    public async Task<IActionResult> RedirectedFromVNPay(
        [FromQuery] VNPayHelper.VNPayResponse? vnpResponse,
        [FromServices] IBillService billService, [FromServices] IEnrollmentService enrollmentService,
        [FromServices] IOptions<AppInfoOptions> appInfo)
    {
        var clientUrl = appInfo.Value.MainFrontendApp;

        if (vnpResponse is null)
        {
            _logger.Warn("Null vnpResponse" + JsonSerializer.Serialize(vnpResponse));
            return Redirect(clientUrl + "/404");
        }

        //var clientId = HttpContext.GetClientId();
        var guids = TextHelper.GetGuidsFromString(vnpResponse.vnp_OrderInfo, 1);
        if (guids.Count == 0 || guids[0] == default)
        {
            _logger.Warn("Null client");
            return Redirect(clientUrl + "/404");
        }
        Guid clientId = guids[0];

        List<Guid> identifiers = TextHelper.GetGuidsFromString(vnpResponse.vnp_OrderInfo, 2);
        if (identifiers.Count < 2)
        {
            _logger.Warn("Invalid identifiers" + vnpResponse.vnp_OrderInfo);
            return Redirect(clientUrl + $"/404");
        }
        Guid client = identifiers[0];
        Guid courseId = identifiers[1];

        if (string.IsNullOrEmpty(vnpResponse.vnp_BankTranNo))
        {
            _logger.Warn("Failed" + vnpResponse.vnp_BankTranNo);
            return Redirect(clientUrl + $"/Payment?courseId={courseId}&failed=true");
        }

        Guid billId = Guid.NewGuid();
        CreateBillDto dto = new()
        {
            Action = PaymentDomainMessages.ACTION_PAY_COURSE,
            Note = vnpResponse.vnp_OrderInfo,
            Gateway = PaymentDomainMessages.GATEWAY_VNPAY
        };
        PaymentResponse paymentResponse = new()
        {
            Amount = vnpResponse.vnp_Amount,
            TransactionId = vnpResponse.vnp_TransactionNo,
            ClientTransactionId = vnpResponse.vnp_BankTranNo,
            Token = string.Empty,
            IsSuccessful = true
        };

        try
        {
            var billTask = billService.Create(billId, dto, paymentResponse, (Guid)clientId);
            var enrollmentTask = enrollmentService.Enroll(courseId, client, billId);
            await Task.WhenAll(billTask, enrollmentTask);
            await enrollmentService.ForceCommitAsync();
            _logger.Warn("Succeed" + vnpResponse.vnp_BankTranNo);
            return Redirect(clientUrl + $"/Course/Detail?id={courseId}");
        }
        catch
        {
            _logger.Warn("Failed false");
            return Redirect(clientUrl + $"/Payment?courseId={courseId}&failed=false");
        }
    }



    [HttpGet("Search")]
    [Authorize(Roles = RoleConstants.SYSADMIN)]
    public async Task<IActionResult> Get([FromQuery] QueryBillDto dto, [FromServices] IBillService billService)
    {
        var result = await billService.Get(dto);
        return result.AsResponse();
    }
}
