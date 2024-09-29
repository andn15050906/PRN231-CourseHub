namespace CourseHub.Core.Entities.PaymentDomain;

#pragma warning disable CS8618

public class PaymentAccount : CreationAuditedEntity
{
    // Attributes
    public string Gateway { get; set; }
    public string AccountNumber { get; set; }
    public string AccountHolderName { get; set; }

    // Navigations
    public User? Creator { get; set; }
}

#pragma warning restore CS8618