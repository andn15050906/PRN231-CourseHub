namespace CourseHub.Core.Entities.PaymentDomain;

#pragma warning disable CS8618

public class Bill : CreationAuditedEntity
{
    // Attributes
    public string Action { get; set; }                      // System Action
    public string Note { get; set; }
    public long Amount { get; set; }                        // Not double
    public string Gateway { get; set; }
    public string TransactionId { get; set; }
    public string ClientTransactionId { get; set; }
    public string Token { get; set; }
    public bool IsSuccessful { get; set; }

    // Navigations
    public User? Creator { get; set; }

#pragma warning restore CS8618

    public Bill()
    {

    }

    public Bill(
        Guid id, string action, string note, long amount, string gateway, string transactionId,
        string clientTransactionId, string token, bool isSuccessful, Guid creatorId)
    {
        Id = id;
        Action = action;
        Note = note;
        Amount = amount;
        Gateway = gateway;
        TransactionId = transactionId;
        ClientTransactionId = clientTransactionId;
        Token = token;
        IsSuccessful = isSuccessful;
        CreatorId = creatorId;
    }
}