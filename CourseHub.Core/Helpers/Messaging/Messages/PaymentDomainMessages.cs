namespace CourseHub.Core.Helpers.Messaging.Messages;

public class PaymentDomainMessages
{
    //...
    public const string ACTION_PAY_COURSE = "Pay for course";
    public const string GATEWAY_VNPAY = "VNPay";

	// 400
	public const string INVALID_ACTION = "Invalid action";
	public const string INVALID_NOTE = "Invalid note";
}
