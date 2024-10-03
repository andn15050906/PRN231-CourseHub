namespace CourseHub.Core.RequestDtos.Payment;

public class CreateWithdrawalDto
{
    public string Gateway { get; set; }
    public string AccountNumber { get; set; }
    public string AccountHolderName { get; set; }

    public long Amount { get; set; }
}
