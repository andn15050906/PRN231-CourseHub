namespace CourseHub.Core.RequestDtos.Payment.BillDtos;

public class CreateBillDto
{
    public string Action { get; set; }                  // PaymentDomainMessage
    public string Note { get; set; }                    // TargetId
    public string Gateway { get; set; }
}
