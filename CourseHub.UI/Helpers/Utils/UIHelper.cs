namespace CourseHub.UI.Helpers.Utils;

public static class UIHelper
{
	public static string GetBaseCourseDetailPage()
	{
		return $"{Global.PAGE_COURSE_DETAIL}?id=";
	}

	public static string GetCourseDetailPage(Guid id)
	{
		return $"{Global.PAGE_COURSE_DETAIL}?id={id}";
	}
}
