namespace CourseHub.Core.Models.Course.CourseModels;

public class CourseMinModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }

    public string ThumbUrl { get; set; }
	public double Price { get; set; }
	public double Discount { get; set; }
	public DateTime DiscountExpiry { get; set; }
}
