namespace CourseHub.Core.RequestDtos.Payment.BillDtos;

public class QueryBillDto
{
    public short PageIndex { get; set; }    // from 0
    public byte PageSize { get; set; } = 20;
}
