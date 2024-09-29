using CourseHub.Core.Models.User.UserModels;

namespace CourseHub.Core.Models.Payment;

public class BillModel
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public string Action { get; set; }
    public string Note { get; set; }
    public long Amount { get; set; }
    public string Gateway { get; set; }
    public string TransactionId { get; set; }
    public string ClientTransactionId { get; set; }
    public string Token { get; set; }
    public bool IsSuccessful { get; set; }

    // Navigations
    public UserMinModel Creator { get; set; }
}
